using UnityEngine;

public class VidaExtra : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si lo que nos toca tiene la etiqueta "Player"...
        if (collision.CompareTag("Player"))
        {
            // Buscamos el script del personaje en ese objeto
            ControlPersonaje personaje = collision.GetComponent<ControlPersonaje>();

            if (personaje != null)
            {
                // Le damos la vida y destruimos el objeto de la escena
                personaje.GanarVida();
                Destroy(gameObject);
            }
        }
    }
}
