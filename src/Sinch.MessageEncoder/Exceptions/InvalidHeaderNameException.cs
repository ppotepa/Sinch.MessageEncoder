using System;

namespace Sinch.MessageEncoder.Exceptions
{
    public class InvalidHeaderNameException : Exception
    {
        public InvalidHeaderNameException(string message) : base(message)
        {
        }
    }
}