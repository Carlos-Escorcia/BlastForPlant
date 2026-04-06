using UnityEngine;


[RequireComponent(typeof(SpriteRenderer))]
public class EfectoParalax : MonoBehaviour

{

    private float longitudSprite;
    private float posicionInicialX;

    [Header("Configuración")]

    public Transform camaraTransform; // Arrastra la Main Camera aquí

    [Tooltip("0 = Se mueve con la cámara (cielo). 1 = Estático (primer plano). 0.5 = Mitad de velocidad.")]

    public float efectoParallax;


    void Start()

    {

        posicionInicialX = transform.position.x;//Obtenemos el ancho exacto del sprite para saber cuándo repetirlo
        longitudSprite = GetComponent<SpriteRenderer>().bounds.size.x;

    }


    void Update()

    {
        float temp = (camaraTransform.position.x * (1 - efectoParallax));//'temp' es cuánto ha avanzado la cámara
        float distancia = (camaraTransform.position.x * efectoParallax);//'distancia' es hacia dónde debemos mover el fondo

        transform.position = new Vector3(posicionInicialX + distancia, transform.position.y, transform.position.z);//movim iento del fondo


        // La magia infinita: Si avanzamos más allá del sprite, reposicionamos el inicio

        if (temp > posicionInicialX + longitudSprite)

        {

            posicionInicialX += longitudSprite;

        }

        else if (temp < posicionInicialX - longitudSprite)

        {

            posicionInicialX -= longitudSprite;

        }

    }

}
