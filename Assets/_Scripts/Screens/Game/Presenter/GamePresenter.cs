using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class GamePresenter
{
    private readonly IGameView view;
    private readonly IScoreService scoreService;
    private readonly float timerSeconds;
    private readonly InputCatcher input;
    private readonly FigureFactory figureFactory;
    private readonly TimerView timerView;
    private readonly List<BehaviourAgent> spawns;
    private float timeIntervalInSeconds;
    private float spawnRate;
    private float spawnStart;
    private List<Figure> figuresOnScene = new List<Figure>();

    private CompositeDisposable disposable = new CompositeDisposable();

    public GamePresenter(IGameView view,
                        IScoreService scoreService,
                        GameSettings gameSettings,
                        InputCatcher input,
                        FigureFactory figureFactory,
                        BehaviourAgent[] spawns,
                        TimerView timerView)
    {
        this.view = view;
        this.scoreService = scoreService;
        this.input = input;
        this.timerSeconds = gameSettings.GameDifficulty.totalGameIntervals;
        this.view = view;
        this.figureFactory = figureFactory;
        this.spawns = spawns.ToList();
        this.timerView = timerView;
        this.spawnRate = gameSettings.GameDifficulty.spawnRate;
        this.timeIntervalInSeconds = gameSettings.GameDifficulty.intervalRange;

        input.Configure();
    }

    public void OnShow()
    {
        StartTimerWithSeconds(timerSeconds);
        CatchUserInputs();
    }

    private void CatchUserInputs()
    {
        input.StartCatchingInput();
        CatchOnClickUserInput();
    }

    public void EveryTimeTick(long tick)
    {
        SpawnOnEveryTickUntilNoRemainingTime(tick);
    }

    private void Spawn(long time)
    {
        if (TimeHasPassedSinceLastSpawn(time))
        {
            SpawnNewFigure();
            UpdateLastSpawnTime();
        }
    }

    private void OnUserInputCatch(Vector2 input)
    {
        CheckCollisionsAndRecycleFigures(input);
    }

    private void CheckCollisionsAndRecycleFigures(Vector2 mousePosition)
    {
        var collidingFigures = CollidingFigures(mousePosition).ToList();
        var recycledFigures = RecycledFigures().ToList();

        IncreaseScore(collidingFigures);
        DecreaseScore(recycledFigures);

        figuresOnScene = RemoveFiguresFromList(collidingFigures, recycledFigures).ToList();
        RemoveFigures(collidingFigures);
    }

    private void SpawnNewFigure()
    {
        foreach (var spawn in spawns)
        {
            if (spawn.IsAvailable())
            {
                var newFigure = InstantiateRandomFigureWithPosition(spawn.transform);
                figuresOnScene.Add(newFigure);
            }
        }
    }

    public void OnTimeOut()
    {
        Dispose();
    }

    private bool TimeHasPassedSinceLastSpawn(long time) => spawnStart < time;

    private void UpdateLastSpawnTime() => spawnStart = Time.time + spawnRate;

    private void SpawnOnEveryTickUntilNoRemainingTime(long tick)
    {
        if (timerView.RemainingTime > 0) Spawn(tick);
    }

    private void StartTimerWithSeconds(float seconds) => view.StartTimer(seconds, timeIntervalInSeconds);

    private void CatchOnClickUserInput() =>
        input.OnClick
             .Subscribe(OnUserInputCatch)
             .AddTo(disposable);

    private Figure InstantiateRandomFigureWithPosition(Transform transform) => figureFactory.CreateRandom(transform);

    private IEnumerable<Figure> CollidingFigures(Vector2 mousePosition) =>
         figuresOnScene.Where(figure => figure.CheckCollision(mousePosition));

    private IEnumerable<Figure> RecycledFigures() =>
        figuresOnScene.Where(figure => figure.IsRecycled());

    private IEnumerable<Figure> RemoveFiguresFromList(IEnumerable<Figure> collidingFigures, IEnumerable<Figure> recycledFigures) =>
        figuresOnScene
        .Except(collidingFigures)
        .Except(recycledFigures).ToList();

    private void IncreaseScore(List<Figure> collidingFigures) =>
       collidingFigures.ForEach(collidingFigure => scoreService.UpdateScore(collidingFigure.ScoreToAdd));

    private void DecreaseScore(List<Figure> collidingFigures) =>
    collidingFigures.ForEach(collidingFigure => scoreService.UpdateScore(collidingFigure.ScoreToRemove));

    private void RemoveFigures(List<Figure> collidingFigures) =>
        collidingFigures.ForEach(collidingFigure => collidingFigure.DoRecycle());


    private void Dispose()
    {
        input.Dispose();
        disposable.Dispose();
    }

}
