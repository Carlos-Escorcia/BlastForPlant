using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator), typeof(Rigidbody2D))]
public class ControlEnemigo2 : MonoBehaviour
{
    [Header("Configuración de Visión y Movimiento")]
    public float distanciaDeteccion = 8f;
    [Tooltip("Velocidad a la que camina hacia ti (ej. 2 o 4)")]
    public float velocidadMovimiento = 2f;
    [Tooltip("Distancia a la que se detiene para dispararte")]
    public float distanciaMinima = 3f;

    [Header("Configuración de Disparo")]
    public GameObject prefabBaba;
    public Transform puntoDisparo;
    public float tiempoEntreDisparos = 2f;
    [Tooltip("Milisegundos que espera para que la baba salga justo cuando abre la boca")]
    public float retrasoAnimacionDisparo = 0.2f;

    [Header("Fuerza de la Parábola")]
    public float fuerzaDisparoX = 5f;
    public float fuerzaDisparoY = 4f;

    private Transform jugador;
    private Animator animator;
    private Rigidbody2D rb;
    private float temporizadorDisparo;
    private bool estaDisparando = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        // Buscar al jugador automáticamente por su tag
        GameObject objJugador = GameObject.FindGameObjectWithTag("Player");
        if (objJugador != null)
        {
            jugador = objJugador.transform;
        }
    }

    void Update()
    {
        if (jugador == null) return;

        float distanciaAlJugador = Vector2.Distance(transform.position, jugador.position);

        if (distanciaAlJugador <= distanciaDeteccion)
        {
            MirarAlJugador();

            temporizadorDisparo -= Time.deltaTime;

            if (temporizadorDisparo <= 0 && !estaDisparando)
            {
                StartCoroutine(RutinaAtaque());
                temporizadorDisparo = tiempoEntreDisparos;
            }

            if (!estaDisparando && distanciaAlJugador > distanciaMinima)
            {
                float direccionX = Mathf.Sign(jugador.position.x - transform.position.x);
                rb.linearVelocity = new Vector2(direccionX * velocidadMovimiento, rb.linearVelocity.y);
            }
            else if (!estaDisparando)
            {
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            }
        }
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            temporizadorDisparo = 0.5f;
        }
    }

    private IEnumerator RutinaAtaque()
    {
        estaDisparando = true;
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        animator.SetBool("Atacando", true);

        yield return new WaitForSeconds(retrasoAnimacionDisparo);

        DispararBaba();

        yield return new WaitForSeconds(0.3f);

        animator.SetBool("Atacando", false);
        estaDisparando = false;
    }

    private void DispararBaba()
    {
        if (prefabBaba != null && puntoDisparo != null)
        {
            GameObject baba = Instantiate(prefabBaba, puntoDisparo.position, Quaternion.identity);

            Rigidbody2D rbBaba = baba.GetComponent<Rigidbody2D>();
            if (rbBaba != null)
            {
                float direccionX = transform.localScale.x;
                Vector2 fuerza = new Vector2(fuerzaDisparoX * direccionX, fuerzaDisparoY);
                rbBaba.AddForce(fuerza, ForceMode2D.Impulse);
            }
        }
    }

    private void MirarAlJugador()
    {
        if (jugador.position.x > transform.position.x && transform.localScale.x < 0)
        {
            Girar();
        }
        else if (jugador.position.x < transform.position.x && transform.localScale.x > 0)
        {
            Girar();
        }
    }

    private void Girar()
    {
        Vector3 escala = transform.localScale;
        escala.x *= -1;
        transform.localScale = escala;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Si el slime choca físicamente contra el jugador
        if (collision.gameObject.CompareTag("Player"))
        {
            // Buscamos el componente del jugador para restarle vida
            ControlPersonaje personaje = collision.gameObject.GetComponent<ControlPersonaje>();
            if (personaje != null)
            {
                personaje.RecibirDaño();
            }

            // Destruimos a este enemigo (el slime)
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bala"))
        {
            Destroy(collision.gameObject);

            ControlContador contador = Object.FindFirstObjectByType<ControlContador>();
            if (contador != null) contador.SumarBaja();

            Destroy(gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distanciaDeteccion);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, distanciaMinima);
    }
}