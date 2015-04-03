using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Fhirbase.Net.Api;
using Fhirbase.Net.SearchHelpers;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using Hl7.Fhir.Serialization;
using NUnit.Framework;

namespace Fhirbase.Net.Test
{
    [TestFixture]
    public class ApiTest
    {
        public IFHIRbase FHIRbase { get; set; }

        public Patient CommonPatient { get; set; }

        public Patient SimplePatient { get; set; }

        public List<Patient> SearchPatients { get; set; }

        public Bundle RemovedPatient { get; set; }
            
        [TestFixtureSetUp]
        public void Test_Init()
        {
            FHIRbase = new FHIRbaseApi();
            CommonPatient = (Patient) FhirParser.ParseFromJson(File.ReadAllText("Examples/common_patient.json"));

            SimplePatient = new Patient();
            SimplePatient.Name.Add(HumanName.ForFamily("Hello").WithGiven("World"));
            SimplePatient.Telecom.Add(new ContactPoint
            {
                System = ContactPoint.ContactPointSystem.Phone,
                Use = ContactPoint.ContactPointUse.Mobile,
                Value = "123456789"
            });

            RemovedPatient = FHIRbase.Search("Patient");
            foreach (var entryComponent in RemovedPatient.Entry)
                FHIRbase.Delete(entryComponent.Resource);

            SearchPatients = new List<Patient>
            {
                (Patient) FHIRbase.Create(SimplePatient),
                (Patient) FHIRbase.Create(SimplePatient),
                (Patient) FHIRbase.Create(SimplePatient),
                (Patient) FHIRbase.Create(CommonPatient),
                (Patient) FHIRbase.Create(CommonPatient),
                (Patient) FHIRbase.Create(CommonPatient),
                (Patient) FHIRbase.Create(CommonPatient)
            };

        }

        [TestFixtureTearDown]
        public void Test_TearDown()
        {
            foreach (var searchPatient in SearchPatients)
                FHIRbase.Delete(searchPatient);

            foreach (var entryComponent in RemovedPatient.Entry)
                FHIRbase.Create(entryComponent.Resource);
        }

        [Test]
        public void Test_Create()
        {
            var createdPatient = (Patient)FHIRbase.Create(SimplePatient);

            Assert.That(createdPatient, Is.Not.Null);
            Assert.That(createdPatient.Id, Is.Not.Null.Or.Empty);
            Assert.That(createdPatient.HasVersionId, Is.True);
            Assert.That(createdPatient.Name.First().Family.First(), Is.EqualTo("Hello"));
            Assert.That(createdPatient.Name.First().Given.First(), Is.EqualTo("World"));
            Assert.That(createdPatient.Telecom.First().Value, Is.EqualTo("123456789"));

            FHIRbase.Delete(createdPatient);

            var createdpatient1 = FHIRbase.Create(CommonPatient);
            Assert.That(createdpatient1, Is.Not.Null);
            Assert.That(createdpatient1.Id, Is.Not.Null.Or.Empty);
            Assert.That(createdpatient1.HasVersionId, Is.True);

            var readCreated = FHIRbase.Read(createdpatient1);
            Assert.That(readCreated, Is.Not.Null);
            Assert.That(readCreated.Id, Is.Not.Null.Or.Empty);
            Assert.That(readCreated.HasVersionId, Is.True);

            FHIRbase.Delete(createdpatient1);
        }

        [Test]
        public void Test_ReadLastVersion()
        {
            var createdPatient = (Patient) FHIRbase.Create(SimplePatient);
            var updatedPatient1 = (Patient) FHIRbase.Update(createdPatient);
            var updatedPatient2 = (Patient)FHIRbase.Update(updatedPatient1);

            var history = FHIRbase.History(createdPatient.TypeName, createdPatient.Id);
            
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

        [Test]
        public void Test_Search()
        {
            var search1 = FHIRbase.Search("Patient", Search.By("family", "Hello"));
            
            Assert.That(search1.Entry, Is.Not.Null);
            Assert.That(search1.Entry.Count, Is.EqualTo(3));

            var search2 = FHIRbase.Search("Patient", Search.By("identifier.value", "12345"));

            Assert.That(search2.Entry, Is.Not.Null);
            Assert.That(search2.Entry.Count, Is.EqualTo(7));
        }

        [Test]
        public void Test_Search_StructureDefinition()
        {
            var search = FHIRbase.Search("StructureDefinition");

            Assert.That(search.Entry, Is.Not.Null);
        }

        //[Test]
        //public void Drop()
        //{
        //    var client = new FhirClient("http://fhir.zdrav.netrika.ru/fhir");
        //    var patients = client.Search<Patient>();

        //    foreach (var entryComponent in patients.Entry)
        //        client.Delete(entryComponent.Resource);
        //}
    }
}
