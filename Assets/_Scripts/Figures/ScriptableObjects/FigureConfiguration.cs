using System;
using System.Collections.Generic;
using UnityEngine;

namespace UT.GameLogic
{
    [CreateAssetMenu(menuName = "Game/Figures Configuration", fileName = "FigureSettings")]
    public class FigureConfiguration : ScriptableObject
    {
        [SerializeField] private Figure[] figures;
        [Tooltip("Initial Count of Figures per Figure")]
        [SerializeField] private int poolObjectCount;
        private Dictionary<FigureId, Figure> _idToFigure;

        public Figure[] Figures => figures;
        public int PoolObjectCount => poolObjectCount;

        public void Initialize()
        {
            _idToFigure = new Dictionary<FigureId, Figure>(figures.Length);
            foreach (var figure in figures)
            {
                _idToFigure.Add(figure.Id, figure);
            }
        }

        public Figure GetFigurePrefabById(FigureId id)
        {
            if (!_idToFigure.TryGetValue(id, out var figure))
            {
                throw new Exception($"Figure with id {id} does not exit");
            }
            return figure;
        }

        public IEnumerable<Figure> GetRandomFigure() => _idToFigure.RandomValues<FigureId, Figure>();
    }
}

