using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerView : MonoBehaviour, ISpawnerView
{
    [SerializeField] private FigureConfiguration configuration;

    SpawnerPresenter presenter;

    SpawnerPresenter Presenter() => new SpawnerPresenter(this, new FigureFactory(configuration));

    public void Initialize()
    {
        presenter = Presenter();
    }

    public void InstantiateFigure(Figure figureToInstance)
    {
        Instantiate(figureToInstance, transform.position, Quaternion.identity);
    }
}
