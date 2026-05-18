using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// Estas etiquetas obligan a Unity a añadir estos componentes automáticamente 
// al objeto si no los tiene. Así evitamos errores de "NullReferenceException".
[RequireComponent(typeof(AudioSource))]
public class BotonCambioEscena : MonoBehaviour
{
    [Header("Configuración de Escena")]
    [Tooltip("Escribe exactamente el nombre de la escena a la que quieres ir (ej: MenuPrincipal)")]
    public string nombreEscenaDestino;

    [Header("Configuración de Audio")]
    public AudioClip sonidoClic;

    // Referencia interna al componente AudioSource de nuestro objeto
    private AudioSource miAudioSource;

    private void Awake()
    {
        // En el Awake (antes de que empiece el juego), obtenemos la referencia al AudioSource.
        miAudioSource = GetComponent<AudioSource>();

        // Nos aseguramos de que el AudioSource no reproduzca nada al aparecer
        miAudioSource.playOnAwake = false;
    }
    public void PresionarBoton()
    {
        // En lugar de cambiar la escena directamente, iniciamos la Corrutina
        StartCoroutine(RutinaCambioEscena());
    }
    private IEnumerator RutinaCambioEscena()
    {
        // 1. Asignamos el sonido y lo reproducimos
        if (sonidoClic != null)
        {
            miAudioSource.clip = sonidoClic;
            miAudioSource.Play();

            // 2. Le decimos a Unity que ESPERE el tiempo exacto que dura el sonido
            // yield return significa "pausa esta función aquí y devuelve el control a Unity hasta que pase esto"
            yield return new WaitForSeconds(sonidoClic.length);
        }
        else
        {
            yield return null;
        }

        //Cuando acaba el tiempo de espera, cambia la escena
        SceneManager.LoadScene(nombreEscenaDestino);
    }
}
