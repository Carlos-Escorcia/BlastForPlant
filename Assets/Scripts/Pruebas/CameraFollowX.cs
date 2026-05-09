using UnityEngine;

public class CameraFollowX : MonoBehaviour
{
    [Header("Configuración de Seguimiento")]
    [Tooltip("Arrastra aquí a tu jugador desde la Jerarquía")]
    [SerializeField] private Transform target; // El objetivo a seguir (tu personaje)

    [Tooltip("Distancia horizontal que la cámara mantendrá respecto al jugador")]
    [SerializeField] private float offsetX = 5f;

    [Tooltip("La altura fija a la que se quedará la cámara siempre")]
    [SerializeField] private float fixedYPosition = 0f;

    // Usamos LateUpdate para que la cámara se mueva después del Update del jugador
    void LateUpdate()
    {
        // Si no hemos asignado un objetivo, no hacemos nada para evitar errores
        if (target == null) return;

        // Creamos una nueva posición: 
        // - En X: la posición del jugador más nuestro desfase (offset).
        // - En Y: nuestra altura bloqueada.
        // - En Z: mantenemos la profundidad original de la cámara (suele ser -10).
        Vector3 newPosition = new Vector3(target.position.x + offsetX, fixedYPosition, transform.position.z);

        // Aplicamos la nueva posición a la cámara
        transform.position = newPosition;
    }
}