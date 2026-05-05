using UnityEngine;

public class CamaraSigueJugadorTotal : MonoBehaviour
{
    [Header("Configuración")]
    public Transform objetivo;
    public float suavizado = 0.125f;
    public Vector3 offset;

    [Header("Restricciones")]
    [Tooltip("Si está activo, la cámara no subirá ni bajará con el jugador.")]
    public bool bloquearY = true;
    [Tooltip("La altura fija que tendrá la cámara si bloquearY está activo.")]
    public float yFija = 0f;

    [Header("Estado")]
    public bool puedeSeguir = true;

    void FixedUpdate()
    {
        if (objetivo == null || !puedeSeguir) return;

        Vector3 posicionDeseada = objetivo.position + offset;

        if (bloquearY)
        {
            posicionDeseada.y = yFija;
        }

        Vector3 posicionSuavizada = Vector3.Lerp(transform.position, posicionDeseada, suavizado);
        transform.position = posicionSuavizada;
    }
}