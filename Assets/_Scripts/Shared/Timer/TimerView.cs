using System;
using System.Collections;
using System.Collections.Generic;
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

    public void StartTimer(int seconds, float interval, Action<long> EveryTick, Action OnTimeOut)
    {
        OnStartTimer();
        this.seconds = seconds;
        remainingTime = seconds;
        this.EveryTick += EveryTick;
        currentTimer = new Timer().Start(interval, seconds, UpdateTime, OnTimeOut);
    }

    public void Stop()
    {
        if (currentTimer != null)
        {
            OnStopTimer();
        }
        currentTimer?.Dispose();
    }

    public void UpdateTime(long time)
    {
        remainingTime = (int)(seconds - time);
        if (imageTimer != null) UpdateImage(remainingTime, seconds);
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
        if (textTimer.text == remainingTime.ToString())
            return;
        textTimer.text = remainingTime.ToString();
    }

}
