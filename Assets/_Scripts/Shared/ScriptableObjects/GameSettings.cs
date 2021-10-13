using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Settings", fileName ="GameSettings")]
public class GameSettings : ScriptableObject
{
    [SerializeField] private Difficulty[] difficulty;

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
    [Space]
    [Header("Settings")]
    public float totalGameIntervals = 50;
    [Tooltip("How often will the time be reduced")]
    public float intervalRange = 0.5f;
    public float spawnRate = 0.5f;
    public float collisionRadius = 2f;
}