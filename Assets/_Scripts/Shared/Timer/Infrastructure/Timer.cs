using System;
using UniRx;

namespace UT.GameLogic
{
    public class Timer
    {
        public IDisposable disposable;
        private long _currentTotalSeconds;

        public Timer Start(double tickSeconds, int seconds, Action<long> EveryTick, Action OnTimeOut)
        {
            _currentTotalSeconds = seconds;

            disposable = Observable.Timer(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(tickSeconds))
                 .TakeWhile(t => t <= _currentTotalSeconds)
                 .Subscribe(EveryTick, OnTimeOut);

            return this;
        }

        public void AddSeconds(int seconds)
        {
            _currentTotalSeconds += seconds;
        }

        public void Stop()
        {
            if (disposable != null)
                disposable.Dispose();
        }
    }
}