using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))] 
[RequireComponent(typeof(Animator))]
//Componentes para que el script no de error
//(Sacados de la IA, porque no sabía porqué no me iba, y la IA me ha dado esto).
public class ControlPersonaje : MonoBehaviour
{
    [Header("Ajustes de Movimiento")]
    public float velocidadMovimiento = 5f;

    private Rigidbody2D rb;
    private Animator animator;
    private float movimientoHorizontal;

    void Start()
    {
        //Al empezar, el script busca los componentes en tu Personaje
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        //Lee A/D o Flechas Izquierda/Derecha automáticamente
        movimientoHorizontal = Input.GetAxisRaw("Horizontal");

        //CAMBIAR DE ANIMACIÓN
        // Mathf.Abs convierte números negativos a positivos (para que camine hacia la izquierda sin problemas)
        float velocidadActual = Mathf.Abs(movimientoHorizontal);
        animator.SetFloat("Velocidad", velocidadActual);

        // 3. GIRAR EL SPRITE
        if (movimientoHorizontal > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (movimientoHorizontal < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    void FixedUpdate()
    {
        //APLICAR MOVIMIENTO (Las físicas se calculan aquí)
        rb.linearVelocity = new Vector2(movimientoHorizontal * velocidadMovimiento, rb.linearVelocity.y);
    }
}