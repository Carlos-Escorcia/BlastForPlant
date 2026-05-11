using UnityEngine;
using TMPro;

public class ControlContador : MonoBehaviour
{
    [Header("Configuración")]
    public TextMeshProUGUI textoPantalla; //Aquí se pone el texto

    [Tooltip("El texto que aparecerá antes del número")]
    public string prefijoTexto = "Kills: ";

    private int enemigosMuertos = 0; //Contador interno

    void Start()
    {
        ActualizarTexto(); //Empieza en 0 al cargar la escena
    }

    //La llaman los enemigos al morir
    public void SumarBaja()
    {
        enemigosMuertos++; //Sumamos 1
        ActualizarTexto(); //Actualizamos la pantalla
    }

    private void ActualizarTexto()
    {
        textoPantalla.text = prefijoTexto + enemigosMuertos; //Aquí se cambia el texto en Unity, en el Inspector
    }
}