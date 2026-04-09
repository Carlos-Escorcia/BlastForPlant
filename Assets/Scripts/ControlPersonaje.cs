using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class ControlPersonaje : MonoBehaviour
{
    [Header("Ajustes de Movimiento")]
    public float velocidadMovimiento = 5f;
    public float fuerzaSalto = 6.5f;

    [Header("Ajustes de Gravedad")]
    public float multiplicadorCaida = 2.5f;

    [Header("Doble Salto y Suelo")]
    public Transform controladorSuelo;
    public float radioSuelo = 0.2f;
    public LayerMask EsSuelo;

    private Rigidbody2D rb;
    private Animator animator;
    private float movimientoHorizontal;

    private Vector3 escalaInicial;

    private bool enSuelo;
    private bool puedeDobleSalto;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        escalaInicial = transform.localScale;
    }

    void Update()
    {
        movimientoHorizontal = Input.GetAxisRaw("Horizontal");

        //Solo tocamos el suelo si estamos cayendo o quietos
        if (rb.linearVelocity.y <= 0.1f)
        {
            enSuelo = Physics2D.OverlapCircle(controladorSuelo.position, radioSuelo, EsSuelo);
        }
        else
        {
            enSuelo = false; // Si estamos subiendo, obligamos a que el suelo sea falso
        }

        // Avisamos al Animator
        animator.SetBool("EnSuelo", enSuelo);

        if (enSuelo)
        {
            puedeDobleSalto = true;
        }

        float velocidadActual = Mathf.Abs(movimientoHorizontal);
        animator.SetFloat("Velocidad", velocidadActual);

        if (movimientoHorizontal > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(escalaInicial.x), escalaInicial.y, escalaInicial.z);
        }
        else if (movimientoHorizontal < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(escalaInicial.x), escalaInicial.y, escalaInicial.z);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (enSuelo)
            {
                EjecutarSalto();
            }
            else if (puedeDobleSalto)
            {
                EjecutarSalto();
                puedeDobleSalto = false;
            }
        }

        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (multiplicadorCaida - 1) * Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(movimientoHorizontal * velocidadMovimiento, rb.linearVelocity.y);
    }

    private void EjecutarSalto()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaSalto);

        // Forzamos al animator a saber que ya no estamos en el suelo al pulsar la tecla al instante
        enSuelo = false;
        animator.SetBool("EnSuelo", false);

        animator.SetTrigger("Saltar");
    }

    private void OnDrawGizmos()
    {
        if (controladorSuelo != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(controladorSuelo.position, radioSuelo);
        }
    }
}