using UnityEngine;

public class ControlBaba : MonoBehaviour
{
    [Header("Configuración Básica")]
    public float tiempoDeVida = 3f;

    public string tagJugador = "Player";
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
        //Si la baba choca contra el enemigo que la escupió, no pasa nada
        if (objetoTocado.CompareTag(tagEnemigo)) return;

        //Si toca al jugador, resta vida
        if (objetoTocado.CompareTag(tagJugador))
        {
            ControlPersonaje personaje = objetoTocado.GetComponent<ControlPersonaje>();
            if (personaje != null)
            {
                personaje.RecibirDaño();
            }
            Destroy(gameObject);
        }
        //Si toca suelo, pared, techo o cualquier cosa sólida... se destruye
        else
        {
            Destroy(gameObject);
        }
    }
}