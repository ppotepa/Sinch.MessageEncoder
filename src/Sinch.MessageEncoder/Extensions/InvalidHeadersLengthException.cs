using System;

namespace Sinch.MessageEncoder.Extensions
{
    internal class InvalidHeadersLengthException : Exception
    {
        public InvalidHeadersLengthException(string message, Exception inner) : base(message, inner)
        {
        
        }
    }
}