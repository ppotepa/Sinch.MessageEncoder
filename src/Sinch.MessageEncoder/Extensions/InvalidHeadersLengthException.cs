using System;
using System.Runtime.Serialization;

namespace Sinch.MessageEncoder.Extensions
{
    public class InvalidHeadersLengthException : Exception
    {
        public InvalidHeadersLengthException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}