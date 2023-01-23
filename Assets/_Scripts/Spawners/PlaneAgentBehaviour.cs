using System;
using UniRx;
using UnityEngine;

namespace UT.GameLogic
{
    public class PlaneAgentBehaviour : BehaviourAgent
    {
        [Header("Agent Settings")]
        [Range(0f, 1f)]
        public float speed;
        [Tooltip("amplitude in unity units in the back and forward motion")]
        public float amplitude;
        public float degreesYRotationPositive;
        public float degreesYRotationNegative;
        public float rayDistanceToGround = 10f;
        public LayerMask layerGround;

        private Vector3 _movement;
        private float z = 0;
        private float z0 = -1; //last position z
        private float degrees = 0;
        private IDisposable _movementDisposable;
        private Ray _ray = new Ray();
        private bool _isSpawning = false;
        private Quaternion _lastRotation = new Quaternion();
        private Subject<bool> _isHittingGround = new Subject<bool>();

        public override void MoveToSpawn()
        {
            _movementDisposable = Observable.EveryUpdate()
                    .Subscribe(frame =>
                    {
                        _isHittingGround.ResetSubject();
                        _isSpawning = true;
                        Move();
                        Rotate();
                        DetectGround();
                    }, Dispose);
        }

        private void Move()
        {
            z += speed * Time.deltaTime;
            _movement.z = Mathf.Sin(z);
            transform.localPosition = _movement * amplitude;
        }

        private void Rotate()
        {
            degrees = FunctionIsIncreasing() ?
                degreesYRotationPositive :
                degreesYRotationNegative;

            var rotateTo = new Vector3(transform.rotation.x, degrees, transform.rotation.z);
            transform.eulerAngles = Vector3.Lerp(transform.rotation.eulerAngles, rotateTo, 1);

            if (RotationChange())
            {
                _isSpawning = false;
                Dispose();
            }
        }

        private void DetectGround()
        {
            _ray.origin = transform.position;
            _ray.direction = Vector3.down;
            _isHittingGround.OnNext(Physics.Raycast(_ray, rayDistanceToGround, layerGround) && IsSpawning);
        }

        private bool FunctionIsIncreasing()
        {
            var isIncreasing = _movement.z > z0;
            z0 = _movement.z;
            return isIncreasing;
        }

        private bool RotationChange()
        {
            bool hasChangeRotation = !(_lastRotation == transform.rotation);
            _lastRotation = transform.rotation;
            return hasChangeRotation;
        }

        public override bool IsAvailable => !IsSpawning;
        public override IObservable<bool> IsHittingGround => _isHittingGround;
        private bool IsSpawning => _isSpawning;
        public override void CompleteSpawn() => _isHittingGround.OnCompleted();

        private void Dispose()
        {
            _movementDisposable.Dispose();
        }
    }
}
