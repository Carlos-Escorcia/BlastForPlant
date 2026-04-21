using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class ControlPausa : MonoBehaviour
{
    [Header("Lo que aparece al pausar")]
    public GameObject todoElMenuPausa;

    [Header("El Botón de la Mochila")]
    public Image imagenMochila;
    public Sprite mochilaCerrada;
    public Sprite mochilaAbierta;

    [Header("Animación y Sonido del Menú")]
    public Animator animadorMenu;
    public AudioClip sonidoClic;
    public float tiempoDeEspera = 0.5f;
    private bool estaPausado = false;

    void Start()
    {
        Time.timeScale = 1f;
        if (todoElMenuPausa != null) todoElMenuPausa.SetActive(false);
        if (imagenMochila != null) imagenMochila.sprite = mochilaCerrada;
    }

    public void TocarMochila()
    {
        if (estaPausado == true)
        {
            estaPausado = false;
            Time.timeScale = 1f;
            todoElMenuPausa.SetActive(false);
            imagenMochila.sprite = mochilaCerrada;
        }
        else
        {
            estaPausado = true;
            Time.timeScale = 0f;
            todoElMenuPausa.SetActive(true);
            imagenMochila.sprite = mochilaAbierta;
        }
    }

    public void BotonIrAlMenu(string nombreEscenaPrincipal)
    {
        StartCoroutine(CargarMenuConRetraso(nombreEscenaPrincipal));
    }

    private IEnumerator CargarMenuConRetraso(string nombreEscena)
    {
        Time.timeScale = 1f;

        // 1. DISPARAMOS ANIMACIÓN
        if (animadorMenu != null) animadorMenu.Play("Pulsado");

        // 2. LA MAGIA DEL AUDIO (Basado en tu AudioFeedback)
        if (sonidoClic != null)
        {
            // Creamos un altavoz invisible en el juego
            GameObject altavozTemporal = new GameObject("EfectoUI_Clic");

            // ¡Lo hacemos VIP para que sobreviva al cambio de escena!
            DontDestroyOnLoad(altavozTemporal);

            // Le ponemos las propiedades para que suene perfecto en 2D
            AudioSource fuenteAudio = altavozTemporal.AddComponent<AudioSource>();
            fuenteAudio.spatialBlend = 0f;
            fuenteAudio.clip = sonidoClic;

            // Reproducimos
            fuenteAudio.Play();

            // Programamos que este altavoz invisible se borre cuando acabe el MP3
            Destroy(altavozTemporal, sonidoClic.length);
        }

        // 3. Esperamos la animación
        yield return new WaitForSeconds(tiempoDeEspera);

        // 4. Cambiamos de escena
        SceneManager.LoadScene(nombreEscena);
    }
}