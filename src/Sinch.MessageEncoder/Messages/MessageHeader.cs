using System;
using System.Collections.Generic;
using System.Linq;

namespace Sinch.MessageEncoder.Messages;

public struct MessageHeader
{
    public readonly Dictionary<string, string> _headerMap = default;
    private const string SPACE = " ";
    public MessageHeader()
    {
        _headerMap = new Dictionary<string, string>();
    }

    public string this[string key]
    {
        get => _headerMap[key];
        set
        {
            if (key.Length > 1023) throw new InvalidOperationException();
            if (value.Length > 1023) throw new InvalidOperationException();
            _headerMap[key] = value;
        }
    }

    public override string ToString()
    {
        return string.Join("\n", _headerMap.ToList().Select(kvp => $"{kvp.Key}||{kvp.Value}"));
    }
}