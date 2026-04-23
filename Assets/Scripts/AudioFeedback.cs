using UnityEngine;

public class AudioFeedback : MonoBehaviour
{
    [Header("Configuración de Sonido")]
    [Tooltip("Arrastra aquí el archivo de sonido del clic")]
    public AudioClip sonidoClic;

    public void ReproducirSonidoClic()
    {
        if (sonidoClic != null)
        {
            // 1. Creamos un nuevo objeto vacío en el juego llamado "EfectoUI"
            GameObject altavozTemporal = new GameObject("EfectoUI_" + sonidoClic.name);

            // [Gema de Conocimiento]: DontDestroyOnLoad es una función mágica.
            // Le dice a Unity: "Cuando cambies de escena, NO destruyas este objeto".
            DontDestroyOnLoad(altavozTemporal);

            // 2. Le añadimos un componente AudioSource a ese objeto vacío
            AudioSource fuenteAudio = altavozTemporal.AddComponent<AudioSource>();

            // 3. Lo configuramos para que sea 2D (se escuche perfecto) y le asignamos tu MP3
            fuenteAudio.spatialBlend = 0f;
            fuenteAudio.clip = sonidoClic;

            // 4. ¡Le damos al Play!
            fuenteAudio.Play();

            // 5. Programamos su autodestrucción.
            // Destroy(objeto, tiempo) eliminará el altavoz justo cuando termine de sonar el MP3.
            Destroy(altavozTemporal, sonidoClic.length);
        }
        else
        {
            Debug.LogWarning("Falta asignar el AudioClip en el Inspector.");
        }
    }
}