using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(SpriteRenderer))]
public class ControlPersonaje : MonoBehaviour
{
    [Header("Ajustes de Movimiento")]
    public float velocidadMovimiento = 5f;
    public float fuerzaSalto = 6.5f;
    public float multiplicadorCaida = 2.5f;

    [Header("Doble Salto y Suelo")]
    public Transform controladorSuelo;
    public float radioSuelo = 0.2f;
    public LayerMask EsSuelo;

    [Header("Sistema de Vidas")]
    public int vidas = 4;
    [Tooltip("Arrastra aquí tu Text (TMP) de la Hierarchy")]
    public TextMeshProUGUI textoVidas;
    public string nombreEscenaGameOver = "GameOver";

    private bool esInvulnerable = false;
    private SpriteRenderer spriteRenderer;

    [Header("Sistema de Disparo")]
    public GameObject prefabBala;
    public Transform puntoDeDisparo;
    public float tiempoRecarga = 1.5f;
    private float tiempoUltimoDisparo = -10f;

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
        spriteRenderer = GetComponent<SpriteRenderer>();
        escalaInicial = transform.localScale;

        ActualizarUI();
    }

    void Update()
    {
        // MOVIMIENTO
        movimientoHorizontal = Input.GetAxisRaw("Horizontal");

        if (rb.linearVelocity.y <= 0.1f) enSuelo = Physics2D.OverlapCircle(controladorSuelo.position, radioSuelo, EsSuelo);
        else enSuelo = false;

        animator.SetBool("EnSuelo", enSuelo);
        if (enSuelo) puedeDobleSalto = true;

        animator.SetFloat("Velocidad", Mathf.Abs(movimientoHorizontal));

        if (movimientoHorizontal > 0) transform.localScale = new Vector3(Mathf.Abs(escalaInicial.x), escalaInicial.y, escalaInicial.z);
        else if (movimientoHorizontal < 0) transform.localScale = new Vector3(-Mathf.Abs(escalaInicial.x), escalaInicial.y, escalaInicial.z);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (enSuelo) EjecutarSalto();
            else if (puedeDobleSalto) { EjecutarSalto(); puedeDobleSalto = false; }
        }

        if (rb.linearVelocity.y < 0) rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (multiplicadorCaida - 1) * Time.deltaTime;

        // DISPARO CON LA TECLA ENTER (Return)
        if (Input.GetKeyDown(KeyCode.Return) && Time.time >= tiempoUltimoDisparo + tiempoRecarga)
        {
            Disparar();
            tiempoUltimoDisparo = Time.time;
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
        enSuelo = false;
        animator.SetBool("EnSuelo", false);
        animator.SetTrigger("Saltar");
    }

    public void RecibirDano()
    {
        if (esInvulnerable) return; // Si está parpadeando, es inmune

        vidas--;
        ActualizarUI();

        if (vidas <= 0)
        {
            SceneManager.LoadScene(nombreEscenaGameOver);
        }
        else
        {
            StartCoroutine(RutinaInvulnerabilidad());
        }
    }

    private IEnumerator RutinaInvulnerabilidad()
    {
        esInvulnerable = true;
        float duracionParpadeo = 2f;
        float tiempoPasad = 0f;

        // Hace que el sprite aparezca y desaparezca creando efecto de parpadeo
        while (tiempoPasad < duracionParpadeo)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(0.1f);
            tiempoPasad += 0.1f;
        }

        spriteRenderer.enabled = true;
        esInvulnerable = false;
    }

    private void Disparar()
    {
        //¡Esta es la instrucción que avisa al Animator para ejecutar la animación!
        animator.SetTrigger("Disparar");

        Instantiate(prefabBala, puntoDeDisparo.position, transform.rotation);
    }

    private void ActualizarUI()
    {
        if (textoVidas != null)
        {
            textoVidas.text = "Vidas: " + vidas;
        }
        else
        {
            //Este aviso te saldrá en amarillo si te olvidas de asignar el texto, sin romper el juego
            Debug.LogWarning("Falta asignar el Texto Vidas en el script ControlPersonaje.");
        }
    }
}