using UnityEngine;

public class ControlBaba : MonoBehaviour
{
    public float daño = 1f; // Aunque tu sistema usa vidas enteras, lo dejamos por si acaso
    public float tiempoDeVida = 3f; // Destruir después de unos segundos si no toca nada

    void Start()
    {
        // Destruir automáticamente después de un tiempo para no saturar la memoria
        Destroy(gameObject, tiempoDeVida);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si la baba toca al jugador
        if (collision.gameObject.CompareTag("Player"))
        {
            ControlPersonaje personaje = collision.gameObject.GetComponent<ControlPersonaje>();
            if (personaje != null)
            {
                personaje.RecibirDaño(); // Usamos el método que ya tienes en tu jugador
            }
            Destroy(gameObject); // Destruir la baba al impactar
        }
        // Si toca el suelo o las paredes (asegúrate de que tu capa de suelo se llame "Ground" o cambia esto)
        else if (collision.gameObject.layer == LayerMask.NameToLayer("EsSuelo") || collision.gameObject.layer == LayerMask.NameToLayer("EsTile"))
        {
            Destroy(gameObject);
        }
    }
}