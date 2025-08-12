using UnityEngine;
using System.Collections;

public class Trident : MonoBehaviour
{
    [SerializeField] private Transform firePos;
    [SerializeField] private GameObject TridentFire;
    [SerializeField] private float shotDelay = 2f;
    [SerializeField] private float fireDelay = 0.3f;
    [SerializeField] private AudioClip shootSound;

    private float nextShot;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        RotateGun();
        Shoot();
    }

    void RotateGun()
    {
        Vector3 displacement = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float angle = Mathf.Atan2(displacement.y, displacement.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + 180f);
    }

    void Shoot()
    {
        if (Input.GetMouseButtonDown(0) && Time.time > nextShot)
        {
            nextShot = Time.time + shotDelay;
            StartCoroutine(FireAfterDelay());
        }
    }

    IEnumerator FireAfterDelay()
    {
        yield return new WaitForSeconds(fireDelay);

        Instantiate(TridentFire, firePos.position, firePos.rotation);

        if (audioSource != null && shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);
        }
    }
}