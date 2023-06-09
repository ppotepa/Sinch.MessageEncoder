using System;

namespace Sinch.MessageEncoder.Exceptions
{
    public class HeadersCountExceededException : Exception
    {
        public HeadersCountExceededException(string message) : base(message)
        {
        }
    }
}