using UnityEngine;

public class CameraFollowBounded : MonoBehaviour
{
    [Header("Objetivo")]
    [Tooltip("Arrastra aquí a tu jugador")]
    [SerializeField] private Transform target;

    [Header("Configuración de Movimiento")]
    [Tooltip("Distancia a la que se mantiene la cámara (X, Y, y Z debe ser -10)")]
    [SerializeField] private Vector3 offset = new Vector3(3f, 1f, -10f);

    [Tooltip("Velocidad a la que la cámara persigue al jugador. Cuanto más bajo, más elástico.")]
    [SerializeField] private float smoothSpeed = 5f;

    [Header("Límites Verticales de la Cámara")]
    [Tooltip("La cámara NUNCA bajará de este valor Y")]
    [SerializeField] private float minY = 0f;
    [Tooltip("La cámara NUNCA subirá de este valor Y")]
    [SerializeField] private float maxY = 15f;

    void LateUpdate()
    {
        // Prevención de errores por si olvidamos asignar el jugador
        if (target == null) return;

        // 1. Calculamos dónde QUEREMOS que esté la cámara (Posición del jugador + desfase)
        Vector3 desiredPosition = target.position + offset;

        // 2. Aplicamos la regla del límite vertical. 
        // Clamp obliga a que la coordenada 'y' no baje de minY ni pase de maxY.
        desiredPosition.y = Mathf.Clamp(desiredPosition.y, minY, maxY);

        // 3. Suavizamos el viaje desde la posición actual hasta la deseada (Lerp)
        // Lerp calcula un punto intermedio, dándonos ese efecto de cámara fluida y profesional.
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // 4. Movemos la cámara
        transform.position = smoothedPosition;
    }
}