using System;
using UniRx;
using UnityEngine;

public class PlaneAgentBehaviour : BehaviourAgent
{
    [Header("Agent Settings")]
    [Range(0f,1f)]
    public float speed;
    [Tooltip("amplitude in unity units in the back and forward motion")]
    public float amplitude;
    public float degreesYRotationPositive;
    public float degreesYRotationNegative;
    public float rayDistanceToGround = 10f;
    public LayerMask layerGround;

    private Vector3 movement;
    private float z = 0;
    private float z0 = -1; //last position z
    private float degrees = 0;
    private IDisposable movementDisposable;
    private Ray ray = new Ray();
    private bool isSpawning = false;
    private Quaternion lastRotation = new Quaternion();
    private Subject<bool> isHittingGround = new Subject<bool>();

    public override void MoveToSpawn()
    {
        movementDisposable = Observable.EveryUpdate()
                .Subscribe(frame =>
                {
                    isHittingGround.ResetSubject();
                    isSpawning = true;
                    Move();
                    Rotate();
                    DetectGround();
                }, Dispose);
    }

    private void Move()
    {
        z += speed * Time.deltaTime;
        movement.z = Mathf.Sin(z);
        transform.localPosition = movement * amplitude;
    }

    private void Rotate()
    {
        degrees = FunctionIsIncreasing() ? 
            degreesYRotationPositive : 
            degreesYRotationNegative;

        var rotateTo = new Vector3(transform.rotation.x, degrees, transform.rotation.z);
        transform.eulerAngles = Vector3.Lerp(transform.rotation.eulerAngles, rotateTo, 1);

        if(RotationChange())
        {
            isSpawning = false;
            Dispose();
        }
    }

    private void DetectGround()
    {
        ray.origin = transform.position;
        ray.direction = Vector3.down;
        isHittingGround.OnNext(Physics.Raycast(ray, rayDistanceToGround, layerGround) && IsSpawning);
    }

    private bool FunctionIsIncreasing()
    {
        var isIncreasing = movement.z > z0;
        z0 = movement.z;
        return isIncreasing;
    }

    private bool RotationChange()
    {
        bool hasChangeRotation = !(lastRotation == transform.rotation);
        lastRotation = transform.rotation;
        return hasChangeRotation;
    }

    public override bool IsAvailable => !IsSpawning;
    public override IObservable<bool> IsHittingGround => isHittingGround;
    private bool IsSpawning => isSpawning;
    public override void CompleteSpawn() => isHittingGround.OnCompleted();

    private void Dispose()
    {
        movementDisposable.Dispose();
    }
}
