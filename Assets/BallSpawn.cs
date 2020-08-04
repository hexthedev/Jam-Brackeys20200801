using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class BallSpawn : MonoBehaviour
    {
        public Vector3 InitialVelocity;
        public float force;

        private void OnDrawGizmos()
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(Vector3.zero, 1f);
        }
    }
}