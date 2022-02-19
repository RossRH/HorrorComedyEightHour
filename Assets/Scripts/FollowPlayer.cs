
using System;
using UnityEngine;

//[ExecuteAlways]
public class FollowPlayer : MonoBehaviour
{
    public Vector3 Offset;
    public Transform Follow;

    private void Start()
    {
        transform.parent = null;
    }

    void LateUpdate()
    {
        transform.position = Follow.position + Offset;

            // transform.forward = Vector3.down;
        transform.rotation = Quaternion.Euler(0, 0, Follow.eulerAngles.z);
    }
}
