using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using Monads.NET;

namespace Fhirbase.Net.Test
{
    internal class ResourceTestHelper
    {
        public static void ClearExtractPath(string extractPath)
        {
            if (Directory.Exists(extractPath))
                foreach (var file in Directory.EnumerateFiles(extractPath))
                    File.Delete(file);
        }

        public static void UnzipExamples(string zipPath, string extractPath)
        {
            if (!Directory.Exists(extractPath))
                Directory.CreateDirectory(extractPath);

            using (var archive = ZipFile.OpenRead(zipPath))
            {
                foreach (var entry in archive.Entries
                    .Where(entry => entry.FullName.Contains("example.json") /*|| entry.FullName.Contains("example-")*/))
                {
                    entry.ExtractToFile(Path.Combine(extractPath, entry.FullName));
                }
            }
        }

        public static List<Resource> LoadExamples(string extractPath)
        {
            var result = Directory.EnumerateFiles(extractPath)
                .Select(File.ReadAllText)
                .Select(resourceJson => resourceJson.TryLet(x => FhirParser.ParseFromJson(x)).Ignore())
                .Where(x => x != null)
                .Cast<Resource>()
                .ToList();

            return result;
        }
    }
}