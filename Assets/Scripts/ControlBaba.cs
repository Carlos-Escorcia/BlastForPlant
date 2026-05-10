using UnityEngine;

public class ControlBaba : MonoBehaviour
{
    [Header("Configuración Básica")]
    public float tiempoDeVida = 3f;

    [Tooltip("El Tag de tu jugador")]
    public string tagJugador = "Player";

    [Tooltip("El Tag del enemigo para que no se dispare a sí mismo")]
    public string tagEnemigo = "Enemigo";

    void Start()
    {
        Destroy(gameObject, tiempoDeVida);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ProcesarImpacto(collision.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ProcesarImpacto(collision.gameObject);
    }

    private void ProcesarImpacto(GameObject objetoTocado)
    {
        // 1. Si la baba choca contra el enemigo que la escupió, lo ignoramos
        if (objetoTocado.CompareTag(tagEnemigo)) return;

        // 2. Si toca al jugador, restamos vida
        if (objetoTocado.CompareTag(tagJugador))
        {
            ControlPersonaje personaje = objetoTocado.GetComponent<ControlPersonaje>();
            if (personaje != null)
            {
                personaje.RecibirDaño();
            }
            Destroy(gameObject);
        }
        // 3. Si toca el suelo, una pared, el techo o cualquier cosa sólida... se destruye.
        else
        {
            Destroy(gameObject);
        }
    }
}