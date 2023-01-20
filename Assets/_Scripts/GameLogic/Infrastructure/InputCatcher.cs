using System;
using UniRx;
using UnityEngine;

namespace UT.GameLogic
{
    public class InputCatcher
    {
        public IObservable<Vector2> OnClick => _onClick;

        private readonly ISubject<Vector2> _onClick = new Subject<Vector2>();
        private IDisposable _clickDisposable;
        private Vector2 _pointPosition;

        public void StartCatchingInput()
        {
            _clickDisposable = Observable.EveryUpdate()
                   .Where(frame => Input.GetMouseButtonDown(0))
                   .Subscribe(frame =>
                   {
                       OnPointerClick();
                   });
        }

        public void OnPointerClick()
        {
            _pointPosition = PointerInCanvas();
            _onClick.OnNext(_pointPosition);
        }

        public void Dispose()
        {
            _clickDisposable.Dispose();
        }

        Vector2 PointerInCanvas() =>
                Input.mousePosition;
    }
}