using System;
using UniRx;
using UnityEngine;

public abstract class BehaviourAgent : MonoBehaviour
{
    public abstract bool IsAvailable { get; }
    public abstract IObservable<bool> IsHittingGround { get; }
    public abstract void MoveToSpawn();
    public abstract void CompleteSpawn();
}
