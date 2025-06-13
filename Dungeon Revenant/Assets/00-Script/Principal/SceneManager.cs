using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using Newtonsoft.Json;
using TMPro;
using ControladorGame;

public class SceneManager : MonoBehaviour
{
    [Header("Login")]
    [SerializeField] private TMP_InputField usuarioLogin = null;
    [SerializeField] private TMP_InputField passwordLogin = null;

    [Header("Registro")]
    [SerializeField] private TMP_InputField usuarioInput = null;
    [SerializeField] private TMP_InputField emailInput = null;
    [SerializeField] private TMP_InputField passwordInput = null;
    [SerializeField] private TMP_InputField rePasswordInput = null;

    [Header("Mensajes emergentes")]
    [SerializeField] private TMP_Text textoMostrar = null;

    [Header("UI")]
    [SerializeField] private GameObject registerUI = null;
    [SerializeField] private GameObject loginUI = null;
    [SerializeField] private GameObject menuPrincipalUI = null;

    [Header("UI ADMIN")]
    [SerializeField] private GameObject botonAdmin;
    [SerializeField] private GameObject panelUsuarios;


    [Header("Ranking")]
    [SerializeField] private GameObject panelRanking;
    [SerializeField] private Transform contenidoRanking;
    [SerializeField] private GameObject rankingItemPrefab;
    [SerializeField] private TMP_Text textoRanking = null;

    private NetworkManager networkManager;

    private void Awake()
    {
        networkManager = GameObject.FindObjectOfType<NetworkManager>();
    }

    void Start()
    {
        // Comprobar si el GameManager quiere mostrar menú principal tras GameOver
        var gameManager = FindObjectOfType<ControladorGame.GameManager>();
        if (gameManager != null && gameManager.volverAlMenu)
        {
            Debug.Log("Volviendo del GameOver: mostrando menú principal");

            gameManager.ResetGame();

            if (loginUI != null) loginUI.SetActive(false);
            if (registerUI != null) registerUI.SetActive(false);
            if (menuPrincipalUI != null) menuPrincipalUI.SetActive(true);

            gameManager.volverAlMenu = false;
        }
        else
        {
            if (loginUI != null) loginUI.SetActive(true);
            if (registerUI != null) registerUI.SetActive(false);
            if (menuPrincipalUI != null) menuPrincipalUI.SetActive(false);
        }
    }

    //----------------------------------------
    //      LOGIN, REGISTRO Y JUGAR
    //----------------------------------------

    public void SubmitLogin()
    {
        // Control de errores por si no ha rellenado toda la información.
        if (string.IsNullOrEmpty(usuarioLogin.text) || string.IsNullOrEmpty(passwordLogin.text))
        {
            textoMostrar.text = "Todos los campos deben de encontrarse rellenos.";
            return;
        }

        networkManager.CheckUser(usuarioLogin.text, passwordLogin.text, delegate (Response response)
        {
            if (response != null)
            {
                textoMostrar.text = response.mensaje;

                // Cambiar la ventana a Menu principal si coincide.
                if (response.mensaje.StartsWith("Usuario:"))
                {
                    // Guardamos el usuario.
                    networkManager.SetUsuarioActual(usuarioLogin.text);

                    loginUI.SetActive(false);
                    menuPrincipalUI.SetActive(true);

                    // Comprobar si es un administrador
                    if (botonAdmin != null)
                    {
                        botonAdmin.SetActive(response.esAdmin == 1);
                    }
                }
            }
            else
            {
                textoMostrar.text = "Error al conectar con el servidor. Inténtalo más tarde.";
                Debug.LogError("La respuesta del servidor fue nula.");
            }
        });
    }

