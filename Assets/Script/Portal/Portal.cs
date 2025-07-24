using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [Header("Map1Lv2")]
    public string sceneToLoad; // Gõ tên Scene trong Inspector

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Kiểm tra nếu là Player
        {
            SceneManager.LoadScene(sceneToLoad); // Chuyển màn
        }
    }
}