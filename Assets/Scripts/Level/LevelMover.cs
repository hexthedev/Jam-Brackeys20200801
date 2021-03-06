﻿using HexCS.Mathematics;
using HexUN.Animation;
using HexUN.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class LevelMover : MonoBehaviour
    {
        private int _interpId;

        public Vector2SoEvent _moveLevelEvent;

        public Rigidbody2D _player;

        public float slowScale = 0.25f;
        public float transitionTime = 1f;

        public void Awake()
        {
            _moveLevelEvent.Event.Subscribe(Move);
            _interpId = InterpolationManager.Instance.GetUniqueId();
        }

        public void Move(Vector2 move)
        {
            StartCoroutine(MoveRoutine(move));            
        }

        IEnumerator MoveRoutine(Vector2 move)
        {
            TimeManager.Instance.ChangeTimeScale(slowScale);

            float recGrav = _player.gravityScale;
            _player.gravityScale = 0;

            IInterpolationToken<Vector3> interp = UTInterpolation.InterpolateVector3(
                _interpId,
                transform.position,
                transform.position + new Vector3(move.x, move.y, 0),
                transitionTime * slowScale,
                EEasingFunction.Out_Cubic
            );

            interp.OnInterpolationSubscriber.Subscribe( v => transform.position = v );

            yield return new WaitForSeconds(transitionTime * slowScale);
            TimeManager.Instance.ChangeTimeScale(1f);

            _player.gravityScale = recGrav;
        }
    }
}