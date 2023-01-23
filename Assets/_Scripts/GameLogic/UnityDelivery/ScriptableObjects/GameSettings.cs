using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UT.Shared;

namespace UT.GameLogic
{
    //https://forum.unity.com/threads/display-a-list-class-with-a-custom-editor-script.227847/
    [CreateAssetMenu(menuName = "Game/Settings", fileName = "GameSettings")]
    public class GameSettings : ScriptableObject
    {
        public List<Difficulty> difficulties = new List<Difficulty>();
        private int difficultySelectedIndex = 0;

        public void SetDifficultySelectedIndex(int index) => difficultySelectedIndex = index;


        private void AddNew()
        {
            difficulties.Add(new Difficulty());
        }

        private void Remove(int index)
        {
            difficulties.RemoveAt(index);
        }

        public GameSettings()
        {
            DependencyProvider.RegisterDependency<GameSettings>(this);
        }

        public Difficulty SelectedGameDifficulty
        {
            get
            {
                return difficulties[difficultySelectedIndex];
            }
        }
    }

    [Serializable]
    public class Difficulty
    {
        public string name ="Easy";
        public int scoreToWin = 100;
        public float totalGameSeconds = 50;
        public float spawnRateMin = 0.5f;
        public float spawnRateMax = 1f;
        public int objectsAmountMax = 10;
        public int objectsAmountMin = 10;

        [SerializeField] public StringReactiveProperty Name => new StringReactiveProperty(name);
        [SerializeField] public IntReactiveProperty ScoreToWin => new IntReactiveProperty(scoreToWin);
        [SerializeField] public FloatReactiveProperty TotalGameSeconds => new FloatReactiveProperty(totalGameSeconds);
        [SerializeField] public FloatReactiveProperty SpawnRateMin => new FloatReactiveProperty(spawnRateMin);
        [SerializeField] public FloatReactiveProperty SpawnRateMax => new FloatReactiveProperty(spawnRateMax);
        [SerializeField] public IntReactiveProperty ObjectsAmountMax => new IntReactiveProperty(objectsAmountMax);
        [SerializeField] public IntReactiveProperty ObjectsAmountMin => new IntReactiveProperty(objectsAmountMin);
        [SerializeField] public List<FigureSpawnProbability> Chances = new List<FigureSpawnProbability>();
        [HideInInspector] public IntReactiveProperty addExtraTime = new IntReactiveProperty(0);
    }

    [Serializable]
    public struct FigureSpawnProbability
    {
        public FigureId FigureId;
        public float probability;
    }
}