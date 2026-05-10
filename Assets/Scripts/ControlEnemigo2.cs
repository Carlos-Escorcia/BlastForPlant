using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator), typeof(Rigidbody2D))]
public class ControlEnemigo2 : MonoBehaviour
{
    [Header("Configuración de Visión y Movimiento")]
    public float distanciaDeteccion = 10f;
    public float velocidadMovimiento = 2f;
    public float distanciaMinima = 3f;

    [Header("Configuración de Disparo")]
    public GameObject prefabBaba;
    public Transform puntoDisparo;
    public float tiempoEntreDisparos = 2f;
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

        GameObject objJugador = GameObject.FindGameObjectWithTag("Player");
        if (objJugador != null) jugador = objJugador.transform;
    }

    void Update()
    {
        if (jugador == null) return;

        float distanciaAlJugador = Vector2.Distance(transform.position, jugador.position);

        if (distanciaAlJugador <= distanciaDeteccion)
        {
            float diferenciaX = jugador.position.x - transform.position.x;
            if (Mathf.Abs(diferenciaX) > 0.1f)
            {
                if (diferenciaX > 0 && transform.localScale.x < 0) Girar();
                else if (diferenciaX < 0 && transform.localScale.x > 0) Girar();
            }

            if (!estaDisparando && Time.time >= temporizadorDisparo)
            {
                StartCoroutine(RutinaDisparar());
                temporizadorDisparo = Time.time + tiempoEntreDisparos;
            }

            if (!estaDisparando && distanciaAlJugador > distanciaMinima)
            {
                Vector2 objetivo = new Vector2(jugador.position.x, transform.position.y);
                transform.position = Vector2.MoveTowards(transform.position, objetivo, velocidadMovimiento * Time.deltaTime);

                // animator.SetBool("Caminando", true); // <-- APAGADO PARA EVITAR EL ERROR
            }
            else
            {
                // animator.SetBool("Caminando", false); // <-- APAGADO PARA EVITAR EL ERROR
            }
        }
        else
        {
            // animator.SetBool("Caminando", false); // <-- APAGADO PARA EVITAR EL ERROR
        }
    }

    private IEnumerator RutinaDisparar()
    {
        estaDisparando = true;

        // animator.SetTrigger("Disparar"); // <-- APAGADO PARA EVITAR EL ERROR (AQUÍ ESTABA EL PROBLEMA)

        yield return new WaitForSeconds(retrasoAnimacionDisparo);

        if (prefabBaba != null && puntoDisparo != null)
        {
            GameObject babaCreada = Instantiate(prefabBaba, puntoDisparo.position, Quaternion.identity);
            Rigidbody2D rbBaba = babaCreada.GetComponent<Rigidbody2D>();

            if (rbBaba != null)
            {
                float direccionX = (transform.localScale.x > 0) ? 1f : -1f;
                rbBaba.linearVelocity = new Vector2(fuerzaDisparoX * direccionX, fuerzaDisparoY);
            }
        }

        yield return new WaitForSeconds(0.5f);
        estaDisparando = false;
    }

    private void Girar()
    {
        Vector3 escala = transform.localScale;
        escala.x *= -1;
        transform.localScale = escala;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ControlPersonaje personaje = collision.gameObject.GetComponent<ControlPersonaje>();
            if (personaje != null) personaje.RecibirDaño();
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
}