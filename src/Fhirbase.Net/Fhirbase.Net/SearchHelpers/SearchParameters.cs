using System;
using System.Collections.Generic;
using Fhirbase.Net.Common;

namespace Fhirbase.Net.SearchHelpers
{
    public class SearchParameters
    {
        public List<Tuple<string, string>> Parameters { get; set; }

        /// <summary>
        /// Возвращает форматированную строку параметров
        /// </summary>
        public override string ToString()
        {
            return Parameters == null 
                ? string.Empty 
                : FHIRbaseHelper.FormatSearchString(Parameters);
        }
    }

    public static class Search
    {
        public static SearchParameters By(string key, string value)
        {
            return new SearchParameters
            {
                Parameters = new List<Tuple<string, string>>
                {
                    new Tuple<string, string>(key, value)
                }
            };
        }

        public static SearchParameters By(this SearchParameters parameters, string key, string value)
        {
            if (parameters.Parameters == null)
                parameters.Parameters = new List<Tuple<string, string>>();

            parameters.Parameters.Add(new Tuple<string, string>(key, value));

            return parameters;
        }
    }
}
