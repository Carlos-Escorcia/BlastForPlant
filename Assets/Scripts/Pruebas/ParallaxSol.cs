using UnityEngine;

public class ParallaxSol : MonoBehaviour
{
    [Header("Configuración")]
    [Tooltip("La cámara principal de tu juego. Si lo dejas vacío, la buscará sola.")]
    public Transform camara;

    [Tooltip("0 = Se queda quieto en el mundo (como un árbol normal). 1 = Sigue a la cámara perfectamente. Usa un valor como 0.9 para que parezca que está muy lejos.")]
    [Range(0f, 1f)]
    public float efectoParallax = 0.9f;

    [Tooltip("¿Quieres que el sol también suba y baje ligeramente al saltar?")]
    public bool moverEnEjeY = false;

    private Vector2 posicionInicial;
    private Vector2 posicionInicialCamara;

    void Start()
    {
        // Si se te olvida arrastrar la cámara, el script la encuentra automáticamente
        if (camara == null)
        {
            camara = Camera.main.transform;
        }

        // Guardamos las posiciones iniciales
        posicionInicial = transform.position;
        posicionInicialCamara = camara.position;
    }

    void LateUpdate() // Usamos LateUpdate para que se mueva DESPUÉS de que la cámara se haya movido (evita temblores)
    {
        if (camara == null) return;

        // Calculamos cuánto se ha movido la cámara desde el inicio
        float distanciaX = (camara.position.x - posicionInicialCamara.x) * efectoParallax;

        float posY = transform.position.y; // Por defecto, se queda a la misma altura

        // Si quieres que el sol se mueva un poquito hacia arriba/abajo cuando saltas
        if (moverEnEjeY)
        {
            float distanciaY = (camara.position.y - posicionInicialCamara.y) * efectoParallax;
            posY = posicionInicial.y + distanciaY;
        }

        // Aplicamos la nueva posición a nuestro Sol
        transform.position = new Vector3(posicionInicial.x + distanciaX, posY, transform.position.z);
    }
}