using System;
using UnityEngine;

[Serializable]
public class Figure : RecyclableObject //implementar interfaces...?
{
    [SerializeField] private FigureId figureId;

    public FigureId Id => figureId;

    internal override void Init()
    {
        Invoke("Recycle", 5f); //MEJORAR
    }

    internal override void Release()
    {
      
    }
}

[Serializable]
public struct FigureId // no problem if I change from int to string or to an enum...
{
    public int id;
}