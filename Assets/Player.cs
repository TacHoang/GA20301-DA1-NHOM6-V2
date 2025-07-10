using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Lấy input từ bàn phím (WASD hoặc mũi tên)
        moveInput.x = Input.GetAxisRaw("Horizontal"); // Trái/Phải
        moveInput.y = Input.GetAxisRaw("Vertical");   // Lên/Xuống
        moveInput.Normalize(); // Đảm bảo tốc độ di chuyển đều khi đi chéo
    }

    void FixedUpdate()
    {
        // Di chuyển bằng Rigidbody2D
        rb.linearVelocity = moveInput * moveSpeed;
    }
}
