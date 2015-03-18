using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Hl7.Fhir.Model;
using Monads.NET;

namespace Fhirbase.Net
{
    public class FHIRbaseApi : IFHIRbase
    {
        public bool IsExists(ResourceKey key)
        {
            return IsExists(key.TypeName, key.ResourceId);
        }

        public bool IsExists(Resource entry)
        {
            return entry != null
                && !string.IsNullOrEmpty(entry.TypeName)
                && !string.IsNullOrEmpty(entry.Id)
                && IsExists(entry.TypeName, entry.Id);
        }

        public bool IsExists(string resourceName, string id)
        {
            return FHIRbase.Call("fhir.is_exists")
                .WithText(resourceName)
                .WithText(id)
                .Cast<bool>();
        }

        public Resource Create(Resource entry)
        {
            var resourceJson = FHIRbaseHelper.FhirResourceToJson(entry);
            var resource = FHIRbase.Call("fhir.create")
                .WithJsonb(resourceJson)
                .Cast<string>();

            return FHIRbaseHelper.JsonToFhirResource(resource);
        }

        public bool IsDeleted(ResourceKey key)
        {
            return FHIRbase.Call("fhir.is_deleted")
                .WithText(key.TypeName)
                .WithText(key.ResourceId)
                .Cast<bool>();
        }

        public Resource Delete(ResourceKey key)
        {
            var deletedResponse = FHIRbase.Call("fhir.delete")
                .WithText(key.TypeName)
                .WithText(key.ResourceId)
                .Cast<string>();

            return FHIRbaseHelper.JsonToFhirResource(deletedResponse);
        }

        public Resource Read(ResourceKey key)
        {
            var readedResponse = FHIRbase.Call("fhir.read")
                .WithText(key.TypeName)
                .WithText(key.ResourceId)
                .Cast<string>();

            return FHIRbaseHelper.JsonToFhirResource(readedResponse);
        }

        public Resource VRead(ResourceKey key)
        {
            var readedResponse = FHIRbase.Call("fhir.vread")
                .WithText(key.TypeName)
                .WithText(key.VersionId)
                .Cast<string>();

            return FHIRbaseHelper.JsonToFhirResource(readedResponse);
        }

        public Resource Update(Resource resource)
        {
            var resourceJson = FHIRbaseHelper.FhirResourceToJson(resource);
            var updateResponse = FHIRbase.Call("fhir.update")
                .WithJsonb(resourceJson)
                .Cast<string>();

            return FHIRbaseHelper.JsonToFhirResource(updateResponse);
        }

        public Bundle History(ResourceKey key)
        {
            var historyResponse = FHIRbase.Call("fhir.history")
                .WithText(key.TypeName)
                .WithText(key.ResourceId)
                .Cast<string>();

            return FHIRbaseHelper.JsonToBundle(historyResponse);
        }

        public Resource ReadLastVersion(ResourceKey key)
        {
            throw new NotImplementedException();
        }

        public Bundle Search(string resource, IEnumerable<Tuple<string, string>> parameters)
        {
            var searchQuery = FHIRbaseHelper.FormatSearchString(parameters);
            var searchResult = FHIRbase.Call("fhir.search")
                .WithText(resource)
                .WithText(searchQuery)
                .Cast<string>();

            return FHIRbaseHelper.JsonToBundle(searchResult);
        }

        public Bundle Transaction(Bundle bundle)
        {
            var transactionJson = FHIRbaseHelper.FhirResourceToJson(bundle);
            var fhirbaseResult = FHIRbase.Call("fhir.transaction")
                .WithJsonb(transactionJson)
                .Cast<string>();

            return FHIRbaseHelper.JsonToBundle(fhirbaseResult);
        }
    }
}
