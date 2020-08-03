using UnityEngine;

public class BallThrower : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D _ballrb;

    [SerializeField]
    private float _initialForce;

    [SerializeField]
    private Vector2 _defaultLaunchDir = Vector2.up;

    public void Launch(Vector2 vec)
    {
        _ballrb.transform.position = transform.position;
        _ballrb.gameObject.SetActive(true);
        _ballrb.AddForce(vec.normalized * _initialForce, ForceMode2D.Impulse);
    }

    public void Launch()
    {
        _ballrb.transform.position = transform.position;
        _ballrb.gameObject.SetActive(true);
        _ballrb.AddForce(_defaultLaunchDir * _initialForce, ForceMode2D.Impulse);
    }

}
