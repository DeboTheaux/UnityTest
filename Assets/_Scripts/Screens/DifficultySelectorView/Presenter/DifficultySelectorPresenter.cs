using System.Collections.Generic;

public class DifficultySelectorPresenter
{
    private readonly IDifficultySelectorView view;
    private readonly GameSettings settings;
    private readonly List<Difficulty> difficulties;
    private readonly SimpleScreenNavigator navigator;

    public DifficultySelectorPresenter(IDifficultySelectorView view, 
                                       GameSettings settings,
                                       SimpleScreenNavigator navigator)
    {
        this.view = view;
        this.settings = settings;
        this.difficulties = settings.difficulties;
        this.navigator = navigator;
    }

    public void Present()
    {
        foreach (var difficulty in difficulties)
        {
            view.ShowGameDifficulty(difficulty, (dif) => SelectDificulty(dif));
        }
    }

    private void SelectDificulty(Difficulty difficultySelected)
    {
       var selected =  difficulties.Find(difficulty => difficulty == difficultySelected);
       var index = difficulties.FindIndex(difficulty => difficulty == selected);

        settings.SetDifficultySelectedIndex(index);
        navigator.ShowScreen<IGameView>();
    }
}