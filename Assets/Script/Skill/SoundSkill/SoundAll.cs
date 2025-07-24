using UnityEngine;

public class SoundAll : MonoBehaviour
{
    void Start()
    {
        AudioSource audio = GetComponent<AudioSource>();
        if (audio != null && audio.clip != null)
        {
            Destroy(gameObject, audio.clip.length); // tự hủy sau khi âm phát xong
        }
        else
        {
            Destroy(gameObject, 1f); // fallback nếu clip null
        }
    }
}
