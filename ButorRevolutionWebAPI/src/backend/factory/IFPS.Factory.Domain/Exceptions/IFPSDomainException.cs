using System;

namespace IFPS.Factory.Domain.Exceptions
{
    public class IFPSDomainException : Exception
    {
        public IFPSDomainException()
        { }

        public IFPSDomainException(string message)
            : base(message)
        { }

        public IFPSDomainException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
