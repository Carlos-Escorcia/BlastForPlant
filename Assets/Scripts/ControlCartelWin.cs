using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlCartelWin : MonoBehaviour
{
    [Header("Configuración")]
    public GameObject arbol;
    public string nombreEscenaWin = "Win";
    public float tiempoEsperaExtra = 0.5f;

    [Header("Sonidos")]
    public AudioClip sonidoTocarCartel; // <--- EL NUEVO SONIDO DEL CARTEL
    public AudioClip sonidoCrecer;      // El sonido del árbol

    private Animator animatorArbol;
    private bool yaActivado = false;

    void Start()
    {
        if (arbol != null)
        {
            animatorArbol = arbol.GetComponent<Animator>();
            arbol.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !yaActivado)
        {
            yaActivado = true;
            StartCoroutine(SecuenciaVictoria());
        }
    }

    private IEnumerator SecuenciaVictoria()
    {
        // 1. REPRODUCIMOS EL SONIDO DEL CARTEL Y ESPERAMOS
        if (sonidoTocarCartel != null)
        {
            AudioSource.PlayClipAtPoint(sonidoTocarCartel, Camera.main.transform.position);

            // Le decimos al código que espere exactamente los segundos que dura este audio
            yield return new WaitForSeconds(sonidoTocarCartel.length);
        }

        // 2. APAGAMOS EL CARTEL (Ahora sí, después de que acabe el primer sonido)
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;

        if (arbol != null)
        {
            // 3. PREPARAMOS EL ÁRBOL
            arbol.transform.position = transform.position;
            arbol.SetActive(true);

            // 4. REPRODUCIMOS EL SONIDO DEL ÁRBOL
            if (sonidoCrecer != null)
            {
                AudioSource.PlayClipAtPoint(sonidoCrecer, Camera.main.transform.position);
            }

            // --- LA LÍNEA MÁGICA: Esperamos 1 frame para que el Animator "despierte" ---
            yield return null;

            // 5. ANIMACIÓN Y ESPERA FINAL
            animatorArbol.SetTrigger("Crecer");

            yield return new WaitForSeconds(0.1f);

            float duracionAnimacion = animatorArbol.GetCurrentAnimatorStateInfo(0).length;
            yield return new WaitForSeconds(duracionAnimacion + tiempoEsperaExtra);
        }

        // 6. ¡VICTORIA!
        SceneManager.LoadScene(nombreEscenaWin);
    }
}