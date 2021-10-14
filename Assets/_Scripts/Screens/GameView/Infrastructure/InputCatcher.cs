using System;
using UniRx;
using UnityEngine;

public class InputCatcher
{
    public IObservable<Vector2> OnClick => onClick;

    private readonly ISubject<Vector2> onClick = new Subject<Vector2>();
    private IDisposable clickDisposable;
    private RectTransform canvas;
    private Vector2 pointPosition;

    public void Configure()
    {
        canvas = DependencyProvider.GetDependency<Canvas>().transform as RectTransform;      
    }

    public void StartCatchingInput()
    {
        clickDisposable = Observable.EveryUpdate()
               .Where(frame => Input.GetMouseButtonDown(0))
               .Subscribe(frame =>
               {
                   OnPointerClick();
               });
    }

    public void OnPointerClick()
    {
        pointPosition = PointerInCanvas();
        onClick.OnNext(pointPosition);
    }

    public void Dispose()
    {
        clickDisposable.Dispose();
    }

    Vector2 PointerInCanvas() =>
            Input.mousePosition;
}