    public void SubmitRegister()
    {
        // Control de errores por si no ha rellenado toda la información
        if (string.IsNullOrEmpty(usuarioInput.text) || string.IsNullOrEmpty(emailInput.text) || string.IsNullOrEmpty(passwordInput.text) || string.IsNullOrEmpty(rePasswordInput.text))
        {
            textoMostrar.text = "Todos los campos deben de encontrarse rellenos.";
        }
        // Control para saber si ha escrito las mismas contraseñas.
        if (passwordInput.text.Equals(rePasswordInput.text))
        {
            textoMostrar.text = "Procesando...";
            networkManager.CrearUsuario(usuarioInput.text, emailInput.text, passwordInput.text, delegate (Response response)
            {
                if (response != null)
                {
                    textoMostrar.text = response.mensaje;
                }
                else
                {
                    textoMostrar.text = "Error al conectar con el servidor. Inténtalo más tarde.";
                    Debug.LogError("La respuesta del servidor fue nula.");
                }
            });
        }
        else
        {
            textoMostrar.text = "Las contraseñas no son iguales.";
        }
    }

    public void ShowLogin()
    {
        // Activamos y desactivamos pantallas
        registerUI.SetActive(false);
        loginUI.SetActive(true);
    }

    public void ShowRegister()
    {
        // Activamos y desactivamos pantallas
        registerUI.SetActive(true);
        loginUI.SetActive(false);
    }

    public void Jugar()
    {
        // Cargar la escena del primer mapa
        UnityEngine.SceneManagement.SceneManager.LoadScene("Mapa01");
    }

    //----------------------------------------
    //      RANKING
    //----------------------------------------

    public void MostrarRanking()
    {
        Debug.Log("Intentando mostrar el panel de ranking...");

        panelRanking.SetActive(true);
        StartCoroutine(CargarRankingDesdePHP());
    }

    private IEnumerator CargarRankingDesdePHP()
    {
        UnityWebRequest www = UnityWebRequest.Get("http://172.22.229.23/Game/obtenerRanking.php");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            textoMostrar.text = "Error al cargar el ranking.";
       //     Debug.LogError(www.error);
            yield break;
        }
        Debug.Log("Respuesta JSON del servidor: " + www.downloadHandler.text);
        // Limpiar contenido anterior
        foreach (Transform hijo in contenidoRanking)
            Destroy(hijo.gameObject);

        // Convertimos la respuesta JSON a una lista básica
        RankingSimple[] ranking = JsonConvert.DeserializeObject<RankingSimple[]>(www.downloadHandler.text);
        Debug.Log("Actualizando textoRanking con: " + textoRanking.name);

        // Comprobar longitud del array
        if (ranking.Length == 0)
        {
            textoRanking.text = "No hay datos en el ranking aún.";
        }
        else
        {
            textoRanking.text = "";
            int puesto = 1;
            foreach (RankingSimple fila in ranking)
            {
                GameObject rankingItem = Instantiate(rankingItemPrefab, contenidoRanking);
                TMP_Text texto = rankingItem.GetComponentInChildren<TMP_Text>();
                texto.text = puesto + ". " + fila.usuario + ": " + fila.puntuacion + " pts";
                puesto++;
            }
        }
    }

    public void CerrarRanking()
    {
        panelRanking.SetActive(false);
    }

    //----------------------------------------
    //      LISTAR USUARIOS
    //----------------------------------------

    public void AbrirPanelUsuarios()
    {
        panelUsuarios.SetActive(true);
        var panelScript = panelUsuarios.GetComponent<PanelUsuariosManager>();
        if (panelScript != null)
        {
            panelScript.AbrirPanel();
        }
    }

    public void CerrarUsuarios()
    {
        panelUsuarios.SetActive(false);
    }

    //----------------------------------------
    //      Cerrar sesion Y CERRAR JUEGO
    //----------------------------------------

    public void CerrarSesion()
    {
       // Debug.Log("Cerrando sesión...");

        // Limpiar los campos de login
        usuarioLogin.text = "";
        passwordLogin.text = "";

        // Cambiar a la ventana de login
        menuPrincipalUI.SetActive(false);
        loginUI.SetActive(true);
    }

    public void SalirDelJuego()
    {
     //   Debug.Log("Saliendo del juego...");
        Application.Quit();

    }
}

[System.Serializable]
public class RankingSimple
{
    public string usuario;
    public int puntuacion;
}
