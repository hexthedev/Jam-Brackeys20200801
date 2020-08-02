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

        [Header("Input")]
        [SerializeField]
        private InputActionAsset _actions;

        [SerializeField]
        private string _rewindActionName = "ActionWest";

        void Awake()
        {
            // rewind action
            InputAction rewind = _actions.FindAction(_rewindActionName);

            rewind.started += Rewind_started;
            rewind.canceled += Rewind_canceled;

            _actions.FindAction("Move").started += PTGlobalInput_started; ;
        }

        private void PTGlobalInput_started(CallbackContext obj)
        {
            _onMoveEvent.Invoke(obj.ReadValue<Vector2>());
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
    }
}