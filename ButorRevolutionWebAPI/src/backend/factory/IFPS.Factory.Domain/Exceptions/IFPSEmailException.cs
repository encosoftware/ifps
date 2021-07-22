using System;

namespace IFPS.Factory.Domain.Exceptions
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
