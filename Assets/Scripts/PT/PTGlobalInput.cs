using Game;
using HexUN.Events;
using HexUN.MonoB;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace PT
{
    /// <summary>
    /// Protoglobal input
    /// </summary>
    public class PTGlobalInput : AMonoSingletonPersistent<PTGlobalInput>
    {
        [Header("Emissions")]
        [SerializeField]
        private Vector2ReliableEvent _onMoveEvent;

        [SerializeField]
        private BooleanReliableEvent _onRewindEvent;

        [SerializeField]
        private VoidReliableEvent _onJump;

        [Header("Input")]
        [SerializeField]
        private InputActionAsset _actions;

        [SerializeField]
        private string _rewindActionName = "ActionWest";

        private InputAction _moveAction;
        private bool _isMoving = false;

        void Awake()
        {
            // rewind action
            InputAction rewind = _actions.FindAction(_rewindActionName);

            rewind.started += Rewind_started;
            rewind.canceled += Rewind_canceled;

            _moveAction = _actions.FindAction("Move");
            _moveAction.started += PTGlobalInput_started;
            _moveAction.canceled += cc => _isMoving = false;

            _actions.FindAction("ActionSouth").started += cc => _onJump.Invoke();
        }

        private void PTGlobalInput_started(CallbackContext obj)
        {
            _isMoving = true;
        }

        // Rewind callbacks
        private void Rewind_started(CallbackContext obj)
        {
            _onRewindEvent.Invoke(true);

        }
        private void Rewind_canceled(CallbackContext obj)
        {
            _onRewindEvent.Invoke(false);
        }

        private void Update()
        {
            if (_isMoving)
            {
                _onMoveEvent.Invoke(_moveAction.ReadValue<Vector2>());
            }
        }
    }
}