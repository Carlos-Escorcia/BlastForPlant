using UnityEngine;
using System.Collections; // NUEVO: Necesario para usar las Corrutinas

[RequireComponent(typeof(Animator), typeof(Rigidbody2D))]
public class ControlEnemigo2 : MonoBehaviour
{
    [Header("Configuración de Visión y Movimiento")]
    public float distanciaDeteccion = 8f; // A qué distancia te ve
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
    private bool estaDisparando = false; // Nueva variable para controlar la animación

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

        // Comprobar la distancia entre el enemigo y el jugador
        float distanciaAlJugador = Vector2.Distance(transform.position, jugador.position);

        // Si estamos dentro del rango de visión (nos ha detectado)
        if (distanciaAlJugador <= distanciaDeteccion)
        {
            MirarAlJugador();

            // 1. LÓGICA DE DISPARO
            temporizadorDisparo -= Time.deltaTime;
            // Solo dispara si el temporizador llega a 0 Y además no está ya disparando
            if (temporizadorDisparo <= 0 && !estaDisparando)
            {
                StartCoroutine(RutinaAtaque());
                temporizadorDisparo = tiempoEntreDisparos;
            }

            // 2. LÓGICA DE MOVIMIENTO HORIZONTAL
            // Solo camina si NO está disparando y si aún no está demasiado cerca
            if (!estaDisparando && distanciaAlJugador > distanciaMinima)
            {
                // Calcula si el jugador está a la derecha (1) o izquierda (-1)
                float direccionX = Mathf.Sign(jugador.position.x - transform.position.x);
                // Le aplica velocidad horizontal manteniendo su gravedad natural
                rb.linearVelocity = new Vector2(direccionX * velocidadMovimiento, rb.linearVelocity.y);
            }
            else if (!estaDisparando)
            {
                // Si ya está lo suficientemente cerca, se detiene
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            }
        }
        else
        {
            // Si el jugador se aleja mucho (fuera del rango de visión), se detiene
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            temporizadorDisparo = 0.5f;
        }
    }

    // NUEVO: La corrutina que sincroniza a la perfección la animación con el proyectil
    private IEnumerator RutinaAtaque()
    {
        estaDisparando = true;

        // 1. Lo detenemos en seco para que no resbale por el suelo mientras dispara
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

        // 2. Activamos la animación de ataque
        animator.SetBool("Atacando", true);

        // 3. Esperamos una fracción de segundo para que coincida con el fotograma donde "escupe"
        yield return new WaitForSeconds(retrasoAnimacionDisparo);

        // 4. Creamos la baba (proyectil)
        DispararBaba();

        // 5. Esperamos un poco más para que la animación termine visualmente
        yield return new WaitForSeconds(0.3f);

        // 6. Volvemos al estado Idle (parado)
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
        // Círculo rojo: Rango de visión
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distanciaDeteccion);

        // Círculo azul: Distancia mínima a la que se detiene
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, distanciaMinima);
    }
}