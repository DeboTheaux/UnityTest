using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameView : MonoBehaviour, IGameView
{
    private GamePresenter presenter;

    private GamePresenter Presenter() => new GamePresenter(this);

    public void Initialize()
    {
        presenter = Presenter();
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
