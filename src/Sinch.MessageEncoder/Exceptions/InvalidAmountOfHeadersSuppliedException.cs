using System;

namespace Sinch.MessageEncoder.Exceptions
{
    public class InvalidAmountOfHeadersSuppliedException : Exception
    {
        public InvalidAmountOfHeadersSuppliedException(string message) : base(message)
        {
        }
    }
}