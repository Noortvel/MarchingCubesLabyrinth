using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
