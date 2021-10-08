using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePresenter
{
    private readonly IGameView view;

    public GamePresenter(IGameView view)
    {
        this.view = view;
    }
}
