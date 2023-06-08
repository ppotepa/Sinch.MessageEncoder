using System;

namespace Sinch.MessageEncoder.Exceptions
{
    public class InvalidAmountOfHeadersFound : Exception
    {
        public InvalidAmountOfHeadersFound(string message) : base(message)
        {
        }
    }
}