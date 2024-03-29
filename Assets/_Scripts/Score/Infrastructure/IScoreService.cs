﻿using System;

namespace UT.GameLogic
{
    public interface IScoreService
    {
        void UpdateScore(float scoreIncrement);
        float Score { get; }
        float ScoreRecord { get; }
        IObservable<float> OnScoreValueChange { get; }
        void ResetCurrentScore();
    }
}