using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class CharThrowArrow : MonoBehaviour
    {
        public GameObject Arrow;
        public Vector2 currentDir;

        public void SetDirection(Vector2 vec)
        {
            if(vec == Vector2.zero)
            {
                Arrow.SetActive(false);
                return;
            } else
            {
                Arrow.SetActive(true);
            }

            float angle = Vector2.Angle(Vector2.up, vec);
            if (vec.x > 0) angle *= -1;

            Vector3 vec3 = (transform.rotation * new Vector3(0, 0, 1)) + new Vector3(0, 0, angle);
            transform.rotation = Quaternion.Euler(vec3);

            currentDir = vec;
        }
    }
}