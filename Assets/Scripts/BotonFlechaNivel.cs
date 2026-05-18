using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class BotonFlechaNivel : MonoBehaviour
{
    public string nombreEscenaDestino;
    public AudioClip sonidoClic;
    private AudioSource miAudioSource;

    private void Awake()
    {
        miAudioSource = GetComponent<AudioSource>();
        //No suena al empezar la escena
        miAudioSource.playOnAwake = false;
    }

    public void PresionarBoton()
    {
        StartCoroutine(RutinaEsperaYCambio());
    }

    private IEnumerator RutinaEsperaYCambio()
    {
        if (sonidoClic != null)
        {
            miAudioSource.clip = sonidoClic;
            miAudioSource.Play();
            //Esperala duración del sonido antes de cambiar
            yield return new WaitForSeconds(sonidoClic.length);
        }
        else
        {
            //Si no hay sonido, una espera un frame para evitar errores
            yield return null;
        }

        SceneManager.LoadScene(nombreEscenaDestino);//Cambia escena
    }
}