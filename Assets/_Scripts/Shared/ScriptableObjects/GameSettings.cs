using System;
using System.Collections.Generic;
using UnityEngine;
using UT.Shared;

namespace UT.GameLogic
{
    //https://forum.unity.com/threads/display-a-list-class-with-a-custom-editor-script.227847/
    [CreateAssetMenu(menuName = "Game/Settings", fileName = "GameSettings")]
    public class GameSettings : ScriptableObject
    {
        public List<Difficulty> difficulties;
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
        public string name = "Easy";
        public int scoreToWin = 100;
        public float totalGameSeconds = 50;
        public float spawnRateMin = 0.5f;
        public float spawnRateMax = 1f; //Agregar funcionalidad
        public int objectsAmountMax = 10;
        public int objectsAmountMin = 10;
        public List<FigureSpawnProbability> chances = new List<FigureSpawnProbability>();
    }

    [Serializable]
    public struct FigureSpawnProbability
    {
        public FigureId FigureId;
        public float probability;
    }
}