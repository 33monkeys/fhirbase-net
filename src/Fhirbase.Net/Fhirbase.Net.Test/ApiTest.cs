using System;
using System.Linq;
using Hl7.Fhir.Model;
using NUnit.Framework;

namespace Fhirbase.Net.Test
{
    [TestFixture]
    public class ApiTest
    {
        public IFHIRbase FHIRbase { get; set; }

        [TestFixtureSetUp]
        public void Test_Init()
        {
            FHIRbase = new FHIRbaseApi();
        }

        [Test]
        public void Test_Create()
        {
            var patient = GetPatient();
            var createdPatient = (Patient)FHIRbase.Create(patient);

            Assert.That(createdPatient, Is.Not.Null);
            Assert.That(createdPatient.Id, Is.Not.Null.Or.Empty);
            Assert.That(createdPatient.HasVersionId, Is.True);
            Assert.That(createdPatient.Name.First().Family.First(), Is.EqualTo("Hello"));
            Assert.That(createdPatient.Name.First().Given.First(), Is.EqualTo("World"));
            Assert.That(createdPatient.Telecom.First().Value, Is.EqualTo("123456789"));

            FHIRbase.Delete(createdPatient);
        }

        [Test]
        public void Test_ReadLastVersion()
        {
            var patient = GetPatient();
            var createdPatient = (Patient) FHIRbase.Create(patient);
            var updatedPatient1 = (Patient) FHIRbase.Update(createdPatient);
            var updatedPatient2 = (Patient)FHIRbase.Update(updatedPatient1);

            var history = FHIRbase.History(createdPatient);
            
            Assert.That(history, Is.Not.Null);
            Assert.That(history.Entry, Is.Not.Null);
            Assert.That(history.Entry.Count, Is.EqualTo(3));

            var lastPatientVersion = (Patient)FHIRbase.ReadLastVersion(createdPatient);

            Assert.That(lastPatientVersion, Is.Not.Null);
            Assert.That(lastPatientVersion.VersionId, Is.EqualTo(updatedPatient2.VersionId));

            FHIRbase.Delete(createdPatient);

            Assert.That(FHIRbase.IsDeleted(createdPatient), Is.True);
            Assert.That(FHIRbase.IsExists(createdPatient), Is.False);
        }

        private static Patient GetPatient()
        {
            var patient = new Patient();
            patient.Name.Add(HumanName.ForFamily("Hello").WithGiven("World"));
            patient.Telecom.Add(new ContactPoint
            {
                System = ContactPoint.ContactPointSystem.Phone,
                Use = ContactPoint.ContactPointUse.Mobile,
                Value = "123456789"
            });
            return patient;
        }
    }
}
