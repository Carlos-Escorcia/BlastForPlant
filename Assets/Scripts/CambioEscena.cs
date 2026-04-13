using UnityEngine;
using UnityEngine.SceneManagement; //Cambio de escenas

public class CambioEscena : MonoBehaviour
{
    [Header("Configuración del Nivel")]
    [Tooltip("Escribe aquí el nombre EXACTO de la escena a la que quieres ir")]
    public string nombreSiguienteEscena;

    //Esta función se activa automáticamente cuando alguien entra en la zona "Trigger"
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Comprobamos si el objeto que acaba de entrar tiene la etiqueta "Player" (tu personaje)
        if (collision.CompareTag("Player"))
        {
            //Si es el jugador, cargamos la escena que hayas escrito en el Inspector
            SceneManager.LoadScene(nombreSiguienteEscena);
        }
    }
}
