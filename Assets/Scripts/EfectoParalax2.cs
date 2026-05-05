using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class EfectoParalax2 : MonoBehaviour
{
    private float longitudSprite;
    private float posicionInicialX;
    private float posicionInicialY;

    [Header("Configuración")]
    public Transform camaraTransform;

    [Tooltip("0 = Se mueve con la cámara (cielo). 1 = Estático (primer plano). 0.5 = Mitad de velocidad.")]
    public float efectoParallax;

    [Header("Ajustes de Altura")]
    [Tooltip("Activa esto para que el fondo suba y baje con la cámara.")]
    public bool seguirEnVertical = true;
    [Tooltip("Úsalo para empujar el fondo hacia abajo y que coincida con la cámara.")]
    public float offsetVertical = 0f;

    [Header("Corrección visual")]
    [Tooltip("Cantidad de superposición para tapar las líneas entre imágenes.")]
    public float correccionSolapamiento = 0.05f;

    void Start()
    {
        posicionInicialX = transform.position.x;
        posicionInicialY = transform.position.y;
        float tamañoReal = GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        longitudSprite = tamañoReal - correccionSolapamiento;
    }

    void LateUpdate()
    {
        if (camaraTransform == null) return;

        float movimientoCamara = camaraTransform.position.x * (1 - efectoParallax);
        float distancia = (camaraTransform.position.x * efectoParallax);

        float nuevaPosicionY = posicionInicialY;
        if (seguirEnVertical)
        {
            nuevaPosicionY = camaraTransform.position.y + offsetVertical;
        }

        transform.position = new Vector3(posicionInicialX + distancia, nuevaPosicionY, transform.position.z);

        if (movimientoCamara > posicionInicialX + longitudSprite)
        {
            posicionInicialX += longitudSprite;
        }
        else if (movimientoCamara < posicionInicialX - longitudSprite)
        {
            posicionInicialX -= longitudSprite;
        }
    }
}