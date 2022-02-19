
using System;
using UnityEngine;

//[ExecuteAlways]
public class FollowPlayer : MonoBehaviour
{
    public Vector3 Offset;
    public Transform Follow;

    public bool rotateToPayer = false;

    private void Start()
    {
        transform.parent = null;
    }

    void LateUpdate()
    {
        transform.position = Follow.position + Offset;

        if (rotateToPayer)
        {
            transform.rotation = Quaternion.Euler(0, 0, Follow.eulerAngles.z);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
