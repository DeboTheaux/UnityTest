using System;
using UniRx;
using UnityEngine;

namespace UT.GameLogic
{
    public class LocalScoreService : IScoreService
    {
        private const string SCOREKEY = "SCORE";
        private float _cachedScoreRecord;
        private float _currentScore;
        private readonly ISubject<float> _scoreValueChange = new Subject<float>();

        public float Score => _currentScore;
        public float ScoreRecord => _cachedScoreRecord;
        public IObservable<float> OnScoreValueChange => _scoreValueChange;

        public LocalScoreService()
        {
            _cachedScoreRecord = PlayerPrefs.GetFloat(SCOREKEY, 0);
        }

        public void ResetCurrentScore()
        {
            _currentScore = 0;
        }

        public void UpdateScore(float scoreIncrement)
        {
            _currentScore += scoreIncrement;

            if (_currentScore > _cachedScoreRecord)
            {
                _cachedScoreRecord = _currentScore;
                PlayerPrefs.SetFloat(SCOREKEY, _currentScore);
            }

            _scoreValueChange.OnNext(_currentScore);
        }
    }
}