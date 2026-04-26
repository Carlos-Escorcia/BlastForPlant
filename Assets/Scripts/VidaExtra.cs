using UnityEngine;

public class VidaExtra : MonoBehaviour
{
    [Header("Animación de Flote")]
    public float velocidadFlote = 3f;  // Lo rápido que sube y baja
    public float alturaFlote = 0.2f;   // Los centímetros que se desplaza arriba y abajo

    [Header("Sonido")]
    public AudioClip sonidoRecoger;    // Arrastra aquí tu MP3
    private Vector3 posicionInicial;

    void Start()
    {
        // Guardamos la posición original donde pusiste el corazón para flotar en base a ella
        posicionInicial = transform.position;
    }

    void Update()
    {
        // Hacemos que flote suavemente usando una curva matemática infinita (Seno)
        float nuevaY = posicionInicial.y + Mathf.Sin(Time.time * velocidadFlote) * alturaFlote;
        transform.position = new Vector3(transform.position.x, nuevaY, transform.position.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si lo que nos toca tiene la etiqueta "Player"...
        if (collision.CompareTag("Player"))
        {
            // Buscamos el script del personaje en ese objeto
            ControlPersonaje personaje = collision.GetComponent<ControlPersonaje>();

            if (personaje != null)
            {
                // 1. PRIMERO COMPROBAMOS SI CURA O NO
                if (personaje.vidas < personaje.vidasMaximas)
                {
                    // Solo te cura si no estás al máximo
                    personaje.GanarVida();
                }
                if (sonidoRecoger != null)
                {
                    AudioSource.PlayClipAtPoint(sonidoRecoger, transform.position);
                }

                Destroy(gameObject); //Destrucción objeto
            }
        }
    }
}  