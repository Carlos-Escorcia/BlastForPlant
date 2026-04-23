using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class ControlBala : MonoBehaviour
{
    public float velocidad = 15f;
    public float tiempoDeVida = 1.5f;

    [Header("Sistema de Loot")]
    [Tooltip("Arrastra aquí tu PREFAB de la Vida Extra")]
    public GameObject prefabVidaExtra;

    void Start()
    {
        GetComponent<Rigidbody2D>().linearVelocity = transform.right * velocidad;
        Destroy(gameObject, tiempoDeVida);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemigo"))
        {
            // --- EL SORTEO DEL BOTÍN (1/5) ---
            if (prefabVidaExtra != null)
            {
                // Random.Range(0, 5) elige un número al azar: 0, 1, 2, 3 o 4. (Total 5 opciones)
                int sorteo = Random.Range(0, 5);

                // Si sale el 0 (una probabilidad de 1 entre 5), creamos la vida.
                if (sorteo == 0)
                {
                    Instantiate(prefabVidaExtra, collision.transform.position, Quaternion.identity);
                }
            }

            Destroy(collision.gameObject); //Destruye al enemigo
            Destroy(gameObject); //Destruye la bala
        }
    }
}