using TMPro;
using UnityEngine;

public class ScoreView : MonoBehaviour, IScoreView
{
    [SerializeField] private TextMeshProUGUI scoreText;

    ScorePresenter presenter;

    ScorePresenter Present() => new ScorePresenter(this, 
                                DependencyProvider.GetScoreService());

    private void Awake()
    {
        presenter = Present();
    }

    private void OnEnable()
    {
        presenter.Present();
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
