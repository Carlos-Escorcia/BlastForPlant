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
    public TextMeshProUGUI textoVidas;
    public string nombreEscenaGameOver = "GameOver";

    private bool esInvulnerable = false;
    private SpriteRenderer spriteRenderer;

    [Header("Sistema de Disparo")]
    public GameObject prefabBala;
    public Transform puntoDeDisparo;
    public float tiempoRecarga = 0.5f; // Tiempo de espera obligado entre disparos

    private float tiempoUltimoDisparo = -10f; // Control interno del tiempo

    // Variables internas
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
        // --- 1. LEER INPUTS ---
        movimientoHorizontal = Input.GetAxisRaw("Horizontal");
        animator.SetFloat("Velocidad", Mathf.Abs(movimientoHorizontal));

        if (movimientoHorizontal > 0) transform.localScale = escalaInicial;
        else if (movimientoHorizontal < 0) transform.localScale = new Vector3(-escalaInicial.x, escalaInicial.y, escalaInicial.z);

        enSuelo = Physics2D.OverlapCircle(controladorSuelo.position, radioSuelo, EsSuelo);
        animator.SetBool("EnSuelo", enSuelo);

        if (enSuelo) puedeDobleSalto = true;

        // --- 2. SALTO ---
        if (Input.GetButtonDown("Jump"))
        {
            if (enSuelo) EjecutarSalto();
            else if (puedeDobleSalto)
            {
                EjecutarSalto();
                puedeDobleSalto = false;
            }
        }

        // --- 3. DISPARO INSTANTÁNEO AL PULSAR ENTER ---
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Disparar();
        }

        // --- 4. GRAVEDAD MEJORADA ---
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (multiplicadorCaida - 1) * Time.deltaTime;
        }
    }

    private void FixedUpdate()
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
        if (esInvulnerable) return;

        vidas--;
        ActualizarUI();

        if (vidas <= 0) SceneManager.LoadScene(nombreEscenaGameOver);
        else StartCoroutine(RutinaInvulnerabilidad());
    }

    private IEnumerator RutinaInvulnerabilidad()
    {
        esInvulnerable = true;
        float duracionParpadeo = 2f;
        float tiempoPasad = 0f;

        while (tiempoPasad < duracionParpadeo)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(0.1f);
            tiempoPasad += 0.1f;
        }

        spriteRenderer.enabled = true;
        esInvulnerable = false;
    }

    // =========================================================
    // --- SISTEMA DE DISPARO INSTANTÁNEO CON RECARGA ---
    // =========================================================

    private void Disparar()
    {
        // Solo entra si el tiempo actual es mayor que la última vez que disparamos + la recarga
        if (Time.time >= tiempoUltimoDisparo + tiempoRecarga)
        {
            // 1. Ejecutamos la animación al instante
            animator.SetTrigger("Disparar");

            // 2. Clonamos la bala al instante
            if (prefabBala != null && puntoDeDisparo != null)
            {
                Instantiate(prefabBala, puntoDeDisparo.position, transform.rotation);
            }

            // 3. Registramos la hora de este disparo para bloquear los siguientes hasta que pase el tiempo
            tiempoUltimoDisparo = Time.time;
        }
    }

    // =========================================================

    private void ActualizarUI()
    {
        if (textoVidas != null) textoVidas.text = "Vidas: " + vidas;
    }

    private void OnDrawGizmosSelected()
    {
        if (controladorSuelo != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(controladorSuelo.position, radioSuelo);
        }
    }
}