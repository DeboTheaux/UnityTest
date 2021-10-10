using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameView : MonoBehaviour, IGameView
{
    [SerializeField] private GameSettings gameSettings;
    [SerializeField] private TimerView timerView;

    private GamePresenter presenter;

    private GamePresenter Presenter() => new GamePresenter(this, 
                                         gameSettings,
                                         new InputCatcher());

    public void Initialize()
    {
        DependencyProvider.RegisterDependency<TimerView>(timerView);
        presenter = Presenter();
        presenter.Present();
    }

    public void Show()
    {
        presenter.OnShow();
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void StartTimer(float withSeconds)
    {
        timerView.StartTimer((int)withSeconds, (_) => presenter.EveryTick(_), () => presenter.OnTimeOut());
    }

    public void InitializeSpawners()
    {
        foreach (var spawnerView in GetComponentsInChildren<SpawnerView>())
        {
            spawnerView.Initialize();
        }        
    }
}
