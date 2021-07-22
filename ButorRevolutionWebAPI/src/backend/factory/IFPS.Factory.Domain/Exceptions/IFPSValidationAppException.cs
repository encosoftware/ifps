using System;
using System.Collections.Generic;

namespace IFPS.Factory.Application.Exceptions
{
    public class IFPSValidationAppException : Exception
    {
        public readonly Dictionary<string, List<string>> Errors;

        public IFPSValidationAppException(Dictionary<string, List<string>> errors)
        {
            Errors = errors ?? throw new ArgumentNullException(nameof(errors));
        }
    }
}
