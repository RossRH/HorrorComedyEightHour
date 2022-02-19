
using UnityEngine;

[ExecuteAlways]
public class FollowPlayer : MonoBehaviour
{
    public Vector3 Offset;
    public Transform Follow;
    
    void LateUpdate()
    {
        transform.position = Follow.position + Offset;

            // transform.forward = Vector3.down;
        transform.rotation = Quaternion.Euler(90, Follow.eulerAngles.y, 0);
    }
}
