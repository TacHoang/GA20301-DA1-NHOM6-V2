using UnityEngine;
using UnityEngine.UI;

public class thanhmau : MonoBehaviour
{
    public Image _thanhmau;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void capnhatthanhmau(float mauhientai, float mautoida)
    {
        _thanhmau.fillAmount = mauhientai / mautoida;
    }
}
