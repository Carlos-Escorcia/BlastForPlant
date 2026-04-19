using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections; //ESTO ES VITAL PARA PODER HACER QUE EL JUEGO ESPERE

public class ControlPausa : MonoBehaviour
{
    [Header("Lo que aparece al pausar")]
    // Aquí arrastraremos el objeto que contiene tu botón MENU (y el fondo blanquito si lo tiene)
    public GameObject todoElMenuPausa;

    [Header("El Botón de la Mochila")]
    public Image imagenMochila;
    public Sprite mochilaCerrada;
    public Sprite mochilaAbierta;

    [Header("Animación del Botón Menú")]
    public Animator animadorMenu; //Aquí conectaremos el Animator de tu botón
    public float tiempoDeEspera = 0.5f; //Medio segundo para disfrutar tu animación

    private bool estaPausado = false;

    void Start()
    {
        // 1. Al arrancar el nivel, nos aseguramos de que el tiempo fluye normal
        Time.timeScale = 1f;

        // 2. Apagamos el menú grande de pausa para que no moleste
        if (todoElMenuPausa != null)
        {
            todoElMenuPausa.SetActive(false);
        }

        // 3. Ponemos el dibujo de la mochila cerrada
        if (imagenMochila != null)
        {
            imagenMochila.sprite = mochilaCerrada;
        }
    }

    // Esta es la ÚNICA función que le pondremos al botón de la Mochila
    public void TocarMochila()
    {
        if (estaPausado == true)
        {
            // --- SI ESTABA PAUSADO, LO REANUDAMOS ---
            estaPausado = false;
            Time.timeScale = 1f; // Todo vuelve a moverse

            todoElMenuPausa.SetActive(false); // Escondemos el botón MENU
            imagenMochila.sprite = mochilaCerrada; // Cerramos la mochila
        }
        else
        {
            // --- SI ESTÁBAMOS JUGANDO NORMAL, PAUSAMOS ---
            estaPausado = true;
            Time.timeScale = 0f; // Los enemigos, el jugador y el tiempo se congelan

            todoElMenuPausa.SetActive(true); // Mostramos el botón MENU en pantalla
            imagenMochila.sprite = mochilaAbierta; // Abrimos la mochila
        }
    }

    // Esta función se la pondremos SOLAMENTE al botón gigante de "MENU"
    public void BotonIrAlMenu(string nombreEscenaPrincipal)
    {
        // ¡CRÍTICO! Descongelar el tiempo antes de viajar, o el menú principal no funcionará
        Time.timeScale = 1f;
        SceneManager.LoadScene(nombreEscenaPrincipal);
    }
    // ESTA ES LA MAGIA QUE HACE QUE ESPERE
    private IEnumerator CargarMenuConRetraso(string nombreEscena)
    {
        // 1. Descongelamos el tiempo para que la animación pueda moverse
        Time.timeScale = 1f;

        // 2. Disparamos la animación. (En tu foto vi que el estado se llama "Play")
        if (animadorMenu != null)
        {
            animadorMenu.Play("Play");
        }

        // 3. Esperamos el tiempo que has marcado (0.5 segundos)
        yield return new WaitForSeconds(tiempoDeEspera);

        // 4. AHORA SÍ, la animación ha terminado y cambiamos de escena
        SceneManager.LoadScene(nombreEscena);
    }
}