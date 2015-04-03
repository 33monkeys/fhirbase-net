using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fhirbase.Net.Common;

namespace Fhirbase.Net.SearchHelpers
{
    public class HistoryParameters
    {
        public string Count { get; set; }

        public string Since { get; set; }

        /// <summary>
        /// Возвращает форматированную строку параметров
        /// </summary>
        public override string ToString()
        {
            var result = new List<Tuple<string, string>>();

            if (!string.IsNullOrEmpty(Count))
                result.Add(new Tuple<string, string>("count", Count));

            if (!string.IsNullOrEmpty(Since))
                result.Add(new Tuple<string, string>("since", Since));

            return FHIRbaseHelper.FormatSearchString(result);
        }
    }

    public static class History
    {
        public static HistoryParameters Since(int since)
        {
            return new HistoryParameters{Since = since.ToString()};
        }

        public static HistoryParameters Count(int count)
        {
            return new HistoryParameters{Count = count.ToString()};
        }

        public static HistoryParameters Since(this HistoryParameters parameters, int since)
        {
            parameters.Since = since.ToString();
            return parameters;
        }

        public static HistoryParameters Count(this HistoryParameters parameters, int count)
        {
            parameters.Count = count.ToString();
            return parameters;
        }
    }
}
