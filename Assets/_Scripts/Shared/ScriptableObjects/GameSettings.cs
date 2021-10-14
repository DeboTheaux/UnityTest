using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//https://forum.unity.com/threads/display-a-list-class-with-a-custom-editor-script.227847/
[CreateAssetMenu(menuName = "Game/Settings", fileName ="GameSettings")]
public class GameSettings : ScriptableObject
{
   public List<Difficulty> difficulty;

    private void AddNew()
    {
        difficulty.Add(new Difficulty());
    }

    private void Remove(int index)
    {
        difficulty.RemoveAt(index);
    }

    public GameSettings()
    {
        DependencyProvider.RegisterDependency<GameSettings>(this);
    }

    public Difficulty GameDifficulty
    {
        get
        {
           return difficulty[0]; //TO DO
        }
    }
}

[Serializable]
public class Difficulty
{
    public string name = "Easy";
    public bool selected = false;
    public float totalGameSeconds = 50;
    public float spawnRateMin = 0.5f;
    public float spawnRateMax = 1f; //Agregar funcionalidad
    public List<FigureSpawnProbability> chances = new List<FigureSpawnProbability>();
}

[Serializable]
public struct FigureSpawnProbability
{
    public FigureId FigureId;
    public float probability;
}