using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(SpriteRenderer))]
public class ControlPersonaje : MonoBehaviour
{
    [Header("Conexión con el Mapa Azul (Input System)")]
    [Tooltip("Arrastra aquí tu acción de Moverse")]
    public InputActionReference accionMover;
    [Tooltip("Arrastra aquí tu acción de Saltar")]
    public InputActionReference accionSaltar;
    [Tooltip("Arrastra aquí tu acción de Disparar")]
    public InputActionReference accionDisparar;

    [Header("Ajustes de Movimiento")]
    public float velocidadMovimiento = 5f;
    public float fuerzaSalto = 6.5f;
    public float multiplicadorCaida = 2.5f;

    [Header("Doble Salto y Suelo")]
    public Transform controladorSuelo;
    public float radioSuelo = 0.2f;
    public LayerMask EsSuelo;
    [Tooltip("Capa exclusiva de los Tilemaps para que suenen los pasos")]
    public LayerMask EsTile;

    [Header("Sistema de Vidas e Interfaz")]
    public int vidasMaximas = 3;
    public int vidas = 3;
    public float tiempoInvulnerabilidad = 0.8f;
    public Animator BarraVida;
    public TextMeshProUGUI textoVidas;
    public string nombreEscenaGame_Over = "Game Over";

    [Header("Efectos de Sonido del Personaje")]
    public AudioClip sonidoDaño;
    public AudioClip sonidoDisparo;
    public AudioClip sonidoPaso;
    public AudioClip sonidoSalto;
    public float tiempoEntrePasos = 0.3f;
    [Range(0f, 1f)]
    public float volumenPasos = 0.2f;

    [Header("Sistema de Disparo Fluido")]
    public GameObject prefabBala;
    public Transform puntoDeDisparo;
    public float tiempoRecarga = 0.25f;
    public float retrasoBala = 0.05f;
    public float tiempoRecuperacion = 0.1f;

    private bool estaDisparando = false;

    // Variables internas
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private AudioSource fuenteAudio;
    private bool esInvulnerable = false;
    private float tiempoUltimoDisparo = -10f;
    private float movimientoHorizontal;
    private Vector3 escalaInicial;
    private bool enSuelo;
    private bool puedeDobleSalto;
    private float siguienteTiempoPaso = 0f;
    private bool pisandoTile;

    // Variable de estado para saber si el jugador ha muerto
    private bool estaMuerto = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        fuenteAudio = GetComponent<AudioSource>();

