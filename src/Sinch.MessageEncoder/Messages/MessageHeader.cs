using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Sinch.MessageEncoder.Messages;

[StructLayout(LayoutKind.Explicit, Size = 66_193_471)]
public struct MessageHeader
{
    [FieldOffset(0)]
    private sbyte _headerCount;

    public sbyte HeaderCount
    {
        get
        {
            _headerCount = (sbyte)HeaderMap.Count;
            return _headerCount;
        }
    }

    [FieldOffset(3)]
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1023)]

    Dictionary<string, string> HeaderMap = default;

    public MessageHeader()
    {
        HeaderMap = new Dictionary<string, string>();
        _headerCount = 0;
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