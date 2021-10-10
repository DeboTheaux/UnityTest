using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FigureFactory
{
    private readonly FigureConfiguration configuration;
    private Dictionary<FigureId, ObjectPool> pools = new Dictionary<FigureId, ObjectPool>();


    public FigureFactory(FigureConfiguration configuration)
    {
        this.configuration = configuration;
        this.configuration.Initialize();

        foreach (var figure in configuration.Figures)
        {
            var objectPool = new ObjectPool(figure);
            objectPool.Init(configuration.PoolObjectCount, figure.transform);
            pools.Add(figure.Id, objectPool);
        }
    }

    public Figure CreateFigureById(FigureId id, Transform spawnTransform)
    {

        var objectPool = pools[id];

        return objectPool.Spawn<Figure>(spawnTransform);
    }

    public void CreateRandom(Transform spawnTransform)
    {
        foreach (var figure in configuration.GetRandomFigure().Take(1))
        {
            CreateFigureById(figure.Id, spawnTransform);
        }
    }
}
