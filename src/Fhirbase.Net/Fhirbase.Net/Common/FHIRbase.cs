using System.Collections.Generic;
using Npgsql;

namespace Fhirbase.Net.Common
{
    public static class FHIRbase
    {
        /// <summary>
        /// Create FHIRbase function
        /// </summary>
        /// <param name="fhirbaseFunc"></param>
        /// <returns></returns>
        public static FHIRbaseFunc Call(string fhirbaseFunc)
        {
            return new FHIRbaseFunc {Name = fhirbaseFunc};
        }

        /// <summary>
        /// Add text parameter
        /// </summary>
        /// <param name="func"></param>
        /// <param name="textParameter"></param>
        /// <returns></returns>
        public static FHIRbaseFunc WithText(this FHIRbaseFunc func, string textParameter)
        {
            func.Parameters.Add(PostgresHelper.Text(textParameter));
            return func;
        }

        /// <summary>
        /// Add jsonb parameter
        /// </summary>
        /// <param name="func"></param>
        /// <param name="jsonParameter"></param>
        /// <returns></returns>
        public static FHIRbaseFunc WithJsonb(this FHIRbaseFunc func, string jsonParameter)
        {
            func.Parameters.Add(PostgresHelper.Jsonb(jsonParameter));
            return func;
        }

        public static FHIRbaseFunc WithTextArray(this FHIRbaseFunc func, string[] resources)
        {
            func.Parameters.Add(PostgresHelper.TextArray(resources));
            return func;
        }

        public static FHIRbaseFunc WithInt(this FHIRbaseFunc func, int limit)
        {
            func.Parameters.Add(PostgresHelper.Int(limit));
            return func;
        }

        /// <summary>
        /// Call FHIRbase function and cast value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        public static T Cast<T>(this FHIRbaseFunc func)
        {
            return (T)PostgresHelper.Func(func.Name, func.Parameters.ToArray());
        }

        public class FHIRbaseFunc
        {
            public FHIRbaseFunc()
            {
                Parameters = new List<NpgsqlParameter>();
            }

            public string Name { get; set; }

            public List<NpgsqlParameter> Parameters { get; set; }
        }
    }
}
