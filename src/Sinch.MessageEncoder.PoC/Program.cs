using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.InteropServices;

namespace Sinch.MessageEncoder.PoC
{
    internal class Program
    {
        #region unused_for_now
        //1024x1024
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public enum MSG_HEADER_TYPE
        {
            CONTENT_HEADER_TYPE = 1,
            MSG_SIZE = 2,
            TIMESTAMP = 3,
            CONTAINS_IMAGES = 4
        }
        #endregion 

        static unsafe void Main(string[] args)
        {
            byte[] headerInfo = {
                0x4d, 0x53, 0x47, 0x5f, 0x4c, 0x45,
                0x4e, 0x3a, 0x3a, 0x31, 0x0a, 0x4d,
                0x53, 0x47, 0x5f, 0x54, 0x59, 0x50,
                0x45, 0x3a, 0x3a, 0x54, 0x45, 0x58,
                0x54
            };

            byte[] payload =
            {
                101, 102, 103, 104, 105, 106, 107, 108,
                101, 102, 103, 104, 105, 106, 107, 108,
                101, 102, 103, 104, 105, 106, 107, 108,
                101, 102, 103, 104, 105, 106, 107, 108
            };

            MyTestMessage testMessage = default;
            fixed (byte* ptrPayload = payload, ptrHeader = headerInfo)
            {
                testMessage = new MyTestMessage();
                Marshal.Copy(payload, 0, new IntPtr(testMessage.payload), 24);
                Marshal.Copy(headerInfo, 0, new IntPtr(testMessage.headerInfo), 25);
            }

            byte[] transportBytes = new byte[sizeof(MyTestMessage)];
            
            fixed (byte* p = transportBytes)
            {
                var typed = (MyTestMessage*)p;
                *typed = testMessage;
            }

            var bytes = File.ReadAllBytes("testbinaries\\bytes.bin");
            var obj = Deserialize<MyTestMessage>(bytes);
            //Console.WriteLine(BitConverter.ToString(transportBytes));
        }

        [StructLayout(LayoutKind.Explicit)]
        public unsafe struct MyTestMessage
        {
            [FieldOffset(0)]
            [MarshalAs(UnmanagedType.I1)]
            public byte minutes;

            [FieldOffset(1)]
            [MarshalAs(UnmanagedType.I1)]
            public byte hours;

            [FieldOffset(2)]
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64 * 50)]
            public fixed byte headerInfo[64 * 50];

            [FieldOffset(3202)]
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)]
            public fixed byte payload[24];

            public MyTestMessage()
            {
                minutes = (byte)DateTime.Now.Minute;
                hours = (byte)DateTime.Now.Hour;
            }
        }

        public static unsafe T Deserialize<T>(byte[] buffer) where T : unmanaged
        {
            T result = new T();

            fixed (byte* bufferPtr = buffer)
            {
                Buffer.MemoryCopy(bufferPtr, &result, sizeof(T), sizeof(T));
            }

            return result;
        }
    }
}
