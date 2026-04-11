using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class ControlEnemigo : MonoBehaviour
{
    [Header("Configuración de IA")]
    public float radioDeVision = 6f;
    // Eliminamos el 'radioDeAtaque' porque ahora ataca en cuanto te ve
    public float velocidad = 2f;

    [Header("Referencias")]
    public Transform jugador;

    private Animator animator;
    private Rigidbody2D rb;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        // Autocargar al jugador si se nos olvida en el Inspector
        if (jugador == null)
        {
            GameObject objJugador = GameObject.Find("Personaje");
            if (objJugador != null) jugador = objJugador.transform;
        }
    }

    void Update()
    {
        if (jugador == null) return;

        float distancia = Vector2.Distance(transform.position, jugador.position);

        // --- LÓGICA SIMPLIFICADA: 2 ESTADOS ---

        // Si el jugador entra en el campo de visión...
        if (distancia <= radioDeVision)
        {
            // 1. Cambiamos la animación a ATAQUE
            animator.SetBool("Atacando", true);

            // 2. Nos movemos hacia el jugador
            float direccionX = (jugador.position.x - transform.position.x) > 0 ? 1 : -1;
            rb.linearVelocity = new Vector2(direccionX * velocidad, rb.linearVelocity.y);

            // 3. Volteamos el sprite si es necesario
            MirarAlJugador();
        }
        else // Si el jugador está lejos...
        {
            // 1. Volvemos a la animación IDLE
            animator.SetBool("Atacando", false);

            // 2. Frenamos al enemigo para que no siga resbalando
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
    }

    private void MirarAlJugador()
    {
        // Lógica matemática para voltear el sprite según la posición del jugador
        if (jugador.position.x < transform.position.x)
        {
            // Mira a la izquierda
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else
        {
            // Mira a la derecha
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Si tocamos físicamente al jugador, le hacemos daño y destruimos este enemigo
        if (collision.gameObject.CompareTag("Player"))
        {
            ControlPersonaje personaje = collision.gameObject.GetComponent<ControlPersonaje>();
            if (personaje != null) personaje.RecibirDano();

            Destroy(gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Solo dibujamos la esfera de visión para mantener el editor limpio
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radioDeVision);
    }
}