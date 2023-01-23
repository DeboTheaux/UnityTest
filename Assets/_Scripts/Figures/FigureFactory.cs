using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UT.Shared;

namespace UT.GameLogic
{
    public class FigureFactory
    {
        private readonly FigureConfiguration _configuration;
        private Dictionary<FigureId, ObjectPool> _pools = new Dictionary<FigureId, ObjectPool>();

        private Figure _currentFigure;

        public FigureFactory(FigureConfiguration configuration)
        {
            _configuration = configuration;
            _configuration.Initialize();

            foreach (var figure in configuration.Figures)
            {
                var objectPool = new ObjectPool(figure);
                objectPool.Init(configuration.PoolObjectCount, figure.transform);
                _pools.Add(figure.Id, objectPool);
                figure.figureData.OnInitialize?.Invoke();
            }
        }

        public Figure CreateFigureById(FigureId id, Transform spawnTransform)
        {
            var objectPool = _pools[id];

            return objectPool.Spawn<Figure>(spawnTransform);
        }

        public Figure CreateRandom(Transform spawnTransform)
        {
            foreach (var figure in _configuration.GetRandomFigure().Take(1))
            {
                _currentFigure = CreateFigureById(figure.Id, spawnTransform);
            }

            return _currentFigure;
        }


        public Figure CreateRandom(List<FigureSpawnProbability> probabilities, Transform spawnTransform)
        {
            var sumProbabilities = 0f;

            probabilities.ToList().ForEach(figure => sumProbabilities += figure.probability);

            var randomProb = Random.Range(0, sumProbabilities);

            foreach (var figure in probabilities)
            {
                if (figure.probability > randomProb)
                    return CreateFigureById(figure.FigureId, spawnTransform);
                randomProb -= figure.probability;
            }

            return null;
        }
    }
}
