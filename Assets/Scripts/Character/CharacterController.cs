using HexUN.Events;
using HexUN.Physics2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class CharacterController : MonoBehaviour
    {
        [Header("Events")]
        [SerializeField]
        private Vector2SoEvent _onInput;

        [SerializeField]
        private VoidSoEvent _onJump;

        [Header("Deps")]
        [SerializeField]
        private Rigidbody2D _rb;

        [SerializeField]
        private Raycast2DSensor _groundSensor;

        [Header("Options")]
        [SerializeField]
        private float acceleration = 1;

        [SerializeField]
        private float maxVelocity = 2;

        [SerializeField]
        private float _jumpForce = 200;


        private bool _moveThisFrame = false;
        private Vector2 _moveDirection;

        private float _maxVelocitySqr;

        public void Awake()
        {
            _maxVelocitySqr = maxVelocity * maxVelocity;

            _onInput.Event.Subscribe(Move);
            _onJump.Event.Subscribe(Jump);
        }

        public void Move(Vector2 direction)
        {
            _moveDirection = direction.normalized;
            _moveThisFrame = true;
        }

        public void Jump()
        {
            RaycastHit2D[] col = _groundSensor.Sense();

            if (col.Length != 0)
            {
                Debug.Log(col[0].collider);
                _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Force);
            }
        }

        public void Update()
        {
            if (_moveThisFrame)
            {
                _rb.velocity += Time.deltaTime * acceleration * _moveDirection;

                if(_rb.velocity.sqrMagnitude > _maxVelocitySqr)
                {
                    _rb.velocity = _rb.velocity.normalized * maxVelocity;
                }

                _moveThisFrame = false;
            }
        }
    }
}