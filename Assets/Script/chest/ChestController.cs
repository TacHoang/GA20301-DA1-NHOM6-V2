using UnityEngine;
using TMPro;

public class ChestController : MonoBehaviour
{
    [Header("Gắn Prefab Item rơi ra")]
    public GameObject itemPrefab;

    [Header("Text UI hiện gợi ý (TextMeshProUGUI)")]
    public TextMeshProUGUI hintText;

    private bool isPlayerNearby = false;
    private bool isOpened = false;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

        // Ẩn text khi bắt đầu game
        if (hintText != null)
            hintText.gameObject.SetActive(false);
    }

    void Update()
    {
        // Khi người chơi gần và chưa mở rương, nhấn E để mở
        if (isPlayerNearby && !isOpened && Input.GetKeyDown(KeyCode.E))
        {
            OpenChest();
        }
    }

    void OpenChest()
    {
        isOpened = true;

        // Gửi trigger "Open" cho Animator
        if (animator != null)
            animator.SetTrigger("Open");

        // Ẩn dòng chữ gợi ý
        if (hintText != null)
            hintText.gameObject.SetActive(false);

        // Tạo item rơi ra (nếu có)
        if (itemPrefab != null)
        {
            Instantiate(itemPrefab, transform.position + Vector3.up * 1.5f, Quaternion.identity);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Nếu player đến gần rương
        if (collision.CompareTag("Player"))
        {
            isPlayerNearby = true;

            // Hiện chữ gợi ý nếu rương chưa mở
            if (!isOpened && hintText != null)
                hintText.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Khi player rời xa rương
        if (collision.CompareTag("Player"))
        {
            isPlayerNearby = false;

            if (hintText != null)
                hintText.gameObject.SetActive(false);
        }
    }
}