using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuPresenter
{
   private readonly IMainMenuView view;
   private readonly SimpleScreenNavigator navigator;

    public MainMenuPresenter(IMainMenuView view, SimpleScreenNavigator navigator)
    {
        this.view = view;
        this.navigator = navigator;
    }

    public void OnPlayButtonClick()
    {
        navigator.ShowScreen<IDifficultySelectorView>();   
    }
}
