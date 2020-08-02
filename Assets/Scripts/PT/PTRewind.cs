using Game;
using HexCS.Mathematics;
using HexUN.Animation;
using PathCreation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PTRewind : MonoBehaviour
{
    private int _interpolationId;

    [SerializeField]
    [Tooltip("The physics RB")]
    private Rigidbody2D _rb;

    [SerializeField]
    private PTPathTracker _tracker;

    [SerializeField]
    private EEasingFunction _func;

    [SerializeField]
    private float _rewindTime;

    private VertexPath _path = null;
    private IInterpolationToken<float[]> _interp;
    private GameObject _pathContainer;

    private Vector3 _lastPos;
    private Vector3 _currentPos;


    private void Start()
    {
        _interpolationId = InterpolationManager.Instance.GetUniqueId();
        _pathContainer = new GameObject("Path Container");
    }

    /// <summary>
    /// Go into or out of of rewind mode
    /// </summary>
    /// <param name="rewind"></param>
    public void HandleRewind(bool rewind)
    {
        if (rewind) PerformRewind();
        else StopRewind();
    }

    private void PerformRewind()
    {
        _rb.simulated = false;

        _path = new VertexPath(new BezierPath(_tracker.CurrentPath), _pathContainer.transform);

        _interp = InterpolationManager.Instance.StartInterpolation(
            _interpolationId,
            _rewindTime,
            new SInterpolation()
            {
                Start = _path.length,
                End = 0,
                Ease = _func
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

        _interp.OnInterpolationEndSubscriber.Subscribe(() => StopRewind());
    }

    private void StopRewind()
    {
        _interp?.Cancel();

        _rb.simulated = true;

        if(_lastPos != Vector3.negativeInfinity)
        {
            _rb.velocity = (_currentPos - _lastPos) / Time.deltaTime;
        }
    }
}
