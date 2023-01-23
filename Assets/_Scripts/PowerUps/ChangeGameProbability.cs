using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UT.Shared;

namespace UT.GameLogic
{
    [CreateAssetMenu(menuName = "Game/PowerUps/ChangeGameProbability", fileName = "ChangeGameProbability")]
    public class ChangeGameProbability : ScriptableObject, IPowerUp
    {
        [SerializeField] GameSettings settings;
        [SerializeField] List<FigureSpawnProbability> newChances = new List<FigureSpawnProbability>();
        [SerializeField, Tooltip("How many times it repeats before returning to the previous probabilities")] IntReactiveProperty spawnCountToReturn = new IntReactiveProperty(1);
        [SerializeField] BoolReactiveProperty replaceAllProbabilities = new BoolReactiveProperty(false);

        private CompositeDisposable _eventsDisposable = new CompositeDisposable();
        private List<FigureSpawnProbability> _initialChances;
        private int _spawnCounter = 0;

        public void Initialize()
        {
            if (_eventsDisposable != null)
            {
                _eventsDisposable.Dispose();
                _eventsDisposable = new CompositeDisposable();
            }

            _spawnCounter = 0;
            _initialChances = settings.SelectedGameDifficulty.Chances;

            EventsProvider.EventObservable()
                   .Where(evt => evt is SpawnNewFigureEvent)
                   .Select(evt => (SpawnNewFigureEvent)evt)
                   .Subscribe(IncreaseFigureCounter)
                   .AddTo(_eventsDisposable);
        }

        public void Execute()
        {
            _spawnCounter = 0;

            if (replaceAllProbabilities.Value)
            {
                settings.SelectedGameDifficulty.Chances = newChances;
                return;
            }

            foreach (var chance in newChances)
            {
                if (AlreadyHasAFigureProbability(chance))
                {
                    RemoveExistingFigureProbability(chance);
                    AddNewFigureProbability(chance);
                }
                else
                {
                    AddNewFigureProbability(chance);
                }
            }

            bool AlreadyHasAFigureProbability(FigureSpawnProbability chance)
            {
                return settings.SelectedGameDifficulty.Chances.Exists(figureProbability => figureProbability.FigureId.Equals(chance.FigureId));
            }

            FigureSpawnProbability GetProbabilityToReplace(FigureSpawnProbability chance)
            {
                return settings.SelectedGameDifficulty.Chances.Find(figureProbability => chance.FigureId.Equals(figureProbability.FigureId));
            }

            bool RemoveExistingFigureProbability(FigureSpawnProbability chance)
            {
                return settings.SelectedGameDifficulty.Chances.Remove(GetProbabilityToReplace(chance));
            }

            void AddNewFigureProbability(FigureSpawnProbability chance)
            {
                settings.SelectedGameDifficulty.Chances.Add(chance);
            }
        }

        //TODO: Bug: If there are two objects that modify probabilities, it replace it no matter what
        private void IncreaseFigureCounter(SpawnNewFigureEvent evento)
        {
            _spawnCounter++;

            if (_spawnCounter > spawnCountToReturn.Value)
            {
                settings.SelectedGameDifficulty.Chances = _initialChances;
            }
        }
        
        public void Dispose()
        {
            settings.SelectedGameDifficulty.Chances = _initialChances;
            _eventsDisposable.Dispose();
        }

    }
}




