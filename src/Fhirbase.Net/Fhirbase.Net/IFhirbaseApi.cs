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
    public interface IFhirbaseApi
    {
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

        Resource ReadLastVersion(ResourceKey key);

        #endregion

        #region Search

        /// <summary>
        /// Search the resource type based on some filter criteria
        /// </summary>
        /// <param name="resource">Resource Type</param>
        /// <param name="parameters">Name-value set</param>
        /// <returns></returns>
        Bundle Search(string resource, IEnumerable<Tuple<string, string>> parameters);

        #endregion

        #region History

        //TODO: Retrieve the update history for a particular resource type

        #endregion

        #region Validation

        //TODO: Check that the content would be acceptable as an update

        #endregion

        #region Conformance

        //TODO: Get a conformance statement for the system

        #endregion

        #region Transactions

        /// <summary>
        /// Update, create or delete a set of resources as a single transaction
        /// </summary>
        /// <param name="bundle"></param>
        /// <returns></returns>
        Bundle Transaction(Bundle bundle);

        #endregion
    }
}
