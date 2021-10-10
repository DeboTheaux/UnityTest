using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerView : MonoBehaviour, ISpawnerView
{
    [SerializeField] private FigureConfiguration configuration;
    [SerializeField] private BehaviourAgent[] spawns;

    SpawnerPresenter presenter;

    SpawnerPresenter Presenter() => new SpawnerPresenter(this, 
                                    new FigureFactory(configuration),
                                    spawns,
                                    DependencyProvider.GetDependency<TimerView>(),
                                    DependencyProvider.GetDependency<GameSettings>());

    public void Initialize()
    {
        presenter = Presenter();
        presenter.Present();
    }
}
