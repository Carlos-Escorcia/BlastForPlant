using UnityEngine;
using UnityEngine.SceneManagement; //Cambio de escenas

public class CambioEscena : MonoBehaviour
{
    [Header("Configuración del Nivel")]
    public string nombreSiguienteEscena;

    //Esta función se activa automáticamente cuando alguien entra en la zona "Trigger"
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Se comprueba si el objeto tiene la etiqueta "Player"
        if (collision.CompareTag("Player"))
        {
            //Si es el jugador, cambia de escena a la que esté puesta
            SceneManager.LoadScene(nombreSiguienteEscena);
        }
    }
}
