using System;

namespace IFPS.Factory.Application.Exceptions
{
    public class IFPSAppException : Exception
    {
        public ErrorCode? ErrorCode { get; set; }

        public IFPSAppException()
        { }

        public IFPSAppException(string message)
            : base(message)
        { }

        public IFPSAppException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
