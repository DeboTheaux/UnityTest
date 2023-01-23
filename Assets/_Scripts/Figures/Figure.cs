using System;
using UnityEngine;
using Random = UnityEngine.Random;
using UT.Shared;

namespace UT.GameLogic
{
    [Serializable]
    public class Figure : RecyclableObject //implementar interfaces...?
    {
        [Header("Figure Data")]
        [SerializeField] public FigureData figureData;
        [Header("Components")]
        [SerializeField] private Rigidbody rigidbodyComponent;

        public FigureId Id => figureData.figureId;
        public int ScoreToAdd => figureData.isRandomScore.Value ? Random.Range(figureData.minScore.Value, figureData.maxScore.Value) : figureData.scoreToAdd.Value;
        public int ScoreToRemove => figureData.scoreToRemove.Value;
        public bool IsRecycled => isRecycled;

        private int _click = 0;

        internal override void Init()
        {
            rigidbodyComponent.velocity = Vector3.zero;
            _click = 0;
            isRecycled = false;
            Invoke("Recycle", figureData.timeToDisappear.Value); //TODO
        }

        internal override void Release()
        {
            isRecycled = true;
        }

        public void DoRecycle()
        {
            CancelInvoke();
            ShowEfects();
            figureData.OnDestroy?.Invoke();
            Recycle(); //Can play an animatione before set active false.
        }
                
        private void ShowEfects()
        {
            //ShowEfects: animations...
        }

        private void OnApplicationQuit()
        {
            figureData.OnDispose?.Invoke();
        }

        public bool CheckCollision(Vector2 mousePosition) =>
            InsideCollisionRadius(mousePosition) && HasReachedAllClickToDispaear;

        private bool InsideCollisionRadius(Vector2 mousePosition) =>
              Vector2.Distance(mousePosition, ObjectInCanvas(transform.position)) < figureData.collisionRadius.Value;

        private Vector2 ObjectInCanvas(Vector3 position) =>
           Camera.main.WorldToScreenPoint(position);

        private bool HasReachedAllClickToDispaear => (AddOneClick >= figureData.clicksToDisappear.Value);

        private float AddOneClick => ++_click; //we can show feedback to user
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
}