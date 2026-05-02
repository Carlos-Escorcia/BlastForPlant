using UnityEngine;
using System.Collections;

public class CreditosFinales : MonoBehaviour
{
    [Header("Ajustes de los Créditos")]
    public float velocidad = 70f;
    [Tooltip("2 min y 27 seg = 147 segundos")]
    public float tiempoParaBoton = 147f;

    [Header("El Botón que aparecerá")]
    public CanvasGroup grupoBoton;

    private RectTransform rectTransform;
    private float tiempoTranscurrido = 0f;
    private bool botonYaMostrado = false;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        // Preparamos el botón como "invisible" y "no clicable"
        if (grupoBoton != null)
        {
            grupoBoton.alpha = 0f;
            grupoBoton.interactable = false;
            grupoBoton.blocksRaycasts = false;
        }
    }

    void Update()
    {
        // EL TEXTO SIEMPRE SUBE (He quitado el "if subiendo")
        rectTransform.anchoredPosition += Vector2.up * velocidad * Time.deltaTime;

        // El reloj sigue contando
        tiempoTranscurrido += Time.deltaTime;

        // Lógica para mostrar el botón (por tiempo o por tecla)
        if (!botonYaMostrado)
        {
            if (tiempoTranscurrido >= tiempoParaBoton || Input.GetKeyDown(KeyCode.Space))
            {
                MostrarSoloElBoton();
            }
        }
    }

    void MostrarSoloElBoton()
    {
        botonYaMostrado = true; // Así no se repite la corrutina

        if (grupoBoton != null)
        {
            StartCoroutine(FadeInBoton());
        }
    }

    private IEnumerator FadeInBoton()
    {
        float tiempo = 0f;
        float duracion = 0.5f;

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            grupoBoton.alpha = Mathf.Lerp(0f, 1f, tiempo / duracion);
            yield return null;
        }

        grupoBoton.alpha = 1f;
        grupoBoton.interactable = true;
        grupoBoton.blocksRaycasts = true;
    }
}