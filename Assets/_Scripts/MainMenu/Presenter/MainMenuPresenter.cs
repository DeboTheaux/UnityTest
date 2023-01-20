using UT.Shared;

namespace UT.GameLogic
{
    public class MainMenuPresenter
    {
        private readonly IMainMenuView _view;
        private readonly FrameNavigator _navigator;

        public MainMenuPresenter(IMainMenuView view, FrameNavigator navigator)
        {
            _view = view;
            _navigator = navigator;
        }

        public void OnPlayButtonClick()
        {
            _navigator.OpenFrameByType<IDifficultySelectorView>();
        }
    }
}
