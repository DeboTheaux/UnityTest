using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Settings", fileName ="GameSettings")]
public class GameSettings : ScriptableObject
{
    [SerializeField] private Difficulty[] difficulty;

    public Difficulty GameDifficulty
    {
        get
        {
           return difficulty[0]; //todo
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
    public float totalGameMiliseconds = 100;
}