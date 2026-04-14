using UnityEngine;
using UnityEngine.UI;

public class ControlMenuPrincipal : MonoBehaviour
{
    public RawImage rawImage;
    public float velocidadX = -0.1f;
    public float velocidadY = 0.1f;

    private void Update()
    {
        if (rawImage == null) return;

        float offset_x = (Time.time * velocidadX) % 1f;
        float offset_y = (Time.time * velocidadY) % 1f;

        rawImage.uvRect = new Rect(offset_x, offset_y, 1f, 1f);
    }
}
