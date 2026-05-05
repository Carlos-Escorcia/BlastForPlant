using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class EfectoParalax2 : MonoBehaviour
{
    private float longitudSprite;
    private float posicionInicialX;
    private float diferenciaAlturaInicial;

    [Header("Configuración")]
    public Transform camaraTransform;
    [Tooltip("0 = Se mueve con la cámara. 1 = Estático. 0.5 = Mitad de velocidad.")]
    public float efectoParallax;

    [Header("Ajustes de Altura")]
    public bool seguirEnVertical = true;

    [Header("Corrección visual")]
    public float correccionSolapamiento = 0.05f;

    IEnumerator Start()
    {
        posicionInicialX = transform.position.x;
        yield return new WaitForEndOfFrame();

        if (camaraTransform != null)
        {
            diferenciaAlturaInicial = transform.position.y - camaraTransform.position.y;
        }

        float tamañoReal = GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        longitudSprite = tamañoReal - correccionSolapamiento;
    }

    void LateUpdate()
    {
        if (camaraTransform == null) return;

        float distancia = (camaraTransform.position.x * efectoParallax);
        float nuevaPosicionY = transform.position.y;

        if (seguirEnVertical)
        {
            nuevaPosicionY = camaraTransform.position.y + diferenciaAlturaInicial;
        }

        transform.position = new Vector3(posicionInicialX + distancia, nuevaPosicionY, transform.position.z);
    }
}