using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UsuarioUI : MonoBehaviour
{
    public TMP_Text nombreUsuarioText;
    public Button botonEliminar;

    private string nombreUsuario;

    public void Configurar(string nombre, System.Action<string> onEliminar)
    {
        nombreUsuario = nombre;
        nombreUsuarioText.text = nombre;

        botonEliminar.onClick.AddListener(() => {onEliminar?.Invoke(nombreUsuario);});
    }
}

