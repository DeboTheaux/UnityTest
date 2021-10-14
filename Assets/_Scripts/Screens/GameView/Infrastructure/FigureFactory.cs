using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FigureFactory
{
    private readonly FigureConfiguration configuration;
    private Dictionary<FigureId, ObjectPool> pools = new Dictionary<FigureId, ObjectPool>();

    private Figure currentFigure;

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

    public Figure CreateRandom(Transform spawnTransform)
    {
        foreach (var figure in configuration.GetRandomFigure().Take(1))
        {
            currentFigure = CreateFigureById(figure.Id, spawnTransform);
        }

        return currentFigure;
    }


    public Figure CreateRandom(List<FigureSpawnProbability> probabilities, Transform spawnTransform)
    {
        var sumProbabilities = 0f;

        probabilities.ToList().ForEach(figure => sumProbabilities += figure.probability);

        var randomProb = Random.Range(0, sumProbabilities);

        foreach(var figure in probabilities)
        {
            if (figure.probability > randomProb)
              return CreateFigureById(figure.FigureId, spawnTransform);
            randomProb -= figure.probability;
        }

        return null;
    }
}
