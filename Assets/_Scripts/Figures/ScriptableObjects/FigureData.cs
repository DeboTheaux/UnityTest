using System;
using UnityEngine;
using UniRx;
using UnityEngine.Events;

namespace UT.GameLogic
{
    [CreateAssetMenu(menuName = "Game/Figure Data", fileName = "FigureData")]
    public class FigureData : ScriptableObject
    {
        public FigureId figureId;
        [Header("Score Settings")]
        public IntReactiveProperty scoreToAdd = new IntReactiveProperty(5);
        public IntReactiveProperty scoreToRemove = new IntReactiveProperty(1);
        public BoolReactiveProperty isRandomScore = new BoolReactiveProperty(false);
        [ShowIf(nameof(IsRandomScore))] public IntReactiveProperty minScore = new IntReactiveProperty(1);
        [ShowIf(nameof(IsRandomScore))] public IntReactiveProperty maxScore = new IntReactiveProperty(8);
        [Header("Interaction Settings")]
        public FloatReactiveProperty collisionRadius = new FloatReactiveProperty(10f);
        public FloatReactiveProperty clicksToDisappear = new FloatReactiveProperty(1f);
        public FloatReactiveProperty timeToDisappear = new FloatReactiveProperty(5f);
        [Header("PowerUps"), Space(10)]
        public OnFigureInitializeEvent OnInitialize;
        public OnFigureDestroyEvent OnDestroy;
        public OnFigureDestroyEvent OnDispose;



        private bool IsRandomScore => isRandomScore.Value;
    }

    [Serializable]
    public class OnFigureDestroyEvent : UnityEvent
    {

    }

    [Serializable]
    public class OnFigureInitializeEvent : UnityEvent
    {

    }
}