        if (fuenteAudio != null)
        {
            fuenteAudio.spatialBlend = 0f;
            fuenteAudio.playOnAwake = false;
        }
    }

    private void OnEnable()
    {
        if (accionMover != null) accionMover.action.Enable();
        if (accionSaltar != null) accionSaltar.action.Enable();
        if (accionDisparar != null) accionDisparar.action.Enable();
    }

    private void OnDisable()
    {
        if (accionMover != null) accionMover.action.Disable();
        if (accionSaltar != null) accionSaltar.action.Disable();
        if (accionDisparar != null) accionDisparar.action.Disable();
    }

    void Start()
    {
        escalaInicial = transform.localScale;
        ActualizarUI();
    }

    void Update()
    {
        // Si el personaje está muerto, bloqueamos el resto del código completamente
        if (estaMuerto) return;

        // 1. LEEMOS EL MAPA AZUL DIRECTAMENTE
        float inputMando = 0f;
        bool botonSalto = false;
        bool botonDisparo = false;

        // Si hemos conectado las acciones en el Inspector, leemos sus valores
        if (accionMover != null) inputMando = accionMover.action.ReadValue<float>();
        if (accionSaltar != null) botonSalto = accionSaltar.action.WasPressedThisFrame();
        if (accionDisparar != null) botonDisparo = accionDisparar.action.WasPressedThisFrame();


        // 2. LÓGICA DE MOVIMIENTO NORMAL
        if (estaDisparando)
        {
            movimientoHorizontal = 0f;
            animator.SetFloat("Velocidad", 0f);
        }
        else
        {
            movimientoHorizontal = inputMando;
            animator.SetFloat("Velocidad", Mathf.Abs(movimientoHorizontal));

            if (movimientoHorizontal > 0) transform.localScale = escalaInicial;
            else if (movimientoHorizontal < 0) transform.localScale = new Vector3(-escalaInicial.x, escalaInicial.y, escalaInicial.z);
        }

        enSuelo = Physics2D.OverlapCircle(controladorSuelo.position, radioSuelo, EsSuelo);
        animator.SetBool("EnSuelo", enSuelo);

        pisandoTile = Physics2D.OverlapCircle(controladorSuelo.position, radioSuelo, EsTile);

        // Pasos
        if (pisandoTile && Mathf.Abs(movimientoHorizontal) > 0 && !estaDisparando)
        {
            if (Time.time >= siguienteTiempoPaso)
            {
                if (sonidoPaso != null && fuenteAudio != null)
                {
                    fuenteAudio.PlayOneShot(sonidoPaso, volumenPasos);
                }
                siguienteTiempoPaso = Time.time + tiempoEntrePasos;
            }
        }

        if (enSuelo) puedeDobleSalto = true;

        if (botonSalto && !estaDisparando)
        {
            if (enSuelo) EjecutarSalto();
            else if (puedeDobleSalto)
            {
                EjecutarSalto();
                puedeDobleSalto = false;
            }
        }

        if (botonDisparo)
        {
            Disparar();
        }

        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (multiplicadorCaida - 1) * Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        // Si está muerto, bloqueamos el movimiento de físicas
        if (estaMuerto) return;

        rb.linearVelocity = new Vector2(movimientoHorizontal * velocidadMovimiento, rb.linearVelocity.y);
    }

    private void EjecutarSalto()
    {
        if (sonidoSalto != null && fuenteAudio != null)
        {
            fuenteAudio.PlayOneShot(sonidoSalto);
        }

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaSalto);
        enSuelo = false;
        animator.SetBool("EnSuelo", false);
        animator.SetTrigger("Saltar");
    }

    public void RecibirDaño()
    {
        // Bloqueo estricto: Si ya murió, no ejecuta absolutamente nada más
        if (esInvulnerable || estaMuerto) return;

        // NUEVO - El "Martillazo": Cortamos cualquier sonido de paso o salto inmediatamente.
        if (fuenteAudio != null) fuenteAudio.Stop();

        // Ahora sí, reproducimos el sonido del daño limpio.
        if (sonidoDaño != null && fuenteAudio != null) fuenteAudio.PlayOneShot(sonidoDaño);

        PerderVida();

        if (vidas <= 0)
        {
            StartCoroutine(MuerteConRetraso());
        }
        else
        {
            StartCoroutine(RutinaInvulnerabilidad());
        }
    }

    public void PerderVida()
    {
        vidas--;
        ActualizarUI();
    }

    public void GanarVida()
    {
        if (vidas < vidasMaximas)
        {
            vidas++;
            ActualizarUI();
        }
    }

    private IEnumerator RutinaInvulnerabilidad()
    {
        esInvulnerable = true;
        float tiempoPasad = 0f;

        while (tiempoPasad < tiempoInvulnerabilidad)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(0.1f);
            tiempoPasad += 0.1f;
        }

        spriteRenderer.enabled = true;
        esInvulnerable = false;
    }

    private IEnumerator MuerteConRetraso()
    {
        // 1. Apagamos sistemas internos
        estaMuerto = true;
        esInvulnerable = true;

        // 2. NUEVO: Reseteamos los valores a mano para que el Animator no piense que sigues caminando
        movimientoHorizontal = 0f;
        if (animator != null)
        {
            animator.SetFloat("Velocidad", 0f);
        }

        // 3. Lo ocultamos y detenemos físicas
        spriteRenderer.enabled = false;
        rb.linearVelocity = Vector2.zero;
        rb.simulated = false;

        // 4. Esperamos a que suene el hit de muerte
        float tiempoEspera = (sonidoDaño != null) ? sonidoDaño.length : 1f;
        yield return new WaitForSeconds(tiempoEspera);

        // 5. Cambiamos de escena
        SceneManager.LoadScene(nombreEscenaGame_Over);
    }

    private void Disparar()
    {
        if (Time.time >= tiempoUltimoDisparo + tiempoRecarga)
        {
            estaDisparando = true;

            animator.SetTrigger("Disparar");
            StartCoroutine(EsperarParaDisparar());
            tiempoUltimoDisparo = Time.time;
        }
    }

    private IEnumerator EsperarParaDisparar()
    {
        yield return new WaitForSeconds(retrasoBala);

        if (sonidoDisparo != null && fuenteAudio != null)
        {
            fuenteAudio.PlayOneShot(sonidoDisparo);
        }

        if (prefabBala != null && puntoDeDisparo != null)
        {
            GameObject balaCreada = Instantiate(prefabBala, puntoDeDisparo.position, transform.rotation) as GameObject;
            float direccion = (transform.localScale.x > 0) ? 1f : -1f;

            ControlBala scriptBala = balaCreada.GetComponent<ControlBala>();
            if (scriptBala != null) scriptBala.velocidad = Mathf.Abs(scriptBala.velocidad) * direccion;

            balaCreada.transform.localScale = new Vector3(Mathf.Abs(balaCreada.transform.localScale.x) * direccion, balaCreada.transform.localScale.y, balaCreada.transform.localScale.z);
        }

        yield return new WaitForSeconds(tiempoRecuperacion);

        estaDisparando = false;
    }

    private void ActualizarUI()
    {
        if (textoVidas != null) textoVidas.text = "Vidas: " + vidas;
        if (BarraVida != null) BarraVida.SetInteger("VidasActuales", vidas);
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