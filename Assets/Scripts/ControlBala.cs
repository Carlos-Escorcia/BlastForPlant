using UnityEngine;

// Aseguramos que el objeto tenga estos componentes para que el script funcione
[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class ControlBala : MonoBehaviour
{
    [Header("Ajustes de Movimiento")]
    public float velocidad = 15f;
    public float tiempoDeVida = 1.5f;

    [Header("Sistema de Botín (Loot)")]
    [Tooltip("El objeto que aparecerá a veces al derrotar a un enemigo")]
    public GameObject prefabVidaExtra;

    void Start()
    {
        // Aplicamos velocidad hacia la derecha del objeto (su eje X local)
        GetComponent<Rigidbody2D>().linearVelocity = transform.right * velocidad;

        // Autodestrucción por tiempo para no llenar la memoria de balas perdidas
        Destroy(gameObject, tiempoDeVida);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 1. ¿Es un enemigo?
        if (collision.gameObject.CompareTag("Enemigo"))
        {
            LogicaLoot(collision.transform.position);

            Destroy(collision.gameObject); // Destruye al enemigo
            Destroy(gameObject);           // Destruye la bala
        }
        // 2. Si no es un enemigo, pero tampoco es el jugador (para que no se suicide al disparar)
        else if (!collision.gameObject.CompareTag("Player"))
        {
            // Aquí entra: Suelo, Paredes, Techos, etc.
            Destroy(gameObject);
        }
    }

    // He separado la lógica del loot en un método propio para que el código sea más ordenado
    private void LogicaLoot(Vector3 posicionEnemigo)
    {
        if (prefabVidaExtra != null)
        {
            int sorteo = Random.Range(0, 5); // 0, 1, 2, 3, 4 (20% de probabilidad)

            if (sorteo == 0)
            {
                // Aparece un poco más arriba de donde estaba el enemigo
                Vector3 posicionElevada = posicionEnemigo + new Vector3(0f, 1f, 0f);
                Instantiate(prefabVidaExtra, posicionElevada, Quaternion.identity);
            }
        }
    }
}