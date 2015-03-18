using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using NpgsqlTypes;

namespace Fhirbase.Net
{
    internal class PostgresHelper
    {
        public static object Func(string funcName, params NpgsqlParameter[] parameters)
        {
            var fhirbaseConnection = ConfigurationManager.ConnectionStrings["FhirbaseConnection"].ConnectionString;
            var conn = new NpgsqlConnection(fhirbaseConnection);
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
    }
}
