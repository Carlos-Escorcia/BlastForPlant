using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BotonExit : MonoBehaviour
{
    [Header("Configuración de Salida")]
    [Tooltip("Tiempo de espera en segundos antes de cerrar")]
    public float tiempoEspera = 0.4f;

    [Header("Referencias")]
    public Animator miAnimador;
    private Button miBoton;

    [Header("Sonido (Opcional)")]
    public AudioClip sonidoSalida;
    private AudioSource fuenteAudio;

    void Start()
    {
        fuenteAudio = GetComponent<AudioSource>();
        miBoton = GetComponent<Button>();

        if (miAnimador == null)
        {
            miAnimador = GetComponent<Animator>();
        }
    }

    public void EjecutarSalida()
    {
        StartCoroutine(RutinaCerrarJuego());
    }

    private IEnumerator RutinaCerrarJuego()
    {
        if (miBoton != null)
        {
            miBoton.enabled = false;
        }

        if (miAnimador != null) //Reproduce animación
        {
            miAnimador.Play("Pulsado BotonExit");
        }

        if (sonidoSalida != null && fuenteAudio != null) //Reproduce animación de salida
        {
            fuenteAudio.PlayOneShot(sonidoSalida);
        }

        yield return new WaitForSeconds(tiempoEspera); //Espera el tiempo establecido

        //Debug.Log("Cerrando el ejecutable...");
        Application.Quit();
    }
}