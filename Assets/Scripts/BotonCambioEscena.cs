using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class BotonCambioEscena : MonoBehaviour
{
    [Header("Configuración de Escena")]
    [Tooltip("Escribe exactamente el nombre de la escena a la que quieres ir (ej: MenuPrincipal)")]
    public string nombreEscenaDestino;

    [Header("Configuración de Audio")]
    public AudioClip sonidoClic;

    //Referencia al componente AudioSource del objeto
    private AudioSource miAudioSource;

    private void Awake() //Esto se ejecuta antes de que empiece el juego
    {
        //Se obtiene la referencia al AudioSource.
        miAudioSource = GetComponent<AudioSource>();

        //El AudioSource no se reproduce al aparecer
        miAudioSource.playOnAwake = false;
    }
    public void PresionarBoton()
    {
        //Se inicia la corrutina (en lugar de cambiar de escena)
        StartCoroutine(RutinaCambioEscena());
    }
    private IEnumerator RutinaCambioEscena()
    {
        //Asigna el sonido y lo reproduce
        if (sonidoClic != null)
        {
            miAudioSource.clip = sonidoClic;
            miAudioSource.Play();

            //Espera el tiempo EXACTO que dura el sonido
            //yield return significa "pausa esta función aquí y devuelve el control a Unity hasta que pase esto"
            yield return new WaitForSeconds(sonidoClic.length);
        }
        else
        {
            yield return null;
        }

        //Cuando acaba el tiempo de espera, cambia la escena a la que tenga el nombre de esa escena
        SceneManager.LoadScene(nombreEscenaDestino);
    }
}
