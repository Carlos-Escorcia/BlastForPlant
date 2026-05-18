using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BotonExit : MonoBehaviour
{
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

        float tiempoDeAudio = 0f;
        float tiempoDeAnimacion = 0f;

        if (sonidoSalida != null && fuenteAudio != null) //Reproduce sonido y guarda duración
        {
            fuenteAudio.PlayOneShot(sonidoSalida);
            tiempoDeAudio = sonidoSalida.length; //Calcula duración sonido
        }

        if (miAnimador != null) //Reproduce la animación
        {
            miAnimador.Play("Pulsado BotonExit");
            yield return null;

            //Obtiene la información del estado actual
            AnimatorStateInfo infoEstado = miAnimador.GetCurrentAnimatorStateInfo(0);

            //Cálculo de la duración de la velocidad el animator
            tiempoDeAnimacion = infoEstado.length / infoEstado.speedMultiplier;
        }
        float tiempoEsperaTotal = Mathf.Max(tiempoDeAudio, tiempoDeAnimacion);

        yield return new WaitForSeconds(tiempoEsperaTotal); //Espera el tiempo
        //Debug.Log("Cerrando el ejecutable automáticamente...");
        Application.Quit();
    }
}