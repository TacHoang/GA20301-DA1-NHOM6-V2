using UnityEngine;
using UnityEngine.UI;

public class FixMiniMap : MonoBehaviour
{
    public RawImage miniMapImage;
    public RenderTexture miniMapTexture;

    void Start()
    {
        if (miniMapImage != null && miniMapTexture != null)
        {
            miniMapImage.texture = miniMapTexture;
        }
    }
}
