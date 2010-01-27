using System;

namespace md2html
{
    public class ErrorMessageException : Exception
    {
        public ErrorMessageException(string message)
            : base(message)
        {
        }

        public ErrorMessageException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
