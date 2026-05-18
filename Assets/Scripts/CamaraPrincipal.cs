using UnityEngine;

public class CamaraPrincipal : MonoBehaviour
{
    [Header("Configuración")]
    public Transform objetivo;
    public float suavizado = 0.125f; //La suavidad del seguimiento
    public Vector3 offset; //Distancia de la base
    [Header("Estado")]
    public bool puedeSeguir = true; //Controla si la cámara debe moverse o no

    void FixedUpdate()
    {
        //Si no hay objetivo asignado, no se hace nada
        if (objetivo == null || !puedeSeguir) return;

        //Se calcula la posición exacta a la que queremos que vaya la cámara
        Vector3 posicionDeseada = objetivo.position + offset;

        //Lerp suaviza el salto matemático entre la posición actual y la deseada
        Vector3 posicionSuavizada = Vector3.Lerp(transform.position, posicionDeseada, suavizado);

        //Se le da la nueva posición a la camara
        transform.position = posicionSuavizada;
    }
}
