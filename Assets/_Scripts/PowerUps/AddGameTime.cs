using UnityEngine;
using UniRx;

namespace UT.GameLogic
{
    [CreateAssetMenu(menuName = "Game/PowerUps/AddGameTime", fileName = "AddGameTime")]
    public class AddGameTime : ScriptableObject, IPowerUp
    {
        [SerializeField] private GameSettings settings;
        [SerializeField] private IntReactiveProperty extraSeconds = new IntReactiveProperty(4);
        [SerializeField] private IntReactiveProperty pointsToAddExtraSeconds = new IntReactiveProperty(20);
        [SerializeField] private IntReactiveProperty extraSecondsPerPoints = new IntReactiveProperty(1);

        private IScoreService _scoreService;

        public void Initialize()
        {
            _scoreService = ScoreProvider.GetScoreService;
        }

        public void Execute()
        {
            int secondsToAdd = extraSeconds.Value + (int)(Mathf.RoundToInt(_scoreService.Score / pointsToAddExtraSeconds.Value) * extraSecondsPerPoints.Value);

            settings.SelectedGameDifficulty.addExtraTime.Value = secondsToAdd;

            Debug.Log($"{secondsToAdd} seconds added");
        }

        public void Dispose()
        {
            settings.SelectedGameDifficulty.addExtraTime.Value = 0;
        }
    }
}




