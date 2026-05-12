using UnityEngine;

public class ActualizadorCheckpoint : MonoBehaviour
{
    [Header("Referencia al punto único")]
    public Transform puntoDeRespawnGlobal; //Aquí va el objeto Respawn

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si el jugador pisa esta plataforma
        if (collision.CompareTag("Player"))
        {
            // Movemos el punto de respawn a la posición de esta plataforma
            // Le sumamos +1 en el eje Y para que el jugador no aparezca "enterrado"
            puntoDeRespawnGlobal.position = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
        }
    }
}