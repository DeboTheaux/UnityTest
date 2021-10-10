using System;
using UnityEngine;
using UniRx;

public class SpawnerPresenter
{
    private readonly ISpawnerView view;
    private readonly FigureFactory figureFactory;
    private readonly TimerView timerView;
    private readonly BehaviourAgent[] spawns;
    private float spawnRate;
    private float spawnStart;
    private IDisposable timerDisposable;

    public SpawnerPresenter(ISpawnerView view,
                            FigureFactory figureFactory, 
                            BehaviourAgent[] spawns,
                            TimerView timerView,
                            GameSettings gameSettings)
    {
        this.view = view;
        this.figureFactory = figureFactory;
        this.spawns = spawns;
        this.timerView = timerView;
        this.spawnRate = gameSettings.GameDifficulty.spawnRate;
    }

    public void Present()
    {
        timerDisposable = Observable.EveryUpdate()
                .Where(frame => spawnStart < Time.time && timerView.RemainingTime > 0)
                .Subscribe(frame =>
                {
                    spawnStart = Time.time + spawnRate;
                    SpawnFigure();
                });
    }

    private void SpawnFigure()
    {
        foreach (var spawn in spawns)
        {
            if (spawn.IsHittingGround())
            {
                figureFactory.CreateRandom(spawn.transform);
            }
        }
    }

    public void Dispose()
    {
        timerDisposable.Dispose();
    }
}
