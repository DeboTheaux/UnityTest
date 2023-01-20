using UniRx;

namespace UT.GameLogic
{
    public class ScorePresenter
    {
        private readonly IScoreView view;
        private readonly IScoreService scoreService;

        private CompositeDisposable disposable = new CompositeDisposable();

        public ScorePresenter(IScoreView view, IScoreService scoreService)
        {
            this.view = view;
            this.scoreService = scoreService;
        }

        public void Present()
        {
            scoreService.ResetCurrentScore();

            view.ShowScore(scoreService.Score, scoreService.ScoreRecord);

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
}