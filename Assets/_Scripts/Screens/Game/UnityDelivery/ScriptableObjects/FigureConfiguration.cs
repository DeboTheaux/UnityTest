using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Figures Configuration", fileName = "FigureSettings")]
public class FigureConfiguration : ScriptableObject
{
    [SerializeField] private Figure[] figures;
    [Tooltip("Initial Count of Figures per Figure")]
    [SerializeField] private int poolObjectCount;
    private Dictionary<FigureId, Figure> idToFigure;

    public Figure[] Figures => figures;
    public int PoolObjectCount => poolObjectCount;

    public void Initialize()
    {
        idToFigure = new Dictionary<FigureId, Figure>(figures.Length);
        foreach (var figure in figures)
        {
            idToFigure.Add(figure.Id, figure);
        }
    }

    public Figure GetFigurePrefabById(FigureId id)
    {
        if (!idToFigure.TryGetValue(id, out var figure))
        {
            throw new Exception($"Figure with id {id} does not exit");
        }
        return figure;
    }

    public IEnumerable<Figure> GetRandomFigure() => idToFigure.RandomValues<FigureId, Figure>();
}
