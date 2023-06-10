using BenchmarkDotNet.Running;
using Sinch.MessageEncoder.Benchmarks.Conversions;

namespace Sinch.MessageEncoder.Benchmarks
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var summaryBinaryConversionsIntegers = BenchmarkRunner.Run<BinaryConversionsIntegers>();
            var summaryBinaryConversionsFloatingPoints = BenchmarkRunner.Run<BinaryConversionsFloatingPoints>();
        }
    }
}

