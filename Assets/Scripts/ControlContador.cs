using UnityEngine;
using TMPro; // <-- ¡Super importante para usar TextMeshPro!

public class ControlContador : MonoBehaviour
{
    [Header("Configuración")]
    public TextMeshProUGUI textoPantalla; // Aquí arrastraremos el texto

    private int enemigosMuertos = 0; // Nuestro contador interno

    void Start()
    {
        // Nos aseguramos de que empiece en 0 al cargar la escena
        ActualizarTexto();
    }

    // Esta función la llamarán los enemigos cuando mueran
    public void SumarBaja()
    {
        enemigosMuertos++; //Sumamos 1
        ActualizarTexto(); //Actualizamos la pantalla
    }

    private void ActualizarTexto()
    {
        // Cambiamos lo que pone en pantalla
        textoPantalla.text = "Bajas: " + enemigosMuertos;
    }
}