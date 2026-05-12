using UnityEngine;
using TMPro;

public class ControlContador : MonoBehaviour
{
    [Header("Configuración")]
    public TextMeshProUGUI textoPantalla;
    public string prefijoTexto = "Kills: ";

    public static int enemigosMuertosGlobal = 0; //Esto gyuarda las kills enytre niveles

    void Start()
    {
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
}