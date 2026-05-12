using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; //Necesario para detectar en qué escena estamos

public class ControlContador : MonoBehaviour
{
    [Header("Configuración")]
    public TextMeshProUGUI textoPantalla;
    public string prefijoTexto = "Kills: ";
    public static int enemigosMuertosGlobal = 0;
    private int killsAlEmpezarNivel;

    void Start()
    {

        if (SceneManager.GetActiveScene().name == "MenuPrincipal")
        {
            ReiniciarContadorGlobal();
        }

        killsAlEmpezarNivel = enemigosMuertosGlobal;

        ActualizarTexto();
        if (textoPantalla != null)
        {
            textoPantalla.transform.localScale = Vector3.one;
        }
    }

    public void SumarBaja()
    {
        enemigosMuertosGlobal++;
        ActualizarTexto();
    }

    private void ActualizarTexto()
    {
        if (textoPantalla != null)
        {
            textoPantalla.text = prefijoTexto + enemigosMuertosGlobal;
        }
    }

    //Estemétodo es para cuando el jugador reinicia y repite el mismo nivel
    public void DeshacerKillsDelNivel()
    {
        enemigosMuertosGlobal = killsAlEmpezarNivel;
        ActualizarTexto();
    }

//Reinicia el contador en cada partida
    public void ReiniciarContadorGlobal()
    {
        enemigosMuertosGlobal = 0;
        killsAlEmpezarNivel = 0;
        ActualizarTexto();
        Debug.Log("Contador reseteado a 0 automáticamente (Nueva Partida).");
    }
}