using UnityEngine;

public class MiniMapFollow : MonoBehaviour
{
    public Transform player;

    void LateUpdate()
    {
        if (player != null)
        {
            Vector3 newPos = player.position;
            newPos.z = transform.position.z; // Giữ Z = -10
            transform.position = newPos;
        }
    }
}

