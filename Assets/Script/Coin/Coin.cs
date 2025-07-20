using UnityEngine;

public class Coin : MonoBehaviour
{
    public int value = 100;
    private bool collected = false;

    public GameObject floatingTextPrefab; // gán Prefab FloatingText trong Inspector

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player") && !collected)
        {
            collected = true;

            // Hiện chữ " +value " trên Canvascoin
            Transform canvasTransform = GameObject.Find("Canvascoin").transform;
            GameObject go = Instantiate(floatingTextPrefab, transform.position, Quaternion.identity, canvasTransform);

            FloatingText ft = go.GetComponent<FloatingText>();
            if (ft != null)
                ft.Setup("+" + value.ToString(), Color.yellow);

            // Cộng tiền
            CoinManager.Instance.AddCoins(value);

            Destroy(gameObject);
        }
    }
}

