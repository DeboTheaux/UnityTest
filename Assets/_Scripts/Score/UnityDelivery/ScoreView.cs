using TMPro;
using UnityEngine;

namespace UT.GameLogic
{
    public class ScoreView : MonoBehaviour, IScoreView
    {
        [SerializeField] private TextMeshProUGUI scoreText;

        private ScorePresenter _presenter;

        private ScorePresenter Present() => new ScorePresenter(this,
                                    ScoreProvider.GetScoreService);

        private void Awake()
        {
            _presenter = Present();
        }

        private void OnEnable()
        {
            _presenter.Present();
        }

        public void ShowScore(float score, float record)
        {
            scoreText.text = $"Score: {score}, Record: {record}";
        }

        private void OnDisable()
        {
            //  presenter.Dispose();
        }
    }
}