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
    private readonly int objectsAmountMax;
    private readonly int objectsAmountMin;
    private readonly List<FigureSpawnProbability> figuresSpawnProbability;
    private float spawnStart;
    private int totalObjectsToSpawn = 0;
    private int currentObjectsSpawned = 0;
    private List<Figure> collidingFigures = new List<Figure>();
    private List<Figure> recycledFigures = new List<Figure>();
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
        this.timerSeconds = gameSettings.SelectedGameDifficulty.totalGameSeconds;
        this.view = view;
        this.figureFactory = figureFactory;
        this.spawns = spawns.ToList();
        this.timerView = timerView;
        this.spawnRateMin = gameSettings.SelectedGameDifficulty.spawnRateMin;
        this.spawnRateMax = gameSettings.SelectedGameDifficulty.spawnRateMax;
        this.figuresSpawnProbability = gameSettings.SelectedGameDifficulty.chances;
        this.objectsAmountMax = gameSettings.SelectedGameDifficulty.objectsAmountMax;
        this.objectsAmountMin = gameSettings.SelectedGameDifficulty.objectsAmountMin;

        totalObjectsToSpawn = GetRandomNumberOfObjectsToSpawn;

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

    public void EveryTimeTick(long time)
    {
        CheckRecycleFigures();

        if (TimeHasPassedSinceLastSpawn(time) && HasObjectToSpawn)
        {
            SelectAgentToSpawn();
            UpdateLastSpawnTime(time);
        }
        else if (!HasObjectToSpawn)
        {
            GameResult();
        }
    }

    private void CheckRecycleFigures()
    {
        recycledFigures = RecycledFigures().ToList();
        DecreaseScore(recycledFigures);
        figuresOnScene = RemoveFiguresFromList(recycledFigures).ToList();
    }

    private void OnUserInputCatch(Vector2 input)
    {
        CheckCollisionsFigures(input);
    }

    private void CheckCollisionsFigures(Vector2 mousePosition)
    {
        collidingFigures = CollidingFigures(mousePosition).ToList();
        IncreaseScore(collidingFigures);
        figuresOnScene = RemoveFiguresFromList(collidingFigures).ToList();
        RemoveFigures(collidingFigures);
    }

    private void SelectAgentToSpawn()
    {
        var availableAgents = SelectAvailableAgents;
        WaitForAgentToHitGround(availableAgents);       
    }

    public void OnTimeOut()
    {
        GameResult();
        Dispose();
    }

    private void GameResult()
    {
        //Check if Win Or Loose
        Debug.Log("Time Out");
    }

    private bool TimeHasPassedSinceLastSpawn(long time) => spawnStart < time;

    private void UpdateLastSpawnTime(long time) => spawnStart = time + RandomSpawnRate;

    private float RandomSpawnRate => UnityEngine.Random.Range(spawnRateMin, spawnRateMax);

    private IEnumerable<BehaviourAgent> SelectAvailableAgents => spawns.Where(agent => agent.IsAvailable);

    private void StartTimerWithSeconds(float seconds) => view.StartTimer(seconds);

    private void CatchOnClickUserInput() =>
        input.OnClick
             .Subscribe(OnUserInputCatch)
             .AddTo(disposable);

    private bool HasObjectToSpawn => ObjectsSpawned < totalObjectsToSpawn;

    private int GetRandomNumberOfObjectsToSpawn => UnityEngine.Random.Range(objectsAmountMin, objectsAmountMax);
    private int ObjectsSpawned => currentObjectsSpawned;
    private int IncreaseObjectsSpawned() => ++currentObjectsSpawned;

    private IEnumerable<Figure> CollidingFigures(Vector2 mousePosition) =>
         figuresOnScene.Where(figure => figure.CheckCollision(mousePosition));

    private IEnumerable<Figure> RecycledFigures() =>
        figuresOnScene.Where(figure => figure.IsRecycled);

    private IEnumerable<Figure> RemoveFiguresFromList(IEnumerable<Figure> figures) =>
        figuresOnScene
        .Except(figures).ToList();

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
                IncreaseObjectsSpawned();
                figuresOnScene.Add(newFigure);
                agent.CompleteSpawn();
            }, DisposeSpawner);
        });

    private Figure InstantiateRandomFigureWithPosition(Transform transform) => figureFactory.CreateRandom(figuresSpawnProbability, transform);

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
