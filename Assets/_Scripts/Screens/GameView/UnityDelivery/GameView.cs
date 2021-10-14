﻿using TMPro;
using UnityEngine;

public class GameView : MonoBehaviour, IGameView
{
    [SerializeField] private GameSettings gameSettings;
    [SerializeField] private TimerView timerView;
    [SerializeField] private FigureConfiguration configuration;
    [SerializeField] private BehaviourAgent[] spawns;

    private GamePresenter presenter;

    private GamePresenter Presenter() => new GamePresenter(this,
                                         DependencyProvider.GetScoreService(),
                                         gameSettings,
                                         new InputCatcher(),
                                         new FigureFactory(configuration),
                                         spawns,
                                         timerView);

    public void Initialize()
    {
        presenter = Presenter();
    }

    public void Show()
    {
        presenter.OnShow();
        Debug.Log($"Selected Difficulty: {gameSettings.SelectedGameDifficulty.name}");
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void StartTimer(float withSeconds)
    {
        timerView.StartTimer((int)withSeconds, (_) => presenter.EveryTimeTick(_), () => presenter.OnTimeOut());
    }

}