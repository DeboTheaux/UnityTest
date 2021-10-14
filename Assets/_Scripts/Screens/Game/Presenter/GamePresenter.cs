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
    private readonly float spawnRateMin;
    private readonly float spawnRateMax;
    private readonly List<FigureSpawnProbability> chances;
    private float spawnStart;
    private List<Figure> figuresOnScene = new List<Figure>();

    private CompositeDisposable disposable = new CompositeDisposable();
    private IDisposable spawnerDisposable;

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
        this.timerSeconds = gameSettings.GameDifficulty.totalGameSeconds;
        this.view = view;
        this.figureFactory = figureFactory;
        this.spawns = spawns.ToList();
        this.timerView = timerView;
        this.spawnRateMin = gameSettings.GameDifficulty.spawnRateMin;
        this.spawnRateMax = gameSettings.GameDifficulty.spawnRateMax;
        this.chances = gameSettings.GameDifficulty.chances;

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
            SelectAgentToSpawn();
            UpdateLastSpawnTime(time);
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

    private void SelectAgentToSpawn()
    {
        var availableAgents = SelectAvailableAgents;
        WaitForAgentToHitGround(availableAgents);       
    }

    public void OnTimeOut()
    {
        Dispose();
    }

    private bool TimeHasPassedSinceLastSpawn(long time) => spawnStart < time;

    private void UpdateLastSpawnTime(long time) => spawnStart = time + RandomSpawnRate;

    private float RandomSpawnRate => UnityEngine.Random.Range(spawnRateMin, spawnRateMax);

    private void SpawnOnEveryTickUntilNoRemainingTime(long tick) => Spawn(tick);

    private IEnumerable<BehaviourAgent> SelectAvailableAgents => spawns.Where(agent => agent.IsAvailable);

    private void StartTimerWithSeconds(float seconds) => view.StartTimer(seconds);

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

    private void DecreaseScore(List<Figure> recycledFigures) =>
    recycledFigures.ForEach(recycledFigure => scoreService.UpdateScore(-recycledFigure.ScoreToRemove));

    private void RemoveFigures(List<Figure> collidingFigures) =>
        collidingFigures.ForEach(collidingFigure => collidingFigure.DoRecycle());


    private void WaitForAgentToHitGround(IEnumerable<BehaviourAgent> availableAgents) =>  
        availableAgents.Take(1).ToList().ForEach(agent =>
        {
            agent.MoveToSpawn();
            spawnerDisposable = agent.IsHittingGround
            .Subscribe(isHitting =>
            {
                if (!isHitting) return;
                var newFigure = InstantiateRandomFigureWithPosition(agent.transform);
                figuresOnScene.Add(newFigure);
                agent.CompleteSpawn();
            }, DisposeSpawner);
        });

    private void Dispose()
    {
        input.Dispose();
        disposable.Dispose();
    }

    private void DisposeSpawner()
    {
        spawnerDisposable.Dispose();
    }

}
