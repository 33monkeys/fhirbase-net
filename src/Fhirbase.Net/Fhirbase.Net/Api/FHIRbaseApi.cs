using System;
using System.Linq;
using Fhirbase.Net.Common;
using Fhirbase.Net.SearchHelpers;
using Hl7.Fhir.Model;
using Monads.NET;

namespace Fhirbase.Net.Api
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

        public bool IsLatest(ResourceKey key)
        {
            var result = FHIRbase.Call("fhir.is_latest")
               .WithText(key.TypeName)
               .WithText(key.ResourceId)
               .WithText(key.VersionId)
               .Cast<bool>();

            return result;
        }

        public Resource Delete(ResourceKey key)
        {
            var deletedResponse = FHIRbase.Call("fhir.delete")
                .WithText(key.TypeName)
                .WithText(key.ResourceId)
                .Cast<string>();

            return FHIRbaseHelper.JsonToFhirResource(deletedResponse);
        }

        /// <summary>
        /// Generate tables for resources
        /// </summary>
        /// <param name="resources"></param>
        /// <returns></returns>
        public string GenerateTables(params string[] resources)
        {
            var result = FHIRbase.Call("fhir.generate_tables")
                .WithTextArray(resources)
                .Cast<string>();

            return result;
        }

        /// <summary>
        /// Generate tables for DSTU2 resources
        /// </summary>
        /// <returns></returns>
        public string GenerateTables()
        {
            var result = FHIRbase.Call("fhir.generate_tables")
                .Cast<string>();

            return result;
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

        /// <summary>
        /// Retrieve the update history for a particular resource
        /// </summary>
        /// <param name="key">[type] [id] or [type]</param>
        /// <returns></returns>
        public Bundle History(ResourceKey key)
        {
            var historyResponse = FHIRbase.Call("fhir.history")
                .WithText(key.TypeName)
                .WithText(key.ResourceId)
                .Cast<string>();

            return FHIRbaseHelper.JsonToBundle(historyResponse);
        }

        public Bundle History(string resource)
        {
            throw new NotImplementedException();
        }

        public Bundle History()
        {
            throw new NotImplementedException();
        }

        public Resource ReadLastVersion(ResourceKey key)
        {
            var lastVersion = History(key)
                .With(x => x.Entry)
                .Select(x => x.Resource)
                .Where(x => x.HasVersionId)
                .OrderBy(x => x.Meta.LastUpdated)
                .LastOrDefault();

            return lastVersion;
        }

        public Bundle Search(string resource, SearchParameters parameters)
        {
            var searchQuery = parameters.ToString();
            var searchResult = FHIRbase.Call("fhir.search")
                .WithText(resource)
                .WithText(searchQuery)
                .Cast<string>();

            return FHIRbaseHelper.JsonToBundle(searchResult);
        }

        /// <summary>
        /// Create FHIR-conformance
        /// </summary>
        /// <param name="cfg"></param>
        /// <returns>JSON with conformance</returns>
        public Conformance Conformance(string cfg = "{}")
        {
            var conformanceResult = FHIRbase.Call("fhir.conformance")
                .WithJsonb(cfg)
                .Cast<string>();

            return (Conformance)FHIRbaseHelper.JsonToFhirResource(conformanceResult);
        }

        public StructureDefinition StructureDefinition(string resourceName, string cfg = "{}")
        {
            var sdResult = FHIRbase.Call("fhir.structuredefinition")
                .WithJsonb(cfg)
                .WithText(resourceName)
                .Cast<string>();

            return (StructureDefinition)FHIRbaseHelper.JsonToFhirResource(sdResult);
        }

        public Bundle Transaction(Bundle bundle)
        {
            var transactionJson = FHIRbaseHelper.FhirResourceToJson(bundle);
            var fhirbaseResult = FHIRbase.Call("fhir.transaction")
                .WithJsonb(transactionJson)
                .Cast<string>();

            return FHIRbaseHelper.JsonToBundle(fhirbaseResult);
        }

        public string IndexSearchParam(string resource, string name)
        {
            var indexSearchParamsResult = FHIRbase.Call("fhir.index_search_param")
                .WithText(resource)
                .WithText(name)
                .Cast<string>();

            return indexSearchParamsResult;
        }

        public long DropIndexSearchParams(string resource, string name)
        {
            var dropIndexSearchParamsResult = FHIRbase.Call("fhir.drop_index_search_param")
                .WithText(resource)
                .WithText(name)
                .Cast<long>();

            return dropIndexSearchParamsResult;
        }

        public string[] IndexResource(string resource)
        {
            var indexResiurceResult = FHIRbase.Call("fhir.index_resource")
                .WithText(resource)
                .Cast<string[]>();

            return indexResiurceResult;
        }

        public long DropResourceIndexes(string resource)
        {
            var dropResourceIndexesResult = FHIRbase.Call("fhir.drop_resource_indexes")
                .WithText(resource)
                .Cast<long>();

            return dropResourceIndexesResult;
        }

        public string[] IndexAllResources()
        {
            var indexAllResourcesResult = FHIRbase.Call("fhir.index_all_resources")
                .Cast<string[]>();

            return indexAllResourcesResult;
        }

        public long DropAllResourceIndexes()
        {
            var dropAllResourceIndexesResult = FHIRbase.Call("fhir.drop_all_resource_indexes")
                .Cast<long>();

            return dropAllResourceIndexesResult;
        }

        public string AdminDiskUsageTop(int limit)
        {
            var adminDiskUsageTopResult = FHIRbase.Call("fhir.admin_disk_usage_top")
                .WithInt(limit)
                .Cast<string>();

            return adminDiskUsageTopResult;
        }
    }
}
