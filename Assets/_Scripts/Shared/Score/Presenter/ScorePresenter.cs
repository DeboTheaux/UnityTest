using UniRx;

public class ScorePresenter
{
    private readonly IScoreView view;
    private readonly IScoreService scoreService;

    private CompositeDisposable disposable = new CompositeDisposable();

    public ScorePresenter(IScoreView view, IScoreService scoreService)
    {
        this.view = view;
        this.scoreService = scoreService;

        view.ShowScore(0, scoreService.ScoreRecord);

        scoreService.OnScoreValueChange
            .Subscribe(UpdateScore)
            .AddTo(disposable);
    }

    private void UpdateScore(float score)
    {
        view.ShowScore(score, scoreService.ScoreRecord);
    }

    public void Dispose()
    {
        disposable.Dispose();
    }
}
