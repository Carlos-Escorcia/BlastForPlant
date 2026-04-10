using UnityEngine;

//Esto asegura que Unity añada automáticamente estos componentes si no los tienes
[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class ControlEnemigo : MonoBehaviour
{
    [Header("Configuración de Distancias")]
    [Tooltip("Distancia a la que el enemigo detecta al jugador")]
    public float radioDeVision = 5f;
    [Tooltip("Distancia a la que el enemigo ataca")]
    public float radioDeAtaque = 1.5f;
    [Tooltip("Velocidad de movimiento del enemigo")]
    public float velocidad = 3f;

    [Header("Referencias")]
    [Tooltip("Arrastra aquí el objeto 'Personaje' desde la Hierarchy")]
    public Transform jugador;

    private Rigidbody2D rb;
    private Animator animator;
    private bool estaAtacando = false;

    void Start()
    {
        // Obtenemos los componentes pegados a este mismo GameObject ("Enemigo")
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Buena práctica: Si olvidaste asignar al jugador en el Inspector, el código intenta buscarlo.
        if (jugador == null)
        {
            GameObject objJugador = GameObject.Find("Personaje");
            if (objJugador != null) jugador = objJugador.transform;
        }
    }

    void Update()
    {
        // Si el jugador no existe (por ejemplo, si muere y es destruido), no hacemos nada
        if (jugador == null) return;

        // Calculamos la distancia entre el enemigo y el jugador
        float distanciaAlJugador = Vector2.Distance(transform.position, jugador.position);

        // Lógica de la máquina de estados
        if (distanciaAlJugador <= radioDeAtaque)
        {
            Atacar();
        }
        else if (distanciaAlJugador <= radioDeVision)
        {
            Perseguir();
        }
        else
        {
            EstarEnReposo();
        }
    }

    private void EstarEnReposo()
    {
        estaAtacando = false;

        // En Unity 6 usamos linearVelocity en lugar del antiguo velocity
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

        // Desactivamos la animación de movimiento para que vuelva a "Enemigo_Idle"
        animator.SetBool("Moviendose", false);
    }

    private void Perseguir()
    {
        estaAtacando = false;
        animator.SetBool("Moviendose", true); // Activamos la animación de caminar

        // Calculamos en qué dirección está el jugador respecto al enemigo
        Vector2 direccion = (jugador.position - transform.position).normalized;

        // Movemos al enemigo en el eje X, manteniendo su velocidad de caída en Y
        rb.linearVelocity = new Vector2(direccion.x * velocidad, rb.linearVelocity.y);

        // Hacemos que el sprite gire hacia donde está caminando
        GirarSprite(direccion.x);
    }

    private void Atacar()
    {
        if (!estaAtacando)
        {
            estaAtacando = true;
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); // Debe frenar para atacar
            animator.SetBool("Moviendose", false);

            // Disparamos la transición de ataque en el Animator
            animator.SetTrigger("Atacar");

            // NOTA: Aquí más adelante añadiremos el código para quitarle vida al jugador.
        }
    }

    private void GirarSprite(float direccionX)
    {
        // Si la dirección es positiva (derecha), la escala es 1. Si es negativa (izquierda), es -1.
        if (direccionX > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (direccionX < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    // Esta función especial de Unity dibuja esferas en la escena para ayudarte a ajustar las distancias visualmente
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radioDeVision);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radioDeAtaque);
    }
}