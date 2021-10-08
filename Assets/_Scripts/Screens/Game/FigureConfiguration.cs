using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Figures Configuration", fileName = "FigureSettings")]
public class FigureConfiguration : ScriptableObject
{
    [SerializeField] private Figure[] figures;
    private Dictionary<string, Figure> idToFigure;

    public void Initialize()
    {
        idToFigure = new Dictionary<string, Figure>(figures.Length);
        foreach (var figure in figures)
        {
            idToFigure.Add(figure.Id, figure);
        }
    }

    public Figure GetFigurePrefabById(string id)
    {
        if (!idToFigure.TryGetValue(id, out var figure))
        {
            throw new Exception($"Figure with id {id} does not exit");
        }
        return figure;
    }
}
