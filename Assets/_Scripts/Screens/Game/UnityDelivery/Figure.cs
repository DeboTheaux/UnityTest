using System;
using UnityEngine;

[Serializable]
public class Figure : RecyclableObject //implementar interfaces...?
{
    [SerializeField] private FigureId figureId;
    [SerializeField] private int scoreToAdd = 5;
    [SerializeField] private int scoreToRemove = 1;
    [SerializeField] private float collisionRadius = 10f;
    [SerializeField] private float clicksToDisappear = 1f;
    [SerializeField] private float timeToDisappear = 5f;

    public FigureId Id => figureId;
    public int ScoreToAdd => scoreToAdd;
    public int ScoreToRemove => scoreToRemove;
    public bool IsRecycled() => isRecycled;

    private int click = 0;

    internal override void Init()
    {
        click = 0;
        isRecycled = false;
        Invoke("Recycle", timeToDisappear); //TODO
    }

    internal override void Release()
    {
        isRecycled = true;
    }

    public void DoRecycle()
    {
        CancelInvoke();
        ShowEfects();
        Recycle(); //Can play an animatione before set active false.
    }

    private void ShowEfects()
    {
       //ShowEfects: animations...
    }

    public bool CheckCollision(Vector2 mousePosition) =>
        InsideCollisionRadius(mousePosition) && HasReachedAllClickToDispaear;

    private bool InsideCollisionRadius(Vector2 mousePosition) =>
          Vector2.Distance(mousePosition, ObjectInCanvas(transform.position)) < collisionRadius;

    private Vector2 ObjectInCanvas(Vector3 position) =>
       Camera.main.WorldToScreenPoint(position);

    private bool HasReachedAllClickToDispaear => (AddOneClick >= clicksToDisappear);

    private float AddOneClick => ++click; //we can show feedback to user
}

[Serializable]
public struct FigureId // no problem if I change from int to string or to an enum...
{
    public string id;

    public override string ToString()
    {
        return id;
    }
}