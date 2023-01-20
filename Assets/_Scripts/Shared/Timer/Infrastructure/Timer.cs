using System;
using UniRx;

namespace UT.GameLogic
{
    public class Timer
    {
        private IDisposable currentTimer;

        public IDisposable Start(double tickSeconds, int seconds, Action<long> EveryTick, Action OnTimeOut)
        {
            return currentTimer = Observable.Timer(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(tickSeconds))
                 .TakeWhile(t => t <= seconds)
                 .Subscribe(EveryTick, OnTimeOut);
        }

        public void Stop()
        {
            if (currentTimer != null)
                currentTimer.Dispose();
        }
    }
}