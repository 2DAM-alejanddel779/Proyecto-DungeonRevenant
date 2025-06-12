using UnityEngine;
using UnityEngine.UI;

public class BarraVida : MonoBehaviour
{
    public Image rellenoBarraVida;
    private ControllerEnemigoScript enemigoScript;
    private float vidaMaxima;

    void Start()
    {
        enemigoScript = GameObject.Find("mechaBos").GetComponent<ControllerEnemigoScript>();
        vidaMaxima = enemigoScript.vidaMaxima;
    }

    void Update()
    {
        rellenoBarraVida.fillAmount = enemigoScript.vidaActual / vidaMaxima;
    }
}
