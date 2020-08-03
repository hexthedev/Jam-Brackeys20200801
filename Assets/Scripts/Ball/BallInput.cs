using HexCS.Mathematics;
using HexUN.Events;
using PT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// Takes input from SO events and pushes it to various ball control systems.
    /// Tells with cooldowns and stuff like that
    /// </summary>
    public class BallInput : MonoBehaviour
    {
        [Header("Input Events")]
        [SerializeField]
        private BooleanSoEvent _onRewind;

        [Header("Systems")]
        [SerializeField]
        private PathInterpolator _rewind;

        [SerializeField]
        private PTPathTracker _pathTracker;

        [Header("Options (Rewind)")]
        [SerializeField]
        [Range(0.5f, 3f)]
        private float _rewindDuration = 1f;

        [SerializeField]
        private EEasingFunction _rewindEase;

        [SerializeField]
        private float _cooldown = 2f;

        //[Header("Options (Timewarp)")]
        //[SerializeField]
        //[Range(0, 1f)]
        //private float _timeScaleRewind = 0.75f;

        //[SerializeField]
        //[Range(0.1f, 3f)]
        //private float _timeChangeDuration = 0.25f;

        //[SerializeField]
        //private EEasingFunction _timeEase;

        private bool _isRewindHot = true;

        private void Start()
        {
            _onRewind.Event.Subscribe(HandleRewind);
            _rewind.OnEndDistance._event.Subscribe(HandleRewindEnd);
        }

        private void HandleRewind(bool rewind)
        {
            if (rewind)
            {
                if (!_isRewindHot) return;
                //TimeManager.Instance.ChangeTimeScale(_timeScaleRewind, _timeChangeDuration, _timeEase);
                _pathTracker.StopTrack();
                _rewind.PerformInterpolation(_pathTracker.GetLastCrumbs(60), _rewindDuration, _rewindEase);
                _isRewindHot = false;
            }
            else
            {
                //TimeManager.Instance.ChangeTimeScale(1, _timeChangeDuration, _timeEase);
                _rewind.StopInterpolating();
                _pathTracker.StartTrack();
                StartCoroutine(RewindCooldown());
            }
        }


        IEnumerator RewindCooldown()
        {
            yield return new WaitForSeconds(_cooldown);
            _isRewindHot = true;
        }


        private void HandleRewindEnd(float steps)
        {
            _pathTracker.ClearCrumbs((int)steps);
        }
    }
}