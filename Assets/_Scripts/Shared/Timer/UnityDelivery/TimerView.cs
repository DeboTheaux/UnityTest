using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UT.GameLogic
{
    public class TimerView : MonoBehaviour
    {
        public event Action OnStartTimer = () => { };
        public event Action<long> EveryTick = (_) => { };
        public event Action OnStopTimer = () => { };

        public event Action OnTimeOut = () => { };

        public int Seconds { set { _seconds = value; } }

        [SerializeField] private Image imageTimer;
        [SerializeField] private TextMeshProUGUI textTimer;

        public int RemainingTime => _remainingTime;

        private int _seconds;
        private int _remainingTime;
        private Timer _currentTimer;
        private IDisposable _disposable;

        public void StartTimer(int seconds, Action<long> EveryTick, Action OnTimeOut)
        {
            _seconds = seconds;
            _remainingTime = seconds;
            this.EveryTick += EveryTick;
            this.OnTimeOut += OnTimeOut;
            _currentTimer = new Timer().Start(1, seconds, UpdateTime, OnTimeOut);
            _disposable = _currentTimer.disposable;
            OnStartTimer();
        }

        public void Stop()
        {
            if (_disposable != null)
            {
                OnStopTimer();
            }
            _disposable?.Dispose();
        }

        public void UpdateTime(long time)
        {
            _remainingTime = (int)(_seconds - time);
            if (imageTimer != null) UpdateImage(_remainingTime, _seconds);
            if (textTimer != null) UpdateText();
            EveryTick(time);
        }

        void UpdateImage(int remainingT, int totalTime)
        {
            if (totalTime == 0)
                return;
            imageTimer.fillAmount = (float)remainingT / totalTime;
        }

        void UpdateText()
        {
            if (textTimer.text == _remainingTime.ToString())
                return;
            textTimer.text = _remainingTime.ToString();
        }

        public void AddTimeToTimer(int secondsToAdd)
        {
            _seconds += secondsToAdd;
            _currentTimer.AddSeconds(secondsToAdd);
        }
    }
}
