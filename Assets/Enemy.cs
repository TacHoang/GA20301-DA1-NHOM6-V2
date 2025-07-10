using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2f;
    private Transform player;

    void Update()
    {
        // Nếu player chưa gán, thì tìm liên tục
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
            else
            {
                return; // không có player thì không làm gì
            }
        }

        // Di chuyển về phía player
        Vector2 direction = ((Vector2)player.position - (Vector2)transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
    }
}
