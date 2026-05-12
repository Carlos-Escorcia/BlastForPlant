using UnityEngine;
using TMPro;

public class MostrarPuntuacionFinal : MonoBehaviour
{
    [Header("Referencias")]
    public TextMeshProUGUI textoFinal;
    public string mensaje = "Total de bajas: ";

    [Header("Efecto de Latido (Solo aquí)")]
    public float velocidadPulso = 5f;
    public float escalaMinima = 0.8f;
    public float escalaMaxima = 1.2f;

    void Start()
    {
        if (textoFinal != null)
        {
            textoFinal.text = mensaje + ControlContador.enemigosMuertosGlobal; //Muestra el total de ControlCOntador
        }
    }

    void Update()
    {
        EfectoLatido(); //Solo en este escena
    }

    private void EfectoLatido()
    {
        if (textoFinal != null)
        {
            float escala = Mathf.Lerp(escalaMinima, escalaMaxima, (Mathf.Sin(Time.time * velocidadPulso) + 1.0f) / 2.0f);
            textoFinal.transform.localScale = new Vector3(escala, escala, 1f);
        }
    }
}