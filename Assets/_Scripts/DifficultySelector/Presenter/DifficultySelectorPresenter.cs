using System.Collections.Generic;
using UT.Shared;

namespace UT.GameLogic
{

    public class DifficultySelectorPresenter
    {
        private readonly IDifficultySelectorView _view;
        private readonly GameSettings _settings;
        private readonly List<Difficulty> _difficulties;
        private readonly FrameNavigator _navigator;

        public DifficultySelectorPresenter(IDifficultySelectorView view,
                                           GameSettings settings,
                                           FrameNavigator navigator)
        {
            _view = view;
            _settings = settings;
            _difficulties = settings.difficulties;
            _navigator = navigator;
        }

        public void Present()
        {
            foreach (var difficulty in _difficulties)
            {
                _view.ShowGameDifficulty(difficulty, (dif) => SelectDificulty(dif));
            }
        }

        private void SelectDificulty(Difficulty difficultySelected)
        {
            var selected = _difficulties.Find(difficulty => difficulty == difficultySelected);
            var index = _difficulties.FindIndex(difficulty => difficulty == selected);

            _settings.SetDifficultySelectedIndex(index);
            _navigator.OpenFrameByType<IGameView>();
        }
    }
}