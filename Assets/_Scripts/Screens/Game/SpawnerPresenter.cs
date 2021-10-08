using System.Collections;

public class SpawnerPresenter
{
    private readonly ISpawnerView view;
    private readonly FigureFactory figureFactory;

    public SpawnerPresenter(ISpawnerView view, FigureFactory figureFactory)
    {
        this.view = view;
        this.figureFactory = figureFactory;
    }

    public void SpawnPowerUp(string id)
    {
        var figureToInstance = figureFactory.Create(id);
        view.InstantiateFigure(figureToInstance);
    }
}
