using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hl7.Fhir.Model;

namespace Fhirbase.Net
{
    /// <summary>
    /// Fhirbase RESTful+ API
    /// </summary>
    /// <exception cref="FHIRbaseException"></exception>
    public interface IFHIRbase
    {
        #region Generation

        /// <summary>
        /// Generate tables for resources
        /// </summary>
        /// <param name="resources"></param>
        /// <returns></returns>
        string GenerateTables(params string[] resources);

        /// <summary>
        /// Generate tables for DSTU2 resources
        /// </summary>
        /// <returns></returns>
        string GenerateTables();

        #endregion

        #region CRUD

        /// <summary>
        /// Read the current state of the resource
        /// </summary>
        /// <param name="key">[type] [id]</param>
        /// <returns></returns>
        Resource Read(ResourceKey key);

        /// <summary>
        /// Read the state of a specific version of the resource
        /// </summary>
        /// <param name="key">[type] [id] [vid]</param>
        /// <returns></returns>
        Resource VRead(ResourceKey key);

        /// <summary>
        /// Update an existing resource by its id (or create it if it is new)
        /// </summary>
        /// <param name="resource">[type] [id]</param>
        /// <returns></returns>
        Resource Update(Resource resource);

        /// <summary>
        /// Delete a resource
        /// </summary>
        /// <param name="key">[type] [id]</param>
        /// <returns></returns>
        Resource Delete(ResourceKey key);

        /// <summary>
        /// Retrieve the update history for a particular resource
        /// </summary>
        /// <param name="key">[type] [id] or [type]</param>
        /// <returns></returns>
        Bundle History(ResourceKey key);

        Bundle History(string resource);

        Bundle History();

        /// <summary>
        /// Create a new resource with a server assigned id
        /// </summary>
        /// <param name="resource">[type]</param>
        /// <returns></returns>
        Resource Create(Resource resource);
        
        #endregion

        #region Resource Utility

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool IsExists(ResourceKey key);

        bool IsExists(Resource resource);

        bool IsExists(string resourceName, string id);

        bool IsDeleted(ResourceKey key);

        /// <summary>
        /// Check resource is latest version
        /// </summary>
        /// <param name="key">[type] [id] [vid]</param>
        /// <returns></returns>
        bool IsLatest(ResourceKey key);

        Resource ReadLastVersion(ResourceKey key);

        #endregion

        #region Search

        /// <summary>
        /// Search the resource type based on some filter criteria
        /// </summary>
        /// <param name="resource">Resource Type</param>
        /// <param name="parameters">Name-value set</param>
        /// <returns></returns>
        Bundle Search(string resource, params Tuple<string, string>[] parameters);


        #endregion

        #region Conformance

        /// <summary>
        /// Create FHIR-conformance
        /// </summary>
        /// <param name="cfg"></param>
        /// <returns>Conformance</returns>
        Conformance Conformance(string cfg = "{}");

        StructureDefinition StructureDefinition(string resourceName, string cfg = "{}");

        #endregion

        #region Transactions

        /// <summary>
        /// Update, create or delete a set of resources as a single transaction
        /// </summary>
        /// <param name="bundle"></param>
        /// <returns></returns>
        Bundle Transaction(Bundle bundle);

        #endregion

        #region Indexing

        string IndexSearchParam(string resource, string name);

        long DropIndexSearchParams(string resource, string name);

        string[] IndexResource(string resource);

        long DropResourceIndexes(string resource);

        string[] IndexAllResources();

        long DropAllResourceIndexes();

        #endregion

        #region Admin Disk Functions

        string AdminDiskUsageTop(int limit);

        #endregion
    }
}
