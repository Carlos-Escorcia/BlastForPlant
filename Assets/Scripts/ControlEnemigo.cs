using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class ControlEnemigo : MonoBehaviour
{
    [Header("Configuración")]
    [Tooltip("Distancia a la que el enemigo cambia de Idle a Ataque")]
    public float radioDeAtaque = 1.5f;

    [Header("Referencias")]
    public Transform jugador;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

        // Si nos olvidamos de poner al jugador en el Inspector, el código lo busca por su nombre
        if (jugador == null) jugador = GameObject.Find("Personaje").transform;
    }

    void Update()
    {
        // Si el jugador no existe (ej. fue destruido), no hacemos nada
        if (jugador == null) return;

        // --- 1. LÓGICA PARA VOLTEAR EL SPRITE (MIRAR AL JUGADOR) ---
        // Comparamos la posición en el eje X del jugador frente a la del enemigo
        if (jugador.position.x < transform.position.x)
        {
            // El jugador está a la izquierda. Invertimos la escala X del enemigo para que lo mire.
            // (Si tu enemigo se pone de espaldas, simplemente quita el signo "-" de aquí abajo)
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else
        {
            // El jugador está a la derecha. Ponemos la escala X en positivo.
            // (Y ponle el signo "-" aquí si tuviste que quitarlo arriba)
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        // --- 2. LÓGICA PARA CAMBIAR DE IDLE A ATACAR ---
        // Calculamos la distancia exacta en metros de Unity entre ambos
        float distanciaAlJugador = Vector2.Distance(transform.position, jugador.position);

        if (distanciaAlJugador <= radioDeAtaque)
        {
            // Está dentro del radio: Activamos el booleano para que pase a la animación de ataque
            animator.SetBool("Atacando", true);
        }
        else
        {
            // Está lejos: Desactivamos el booleano para que vuelva a su Idle
            animator.SetBool("Atacando", false);
        }
    }

    // --- 3. LÓGICA PARA HACER DAÑO AL JUGADOR ---
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Detecta si lo que ha tocado nuestro cuerpo físico tiene la etiqueta "Player"
        if (collision.gameObject.CompareTag("Player"))
        {
            ControlPersonaje personaje = collision.gameObject.GetComponent<ControlPersonaje>();
            if (personaje != null)
            {
                // Le quitamos una vida
                personaje.RecibirDano();
            }

            // Inmediatamente después de golpear, el enemigo se destruye (desaparece)
            Destroy(gameObject);
        }
    }

    //Esta función dibuja un círculo rojo en el editor para que veas hasta dónde llega tu radio de ataque
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radioDeAtaque);
    }
}