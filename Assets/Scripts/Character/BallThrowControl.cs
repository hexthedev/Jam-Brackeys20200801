using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallThrowControl : MonoBehaviour
{
    public CharThrowArrow _arrow;
    public BallThrower _throw;

    public void ThrowBall()
    {
        _throw.Launch(_arrow.currentDir);
    }
}
