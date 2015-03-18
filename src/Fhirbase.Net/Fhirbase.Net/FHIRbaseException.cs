using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Fhirbase.Net
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
