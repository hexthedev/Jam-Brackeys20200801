using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PTPathTracker : MonoBehaviour
{
    public int PathSaveFrames = 60;
    public GameObject PathPointPrefab;
    public Transform target;

    private Queue<PathPoint> _currentPath = new Queue<PathPoint>();
    private Transform _path;

    private void Start()
    {
        GameObject g = new GameObject();
        _path = g.transform;
    }

    private void FixedUpdate()
    {
        Vector3 record = target.position;

        GameObject go = PathPointPrefab == null ? GameObject.CreatePrimitive(PrimitiveType.Sphere) : Instantiate(PathPointPrefab);
        go.transform.SetParent(_path.transform, false);
        go.transform.position = record;

        _currentPath.Enqueue(new PathPoint()
        {
            position = target.position,
            viz = go
        });

        while(_currentPath.Count > PathSaveFrames)
        {
            _currentPath.Dequeue().DestroyPoint();
        }
    }

    public class PathPoint
    {
        public Vector3 position;
        public GameObject viz;
        public void DestroyPoint() => Destroy(viz);
    }
}
