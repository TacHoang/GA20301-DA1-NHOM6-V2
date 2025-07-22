using UnityEngine;

public class Trident : MonoBehaviour
{
    [SerializeField] private Transform firePos;         // Nơi bắn ra đạn (empty object con của súng)
    [SerializeField] private GameObject TridentFire;    // Prefab đạn (đinh ba bay)
    [SerializeField] private float shotDelay = 2f;      // Delay giữa các lần bắn
    private float nextShot;

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
            Instantiate(TridentFire, firePos.position, firePos.rotation); // Bắn đinh ba bay ra
        }
    }
}

