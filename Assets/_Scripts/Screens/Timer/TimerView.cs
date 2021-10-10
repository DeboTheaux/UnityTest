using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class TimerView : MonoBehaviour
{
    public event Action OnStartTimer = () => { };
    public event Action<long> EveryTick = (_) => { };
    public event Action OnStopTimer = () => { };

    public int Seconds { set { seconds = value; } }

    [SerializeField] private Image imageTimer;
    [SerializeField] private Text textTimer;

    private int seconds; 
    public int RemainingTime => remainingTime;
    int remainingTime;
    private IDisposable currentTimer;
   // private ISoundManager soundManager;

    private void Awake()
    {
       // soundManager = ServiceLocator.GetServices<ISoundManager>();
    }

    public void StartTimer(int seconds, Action<long> EveryTick, Action OnTimeOut)
    {
        OnStartTimer();
        this.seconds = seconds;
        remainingTime = seconds;
        this.EveryTick += EveryTick;
        currentTimer = new Timer().Start(1, seconds, UpdateTime, OnTimeOut);
    }

    public void Stop()
    {
        if (currentTimer != null)
        {
            OnStopTimer();
           // PlayTimerSound(SoundId.TimeOver);
        }
        currentTimer?.Dispose();
    }

    public void UpdateTime(long time)
    {
        remainingTime = (int)(seconds - time);
        if (imageTimer != null) UpdateImage(remainingTime, seconds);
        if (textTimer != null) UpdateText();
       // if (remainingTime <= 10) PlayTimerSound(SoundId.Timer);
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
        if (textTimer.text == remainingTime.ToString())
            return;
        textTimer.text = remainingTime.ToString();
    }

    //void PlayTimerSound(SoundId sound)
    //{
    //    soundManager.PlaySound(sound, true, true);
    //}
}


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