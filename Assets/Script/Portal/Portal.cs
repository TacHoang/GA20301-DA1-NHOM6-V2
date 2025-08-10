using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public string targetScene = "Map1Lv2"; // Scene đích muốn đến
    private bool isLoading = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isLoading)
        {
            isLoading = true;

            // Ghi nhớ scene đích để LoadingScene biết
            GameManager.sceneToLoad = targetScene;

            // Chuyển ngay sang scene Loading
            SceneManager.LoadScene("LoadingScene");
        }
    }
}
