using System;
using UT.Shared;

namespace UT.GameLogic
{
    public interface IDifficultySelectorView : IScreen
    {
        void ShowGameDifficulty(Difficulty difficulty, Action<Difficulty> OnButtonClick);
    }
}

