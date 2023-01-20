using UnityEngine.UI;
using UnityEngine;
using UT.Shared;

namespace UT.GameLogic
{
    public class MainMenuView : MonoBehaviour, IMainMenuView
    {
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _exitButton;

        MainMenuPresenter _presenter;

        MainMenuPresenter Presenter() => new MainMenuPresenter(this,
                                         DependencyProvider.GetDependency<FrameNavigator>());

        public void Initialize()
        {
            _presenter = Presenter();

            _playButton.onClick.AddListener(() => _presenter.OnPlayButtonClick());
            _exitButton.onClick.AddListener(() => Application.Quit());
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}

