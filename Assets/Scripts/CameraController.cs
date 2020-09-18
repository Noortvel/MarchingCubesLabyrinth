using UnityEngine;

namespace MarchingCubes
{
    public class CameraController : MonoBehaviour
    {
        private Transform target;
        private Vector3 offset;
        private void Awake()
        {
            offset = transform.localPosition;
            target = transform.parent;
            transform.parent = null;
        }
        void Update()
        {
            transform.position = target.position + offset;
        }
    }
}