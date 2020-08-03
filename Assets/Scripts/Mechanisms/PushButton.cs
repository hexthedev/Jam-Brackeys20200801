using HexUN.Events;
using UnityEngine;

public class PushButton : MonoBehaviour
{
    public BooleanReliableEvent _onPush;

    public SpriteRenderer r;

    public Color on;
    public Color off;

    public void Start()
    {
        r.color = off;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        r.color = on;
        _onPush.Invoke(true);
    }
}
