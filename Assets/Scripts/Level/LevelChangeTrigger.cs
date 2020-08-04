using HexUN.Events;
using UnityEngine;

namespace Game
{
    public class LevelChangeTrigger : MonoBehaviour
    {
        public StringReliableEvent _onLevelEvent;

        public Vector2ReliableEvent _onMoveLevel;
        public Vector2 _move;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            _onLevelEvent.Invoke(ELevelEvent.ExitPlay.ToString());
            gameObject.SetActive(false);
        }
    }
}