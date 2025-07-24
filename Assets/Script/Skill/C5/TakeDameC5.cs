using UnityEngine;
using System.Collections;



public class TakeDameC5 : MonoBehaviour
{
    public int damageamount = 0;
    public bool destroyOnHit = true;
    public GameObject dropSoundPrefab; // prefab âm rơi riêng

    void Start()
    {
        StartCoroutine(DelayDropSound());
    }

    IEnumerator DelayDropSound()
    {
        yield return new WaitForSeconds(4.5f);
        if (dropSoundPrefab != null)
        {
            Instantiate(dropSoundPrefab, transform.position, Quaternion.identity);
        }
    }








    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Gây sát thương
            EnemyHealth health = other.GetComponent<EnemyHealth>();
            if (health != null)
            {
                health.TakeDamage(damageamount);
            }



        }
    }


}