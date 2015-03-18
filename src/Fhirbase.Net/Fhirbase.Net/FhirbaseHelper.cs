using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;

namespace Fhirbase.Net
{
    public class FHIRbaseHelper
    {
        public static string FhirResourceToJson(Resource entry)
        {
            return FhirSerializer.SerializeResourceToJson(entry);
        }

        public static Resource JsonToFhirResource(string json)
        {
            try
            {
                return (Resource) FhirParser.ParseFromJson(json);
            }
            catch (Exception inner)
            {
                throw new InvalidOperationException("Cannot parse Fhirbase's json into a feed entry: ", inner);
            }
        }

        public static Bundle JsonToBundle(string json)
        {
            var bundle = FhirParser.ParseFromJson(json);
            return (Bundle) bundle;
        }

        public static string FormatSearchString(IEnumerable<Tuple<string, string>> parameters)
        {
            var sb = new StringBuilder();
            foreach (var parameter in parameters)
                sb.Append(string.Format("&{0}={1}", parameter.Item1, parameter.Item2));

            return sb.ToString();
        }
    }
}
