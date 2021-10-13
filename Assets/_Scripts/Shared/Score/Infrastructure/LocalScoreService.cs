using System;
using UniRx;
using UnityEngine;

public class LocalScoreService : IScoreService
{
    private const string SCOREKEY = "SCORE";
    private float cachedScoreRecord;
    private float currentScore;
    private readonly ISubject<float> scoreValueChange = new Subject<float>();

    public float Score => currentScore;
    public float ScoreRecord => cachedScoreRecord;
    public IObservable<float> OnScoreValueChange => scoreValueChange;


    public LocalScoreService()
    {
        cachedScoreRecord = PlayerPrefs.GetFloat(SCOREKEY, 0);
    }

    public void UpdateScore(float scoreIncrement)
    {
        currentScore += scoreIncrement;

        if (currentScore > cachedScoreRecord)
        {
            cachedScoreRecord = currentScore;
            PlayerPrefs.SetFloat(SCOREKEY, currentScore);
        }

        scoreValueChange.OnNext(currentScore);
    }
}