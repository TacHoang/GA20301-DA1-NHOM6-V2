using UnityEngine;
using System.Collections;

public class SoundC5 : MonoBehaviour
{
    private AudioSource audio;

    void Start()
    {
        audio = GetComponent<AudioSource>();

        if (audio != null && audio.clip != null)
        {
            audio.Play();
            StartCoroutine(ShortenSound());
        }
        else
        {
            Destroy(gameObject, 2.5f);
        }
    }

    IEnumerator ShortenSound()
    {
        yield return new WaitForSeconds(2.5f); // chỉ phát 1 giây
        audio.Stop();
        Destroy(gameObject); // hoặc huỷ sau khi dừng
    }
}