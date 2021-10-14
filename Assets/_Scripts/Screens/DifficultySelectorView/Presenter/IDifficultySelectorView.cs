using System;

public interface IDifficultySelectorView : IScreen
{
    void ShowGameDifficulty(Difficulty difficulty, Action<Difficulty> OnButtonClick);
}
