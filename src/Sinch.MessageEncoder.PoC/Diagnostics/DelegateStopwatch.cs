using System;
using System.Diagnostics;

namespace Sinch.MessageEncoder.PoC.Diagnostics
{
    internal class DelegateStopwatch : Stopwatch
    {
        private readonly Action _action;
        private readonly Action<long> _outputTo;

        public DelegateStopwatch(Action action, Action<long> outputTo)
        {
            this._action = action;
            this._outputTo = outputTo;
        }

        public long Execute(int times = 0)
        {
            long avg = 0;
            for (var i = 0; i < (times < 0 ? int.MaxValue : times); i++)
            {
                this.Start();
                _action();
                this.Stop();
                avg += ElapsedMilliseconds;
                _outputTo(ElapsedMilliseconds);
                this.Reset();
            }

            return times is 0 ? 0 : avg / times;
        }
    }
}
