using UnityEngine;

public class Trident : MonoBehaviour
{
    private float rotateOffset = 180f;
    [SerializeField] private Transform firePos;
    [SerializeField] private GameObject TridentFire;
    [SerializeField] private float shotDelay = 2f;
    private float nextShot;
    [SerializeField] private int maxAmo = 1;
    public int currentAmo;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentAmo = maxAmo;
    }

    // Update is called once per frame
    void Update()
    {
        RotateGun();
        Shoot();

    }
    void RotateGun()
    {
        if (Input.mousePosition.x < 0 || Input.mousePosition.x > Screen.width || Input.mousePosition.y < 0 || Input.mousePosition.y > Screen.height)
        {
            return;
        }
        Vector3 displacement = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float angle = Mathf.Atan2(displacement.y, displacement.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + rotateOffset);

    }
    void Shoot()
    {
        if (Input.GetMouseButtonDown(0) && Time.time > nextShot)
        {
            nextShot = Time.time + shotDelay;
            Instantiate(TridentFire, firePos.position, firePos.rotation);
            // currentAmo--;  // Xoá dòng này để không giảm đạn
        }
    }
}
