using System;
using System.Configuration;
using System.Data;
using Npgsql;
using NpgsqlTypes;

namespace Fhirbase.Net.Common
{
    internal class PostgresHelper
    {
        public static object Func(string funcName, params NpgsqlParameter[] parameters)
        {
            var conn = new NpgsqlConnection(GetFHIRbaseConnectionString());
            conn.Open();

            try
            {
                var command = new NpgsqlCommand(funcName, conn) { CommandType = CommandType.StoredProcedure };
                command.Parameters.AddRange(parameters);
                var result = command.ExecuteScalar();
                return result;
            }
            catch (Exception ex)
            {
                throw new FHIRbaseException(
                    string.Format("Call {0} FHIRbase function failed. Reason {1}", funcName, ex.Message),
                    ex);
            }
            finally
            {
                conn.Close();
            }
        }

        private static string GetFHIRbaseConnectionString()
        {
            if (ConfigurationManager.ConnectionStrings["FhirbaseConnection"] == null)
                throw new FHIRbaseException("Add \"FhirbaseConnection\" connection string in app.config or web.config");

            return ConfigurationManager.ConnectionStrings["FhirbaseConnection"].ConnectionString;
        }

        public static NpgsqlParameter Text(string text)
        {
            return new NpgsqlParameter
            {
                NpgsqlDbType = NpgsqlDbType.Text,
                Value = text,
            };
        }

        public static NpgsqlParameter Jsonb(string text)
        {
            return new NpgsqlParameter
            {
                NpgsqlDbType = NpgsqlDbType.Jsonb,
                Value = text,
            };
        }

        public static NpgsqlParameter TextArray(string[] resources)
        {
            return new NpgsqlParameter
            {
                NpgsqlDbType = NpgsqlDbType.Array,
                Value = resources,
            };
        }

        public static NpgsqlParameter Int(int limit)
        {
            return new NpgsqlParameter
            {
                NpgsqlDbType = NpgsqlDbType.Integer,
                Value = limit,
            };
        }
    }
}
