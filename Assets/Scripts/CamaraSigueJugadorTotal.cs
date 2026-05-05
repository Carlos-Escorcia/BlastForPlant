using UnityEngine;

public class CamaraSigueJugadorTotal : MonoBehaviour
{
    [Header("Objetivo")]
    public Transform jugador;

    [Header("Configuración de Seguimiento")]
    [Tooltip("X=0, Y=0 para centrar. Z=-10 es OBLIGATORIO.")]
    public Vector3 offset = new Vector3(0, 0, -10f);

    [Range(0, 1f)]
    [Tooltip("0 = Pegada como una lapa (instantánea). 0.1 = Un pelín de retraso suave.")]
    public float suavizado = 0f;

    [Header("Ejes")]
    public bool seguirEnX = true;
    public bool seguirEnY = true;

    private Vector3 velocidadActual = Vector3.zero;

    void LateUpdate()
    {
        if (jugador == null) return;

        // Calculamos la posición destino basada únicamente en el jugador
        Vector3 puntoDestino = transform.position;

        if (seguirEnX) puntoDestino.x = jugador.position.x + offset.x;
        if (seguirEnY) puntoDestino.y = jugador.position.y + offset.y;
        puntoDestino.z = offset.z;

        // Si el suavizado es 0, la cámara se teletransporta a la posición del jugador cada frame
        if (suavizado <= 0)
        {
            transform.position = puntoDestino;
        }
        else
        {
            // Si quieres un poco de 'fineza', usamos SmoothDamp
            transform.position = Vector3.SmoothDamp(transform.position, puntoDestino, ref velocidadActual, suavizado);
        }
    }
}