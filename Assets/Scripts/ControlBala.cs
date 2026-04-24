using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class ControlBala : MonoBehaviour
{
    public float velocidad = 15f;
    public float tiempoDeVida = 1.5f;

    [Header("Sistema de Loot")]
    public GameObject prefabVidaExtra; //Arrastrar el PREFAB aquí

    void Start()
    {
        GetComponent<Rigidbody2D>().linearVelocity = transform.right * velocidad;
        Destroy(gameObject, tiempoDeVida);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemigo"))
        {
            if (prefabVidaExtra != null)
            {
              int sorteo = Random.Range(0, 5); //Elige un número al azar 0-4 

                if (sorteo == 0)
                {
                    Vector3 posicionElevada = collision.transform.position + new Vector3(0f, 1f, 0f);
                    Instantiate(prefabVidaExtra, posicionElevada, Quaternion.identity);
                }
            }

            Destroy(collision.gameObject); //Destruye al enemigo
            Destroy(gameObject); //Destruye la bala
        }
    }
}