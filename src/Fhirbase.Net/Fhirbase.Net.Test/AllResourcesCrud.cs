using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Runtime.InteropServices;
using Hl7.Fhir.Model;
using Monads.NET;

namespace Fhirbase.Net.Test
{
    [TestFixture]
    class AllResourcesCrud
    {
        private const string ZipPath = "Examples/examples-json.zip";

        private const string ExtractPath = "Examples/tests";

        public List<Resource> Resources
        {
            get
            {
                FHIRbase = new FHIRbaseApi();
                ResourceTestHelper.ClearExtractPath(ExtractPath);
                ResourceTestHelper.UnzipExamples(ZipPath, ExtractPath);

                return ResourceTestHelper.LoadExamples(ExtractPath);
            }
        }

        public IFHIRbase FHIRbase { get; set; }

        [TestFixtureSetUp]
        public void Init()
        {
            FHIRbase = new FHIRbaseApi();
        }

        [TestCaseSource("Resources")]
        public void Resource_CRUD(Resource resource)
        {
            resource.Id = null;

            var createdResource = FHIRbase.Create(resource);

            Assert.That(createdResource, Is.Not.Null);
            Assert.That(createdResource.Id, Is.Not.Null.Or.Empty);
            Assert.That(createdResource.HasVersionId, Is.True);

            var readedResource = FHIRbase.Read(new ResourceKey
            {
                ResourceId = createdResource.Id,
                TypeName = createdResource.TypeName
            });

            Assert.That(readedResource, Is.Not.Null);
            Assert.That(readedResource.Id, Is.Not.Null.Or.Empty);
            Assert.That(readedResource.HasVersionId, Is.True);

            readedResource.Meta.Security.Add(new Coding("http://ehr.acme.org/identifiers/collections", "23234352356"));

            var updatedResource = FHIRbase.Update(readedResource);

            Assert.That(updatedResource, Is.Not.Null);
            Assert.That(updatedResource.Id, Is.Not.Null.Or.Empty);
            Assert.That(updatedResource.HasVersionId, Is.True);
            Assert.That(updatedResource.Meta.Security.First().Code == "23234352356");
            Assert.That(updatedResource.Meta.Security.First().System == "http://ehr.acme.org/identifiers/collections");

            FHIRbase.Delete(updatedResource);

            Assert.That(FHIRbase.IsDeleted(createdResource), Is.True);
            Assert.That(FHIRbase.IsDeleted(readedResource), Is.True);
            Assert.That(FHIRbase.IsDeleted(updatedResource), Is.True);
        }
    }
}
