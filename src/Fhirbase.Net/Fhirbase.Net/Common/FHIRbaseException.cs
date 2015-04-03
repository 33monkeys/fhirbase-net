using System;

namespace Fhirbase.Net.Common
{
    public class FHIRbaseException : Exception
    {
        public FHIRbaseException(string message) : base(message)
        {
        }

        public FHIRbaseException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
