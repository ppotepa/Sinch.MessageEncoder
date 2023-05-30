using System;
using System.Collections.Generic;
using System.Linq;

namespace Sinch.MessageEncoder.Messages;

public class MessageHeader
{
    private sbyte _headerCount;
    private readonly Dictionary<string, string> HeaderMap = default;

    public MessageHeader()
    {
        HeaderMap = new Dictionary<string, string>();
        _headerCount = 0;
    }

    public sbyte HeaderCount
    {
        get
        {
            _headerCount = (sbyte)HeaderMap.Count;
            return _headerCount;
        }
    }
    public string this[string key]
    {
        get => HeaderMap[key];
        set
        {
            if (key.Length > 1023) throw new InvalidOperationException();
            if (value.Length > 1023) throw new InvalidOperationException();
            HeaderMap[key] = value;
        }
    }

    public override string ToString() => string.Join("\n", HeaderMap.ToList().Select(kvp => $"{kvp.Key}||{kvp.Value}"));
}