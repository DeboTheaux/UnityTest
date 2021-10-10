using System;
using UniRx;
using UnityEngine;

public class PlaneAgentBehaviour : BehaviourAgent
{
    [Header("Agent Settings")]
    public float totalLife;
    [Range(0f,1f)]
    public float speed;
    [Tooltip("amplitude in unity units in the back and forward motion")]
    public float amplitude;
    public float degreesYRotationPositive;
    public float degreesYRotationNegative;
    public float delayFrames;
    public float rayDistanceToGround = 10f;
    public LayerMask layerGround;

    private Life life;
    private Vector3 movement;
    private float z = 0;
    private float z0 = 0; //last position z
    private float degrees = 0;
    private IDisposable movementDisposable;
    private Ray ray;

    private void Start()
    {
        life = new Life(totalLife, OnDead);
        ray = new Ray();

        movementDisposable = Observable.EveryUpdate()
                .Where(frame => frame >= delayFrames && life.CurrentLife >= 0)
                .Subscribe(frame =>
                {
                    Move();
                    Rotate();
                    DetectGround();
                });
    }

    private void Move()
    {
        z += speed * Time.deltaTime;
        movement.z = Mathf.Sin(z);
        transform.localPosition = movement * amplitude;
    }

    private void Rotate()
    {
        degrees = MoveFunctionIsIncreasing() ? degreesYRotationPositive : degreesYRotationNegative;
        var rotateTo = new Vector3(transform.rotation.x, degrees, transform.rotation.z);
        transform.eulerAngles = Vector3.Lerp(transform.rotation.eulerAngles, rotateTo, 1);
    }

    private void DetectGround()
    {
        ray.origin = transform.position;
        ray.direction = Vector3.down;
    }

    private void OnDead()
    {
        movementDisposable.Dispose();
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        movementDisposable.Dispose();
    }

    private bool MoveFunctionIsIncreasing()
    {
        var isIncreasing = movement.z > z0;
        z0 = movement.z;
        return isIncreasing;
    }

    public override bool IsHittingGround()
    {
        return Physics.Raycast(ray, rayDistanceToGround, layerGround);
    }
}
