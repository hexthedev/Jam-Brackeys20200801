using HexUN.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelChangeTrigger : MonoBehaviour
{
    public Vector2ReliableEvent _onMoveLevel;
    public Vector2 _move;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        _onMoveLevel.Invoke(_move);
        gameObject.SetActive(false);
    }
}
