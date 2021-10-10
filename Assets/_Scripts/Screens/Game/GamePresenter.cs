using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class GamePresenter
{
    private readonly IGameView view;
    private readonly float timerSeconds;
    private readonly InputCatcher input;

    private CompositeDisposable disposable = new CompositeDisposable();

    public GamePresenter(IGameView view, GameSettings gameSettings, InputCatcher input)
    {
        this.view = view;
        this.input = input;
        this.timerSeconds = gameSettings.GameDifficulty.totalGameMiliseconds;

        input.Configure();
    }

    public void Present()
    {
        view.InitializeSpawners();        
    }

    public void OnShow()
    {
        view.StartTimer(timerSeconds);
        CatchInputs();
    }

    private void CatchInputs()
    {
        input.StartCatchingInput();
        input.OnClick
          .Subscribe(OnInputBegin)
          .AddTo(disposable);
    }

    private void OnInputBegin(Vector2 inputStartPosition)
    {
        Debug.Log(inputStartPosition);
    }

    public void EveryTick(long time)
    {
       //
    }

    public void OnTimeOut()
    {
        //GameWin or Loose
        input.Dispose();
        disposable.Dispose();
    }
}
