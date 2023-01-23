using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UT.Shared;

namespace UT.GameLogic
{
    public class GamePresenter
    {
        private readonly IGameView _view;
        private readonly IScoreService _scoreService;
        private readonly GameSettings _gameSettings;
        private readonly InputCatcher _input;
        private readonly FigureFactory _figureFactory;
        private readonly FrameNavigator _navigator;
        private readonly List<BehaviourAgent> _spawns;

        private float _spawnStart;
        private int _totalObjectsToSpawn = 0;
        private int _currentObjectsSpawned = 0;
        private List<Figure> _collidingFigures = new List<Figure>();
        private List<Figure> _recycledFigures = new List<Figure>();
        private List<Figure> _figuresOnScene = new List<Figure>();

        private Difficulty Settings => _gameSettings.SelectedGameDifficulty;

        private CompositeDisposable _disposable = new CompositeDisposable();
        private IDisposable _spawnerDisposable;

        public GamePresenter(IGameView view,
                            IScoreService scoreService,
                            GameSettings gameSettings,
                            InputCatcher input,
                            FigureFactory figureFactory,
                            BehaviourAgent[] spawns,
                            FrameNavigator navigator)
        {
            _view = view;
            _scoreService = scoreService;
            _input = input;
            _gameSettings = gameSettings;
            _view = view;
            _figureFactory = figureFactory;
            _spawns = spawns.ToList();
            _navigator = navigator;
        }

        public void Present()
        {
            _currentObjectsSpawned = 0;
            _totalObjectsToSpawn = GetRandomNumberOfObjectsToSpawn;
            _spawnStart = 0;

            _figuresOnScene.Clear();
        }

        public void OnShow()
        {
            StartTimerWithSeconds(Settings.TotalGameSeconds.Value);

            Settings.addExtraTime
                .Subscribe(UpdateTimer)
                .AddTo(_disposable);

            CatchUserInputs();
        }

        private void CatchUserInputs()
        {
            _input.StartCatchingInput();
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
                GameResult(UserWon ? "Game Win!" : "Game Over");
            }
        }

        private void UpdateTimer(int timeToAdd)
        {
            _view.AddTimeToTimer(timeToAdd);
        }


        private void CheckRecycleFigures()
        {
            _recycledFigures = RecycledFigures().ToList();
            DecreaseScore(_recycledFigures);
            _figuresOnScene = RemoveFiguresFromList(_recycledFigures).ToList();
        }

        private void OnUserInputCatch(Vector2 input)
        {
            CheckCollisionsFigures(input);
        }

        private void CheckCollisionsFigures(Vector2 mousePosition)
        {
            _collidingFigures = CollidingFigures(mousePosition).ToList();
            IncreaseScore(_collidingFigures);
            _figuresOnScene = RemoveFiguresFromList(_collidingFigures).ToList();
            RemoveFigures(_collidingFigures);
        }

        private void SelectAgentToSpawn()
        {
            var availableAgents = SelectAvailableAgents;
            WaitForAgentToHitGround(availableAgents);
        }

        public void OnTimeOut()
        {
            GameResult(UserWon ? "Game Win!" : "Game Over");
        }

        private void GameResult(string result)
        {
            _view.StopTimer();
            var popUp = _navigator.OpenFrameByType<IGameResultPopUpView>();
            popUp.ShowPopUpWithResult(result);
        }

        private bool TimeHasPassedSinceLastSpawn(long time) => _spawnStart < time;

        private void UpdateLastSpawnTime(long time) => _spawnStart = time + RandomSpawnRate;

        private float RandomSpawnRate => UnityEngine.Random.Range(Settings.SpawnRateMin.Value, Settings.SpawnRateMax.Value);

        private IEnumerable<BehaviourAgent> SelectAvailableAgents => _spawns.Where(agent => agent.IsAvailable);

        private void StartTimerWithSeconds(float seconds) => _view.StartTimer(seconds);

        private void CatchOnClickUserInput() =>
            _input.OnClick
                 .Subscribe(OnUserInputCatch)
                 .AddTo(_disposable);

        private bool HasObjectToSpawn => _currentObjectsSpawned < _totalObjectsToSpawn;

        private int GetRandomNumberOfObjectsToSpawn => UnityEngine.Random.Range(Settings.ObjectsAmountMin.Value, Settings.ObjectsAmountMax.Value);
        private int IncreaseObjectsSpawned() => ++_currentObjectsSpawned;

        private IEnumerable<Figure> CollidingFigures(Vector2 mousePosition) =>
             _figuresOnScene.Where(figure => figure.CheckCollision(mousePosition));

        private IEnumerable<Figure> RecycledFigures() =>
            _figuresOnScene.Where(figure => figure.IsRecycled);

        private IEnumerable<Figure> RemoveFiguresFromList(IEnumerable<Figure> figures) =>
            _figuresOnScene
            .Except(figures).ToList();

        private void IncreaseScore(List<Figure> collidingFigures) =>
           collidingFigures.ForEach(collidingFigure => _scoreService.UpdateScore(collidingFigure.ScoreToAdd));

        private void DecreaseScore(List<Figure> recycledFigures) =>
        recycledFigures.ForEach(recycledFigure => _scoreService.UpdateScore(-recycledFigure.ScoreToRemove));

        private void RemoveFigures(List<Figure> collidingFigures) =>
            collidingFigures.ForEach(collidingFigure => collidingFigure.DoRecycle());


        private void WaitForAgentToHitGround(IEnumerable<BehaviourAgent> availableAgents) =>
            availableAgents.Take(1).ToList().ForEach(agent =>
            {
                agent.MoveToSpawn();
                _spawnerDisposable = agent.IsHittingGround
                .Subscribe(isHitting =>
                {
                    if (!isHitting) return;
                    var newFigure = InstantiateRandomFigureWithPosition(agent.transform);
                    IncreaseObjectsSpawned();
                    _figuresOnScene.Add(newFigure);
                    agent.CompleteSpawn();

                    EventsProvider.EventBus().Execute(new SpawnNewFigureEvent());
                }, DisposeSpawner);
            });

        private Figure InstantiateRandomFigureWithPosition(Transform transform) => _figureFactory.CreateRandom(Settings.Chances, transform);

        private bool UserWon => _scoreService.Score >= Settings.ScoreToWin.Value;

        private void Dispose()
        {
            _input.Dispose();
            _disposable.Dispose();
        }

        private void DisposeSpawner()
        {
            _spawnerDisposable.Dispose();
        }

    }
}