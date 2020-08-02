using HexCS.Mathematics;
using HexUN.Animation;
using PathCreation;
using System.Collections.Generic;
using UnityEngine;

namespace PT
{
    /// <summary>
    /// Rewinds the ball
    /// </summary>
    public class PTPathInterpolator : MonoBehaviour
    {
        private int _interpolationId;

        [SerializeField]
        [Tooltip("The physics rigidbody being rewound")]
        private Rigidbody2D _rb;

        private VertexPath _path = null;
        private IInterpolationToken<float[]> _interp;
        private GameObject _pathContainer;

        private Vector3 _lastPos;
        private Vector3 _currentPos;

        private bool _isInterpolating = false;

        private void Start()
        {
            _interpolationId = InterpolationManager.Instance.GetUniqueId();
            _pathContainer = new GameObject("Path Container");
        }

        /// <summary>
        /// Perform a rewind based on the path given
        /// </summary>
        /// <param name="path"></param>
        public void PerformInterpolation(IEnumerable<Vector3> path, float duration, EEasingFunction ease)
        {
            _rb.simulated = false;

            _path = new VertexPath(new BezierPath(path), _pathContainer.transform);

            _interp = InterpolationManager.Instance.StartInterpolation(
                _interpolationId,
                duration,
                new SInterpolation()
                {
                    Start = _path.length,
                    End = 0,
                    Ease = ease
                });

            _lastPos = Vector3.negativeInfinity;
            _currentPos = _rb.transform.position;

            _interp.OnInterpolationSubscriber.Subscribe(
                v =>
                {
                    if (_path != null)
                    {
                        _lastPos = _currentPos;

                        Vector3 move = _path.GetPointAtDistance(v[0]);
                        _rb.transform.position = move;
                        _currentPos = move;
                    }
                }
            );

            _interp.OnInterpolationEndSubscriber.Subscribe(() => StopInterpolating());

            _isInterpolating = true;
        }

        /// <summary>
        /// Stop interpolating
        /// </summary>
        public void StopInterpolating()
        {
            if (!_isInterpolating) return;

            _interp?.Cancel();

            _rb.simulated = true;

            if (_lastPos != Vector3.negativeInfinity)
            {
                _rb.velocity = (_currentPos - _lastPos) / Time.deltaTime;
            }

            _isInterpolating = false;
        }
    }
}