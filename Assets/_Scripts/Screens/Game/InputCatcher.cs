using System;
using UniRx;
using UnityEngine;

public class InputCatcher
{
    public IObservable<Vector2> OnClick => onClick;

    private readonly ISubject<Vector2> onClick = new Subject<Vector2>();
    private IDisposable clickDisposable;
    private Transform cameraFieldOfView;
    private Vector2 pointPosition;

    public void Configure()
    {
        cameraFieldOfView = DependencyProvider.GetDependency<Camera>().transform;//as Transform;      
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
        pointPosition = PointerInCanvas(Input.mousePosition);
        onClick.OnNext(pointPosition);
    }

    public void Dispose()
    {
        clickDisposable.Dispose();
    }

    Vector2 PointerInCanvas(Vector2 position) =>
            CanvasPosition(NormalizePosition(position));

    Vector2 CanvasPosition(Vector2 normalizedPosition) =>
            Vector2.Scale(normalizedPosition, cameraFieldOfView.localScale);

    static Vector2 NormalizePosition(Vector2 position) =>
            new Vector2(position.x / Screen.width, position.y / Screen.height);
}
