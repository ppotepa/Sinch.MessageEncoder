using System;

namespace Sinch.MessageEncoder.Exceptions;

public class InvalidAmountOfPayloadPropertiesSuppliedException : Exception
{
    public InvalidAmountOfPayloadPropertiesSuppliedException(string message) : base(message)
    {
    }

}