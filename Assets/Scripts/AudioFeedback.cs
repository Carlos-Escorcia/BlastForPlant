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
            //Creación de un objeto vacío en el juego
            GameObject altavozTemporal = new GameObject("EfectoUI_" + sonidoClic.name);

            //DontDestroyOnLoad: Al cambiar de escena, NO destruye el objeto.
            DontDestroyOnLoad(altavozTemporal);

            //Se añade un AudioSource al objeto vacío
            AudioSource fuenteAudio = altavozTemporal.AddComponent<AudioSource>();

            fuenteAudio.spatialBlend = 0f;
            fuenteAudio.clip = sonidoClic;

            //Se ejecuta el sonido con el play
            fuenteAudio.Play();

            //Se elimina cuando termina de sonar.
            Destroy(altavozTemporal, sonidoClic.length);
        }
    }
}