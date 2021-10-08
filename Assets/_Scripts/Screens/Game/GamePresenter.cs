using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePresenter
{
    private readonly IGameView view;
    private readonly float timerSeconds;

    public GamePresenter(IGameView view, float timerSeconds)
    {
        this.view = view;
        this.timerSeconds = timerSeconds;
    }

    public void Present()
    {
        view.InitializeSpawners();
    }

    public void OnShow()
    {        
        view.StartTimer(timerSeconds);
    }

    public void EveryTick(long time)
    {
         
    }

    public void OnTimeOut()
    {
        
    }
}
