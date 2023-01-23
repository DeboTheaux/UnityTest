using UnityEngine;
using UT.Shared;

namespace UT.GameLogic
{
    public class GameView : MonoBehaviour, IGameView
    {
        [SerializeField] private GameSettings gameSettings;
        [SerializeField] private TimerView timerView;
        [SerializeField] private FigureConfiguration configuration;
        [SerializeField] private BehaviourAgent[] spawns;

        private GamePresenter _presenter;

        private GamePresenter Presenter() => new GamePresenter(this,
                                             ScoreProvider.GetScoreService,
                                             gameSettings,
                                             new InputCatcher(),
                                             new FigureFactory(configuration),
                                             spawns,
                                             DependencyProvider.GetDependency<FrameNavigator>());

        public void Initialize()
        {
            _presenter = Presenter();
        }

        public void Show()
        {
            _presenter.Present();
            _presenter.OnShow();
            Debug.Log($"Selected Difficulty: {gameSettings.SelectedGameDifficulty.Name}");
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void StartTimer(float withSeconds)
        {
            timerView.StartTimer((int)withSeconds, (_) => _presenter.EveryTimeTick(_), () => _presenter.OnTimeOut());
        }

        public void StopTimer()
        {
            timerView.Stop();
        }

        public void AddTimeToTimer(int newTotalSeconds)
        {
            timerView.AddTimeToTimer(newTotalSeconds);
        }
    }
}