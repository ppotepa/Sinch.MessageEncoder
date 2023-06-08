﻿using Sinch.MessageEncoder.Attributes;
using Sinch.MessageEncoder.Serializers.Default;

namespace Sinch.MessageEncoder.Messages.Default.Text
{
    [UseSerializer(typeof(DefaultPayloadSerializer))]
    public class DefaultTextMessagePayload : Payload
    {
        public DefaultTextMessagePayload()
        {
        }

        public DefaultTextMessagePayload(string textMessageBody)
        {
            TextMessageBody = textMessageBody;
        }

        [SerializationOrder(Order = 1, HeaderName = nameof(TextMessageBody))]
        public string TextMessageBody { get; set; }
    }
}