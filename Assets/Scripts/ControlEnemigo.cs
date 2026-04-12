using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class ControlEnemigo : MonoBehaviour
{
    [Header("Configuración de IA")]
    public float radioDeVision = 6f;

    // Cambiamos 'velocidad' por fuerzas de salto
    [Tooltip("Fuerza horizontal (X) y vertical (Y) del salto")]
    public Vector2 fuerzaSalto = new Vector2(3f, 5f);
    public float tiempoEntreSaltos = 1.2f; // Segundos que espera en el suelo antes de volver a saltar

    [Header("Detección de Suelo")]
    public Transform puntoSuelo; // Un GameObject vacío a los pies del enemigo
    public float radioSuelo = 0.2f;
    public LayerMask capaSuelo; // Para diferenciar qué es suelo y qué no

    [Header("Referencias")]
    public Transform jugador;

    private Animator animator;
    private Rigidbody2D rb;
    private float temporizadorSalto;
    private bool tocandoSuelo;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        // Autocargar al jugador si se nos olvida
        if (jugador == null)
        {
            GameObject objJugador = GameObject.Find("Personaje");
            if (objJugador != null) jugador = objJugador.transform;
        }
    }

    void Update()
    {
        if (jugador == null) return;

        // 1. Verificamos si estamos tocando el suelo usando un pequeño círculo invisible
        tocandoSuelo = Physics2D.OverlapCircle(puntoSuelo.position, radioSuelo, capaSuelo);

        float distancia = Vector2.Distance(transform.position, jugador.position);

        // --- LÓGICA DE 2 ESTADOS ---

        // ESTADO 2: ATAQUE / PERSECUCIÓN
        if (distancia <= radioDeVision)
        {
            animator.SetBool("Atacando", true);
            MirarAlJugador();

            // Lógica del temporizador para los saltos
            if (temporizadorSalto > 0)
            {
                temporizadorSalto -= Time.deltaTime; // Restamos tiempo
            }

            // Si está en el suelo y el temporizador llegó a 0, ¡Salta!
            if (tocandoSuelo && temporizadorSalto <= 0f)
            {
                DarSaltito();
                temporizadorSalto = tiempoEntreSaltos; // Reiniciamos el temporizador
            }
        }
        // ESTADO 1: IDLE / REPOSO
        else
        {
            animator.SetBool("Atacando", false);

            // Frenamos suavemente si está en el suelo para que no resbale
            if (tocandoSuelo)
            {
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            }
        }
    }

    private void DarSaltito()
    {
        // Reseteamos la velocidad actual para que las fuerzas no se acumulen y salte de más
        rb.linearVelocity = Vector2.zero;

        // Calculamos la dirección (1 = derecha, -1 = izquierda)
        float direccionX = (jugador.position.x - transform.position.x) > 0 ? 1 : -1;

        // Creamos el vector de fuerza diagonal
        Vector2 impulso = new Vector2(fuerzaSalto.x * direccionX, fuerzaSalto.y);

        // Aplicamos la fuerza de golpe (Impulse)
        rb.AddForce(impulso, ForceMode2D.Impulse);
    }

    private void MirarAlJugador()
    {
        if (jugador.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Nota de diseño: Idealmente usa TryGetComponent, es más moderno y seguro
            if (collision.gameObject.TryGetComponent(out ControlPersonaje personaje))
            {
                personaje.RecibirDano();
            }
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radioDeVision);

        // Dibujamos el círculo del suelo para poder configurarlo fácil en el editor
        if (puntoSuelo != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(puntoSuelo.position, radioSuelo);
        }
    }
}