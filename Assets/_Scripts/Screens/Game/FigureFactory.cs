public class FigureFactory
{
    private readonly FigureConfiguration configuration;

    public FigureFactory(FigureConfiguration configuration)
    {
        this.configuration = configuration;
        this.configuration.Initialize();
    }

    public Figure Create(string id)
    {
        var prefab = configuration.GetFigurePrefabById(id);

        return UnityEngine.Object.Instantiate(prefab);
    }
}
