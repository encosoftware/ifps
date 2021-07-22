using System;
using System.Collections.Generic;
using System.Text;

namespace IFPS.Sales.Domain.Exceptions
{
    public class IFPSEmailException : Exception
    {
        public IFPSEmailException()
        { }

        public IFPSEmailException(string message)
            : base(message)
        { }

        public IFPSEmailException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
