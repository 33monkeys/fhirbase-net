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
            var patient = new Patient();
            patient.Name.Add(HumanName.ForFamily("Hello").WithGiven("World"));
            patient.Telecom.Add(new ContactPoint
            {
                System = ContactPoint.ContactPointSystem.Phone, 
                Use = ContactPoint.ContactPointUse.Mobile,
                Value = "123456789"
            });

            var createdPatient = (Patient)FHIRbase.Create(patient);

            Assert.That(createdPatient, Is.Not.Null);
            Assert.That(createdPatient.HasVersionId, Is.True);
            Assert.That(createdPatient.Name.First().Family.First(), Is.EqualTo("Hello"));
            Assert.That(createdPatient.Name.First().Given.First(), Is.EqualTo("World"));
            Assert.That(createdPatient.Telecom.First().Value, Is.EqualTo("123456789"));

            FHIRbase.Delete(createdPatient);
        }
    }
}
