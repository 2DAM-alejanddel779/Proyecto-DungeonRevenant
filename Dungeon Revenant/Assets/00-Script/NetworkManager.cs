using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkManager : MonoBehaviour {

    // Corrutina --> Metodo especial de Unity para pausar la ejecucion y continuar sin bloquear el juego

    public static NetworkManager instance;
    public string UsuarioConectado {  get; private set; }
    public int esAdmin;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetUsuarioActual(string usuario)
    {
        UsuarioConectado = usuario;
    }


    // Metodo para crear usuario
    public void CrearUsuario(string usuario, string email, string pass, Action<Response> response)
    {
        StartCoroutine(CO_CrearUsuario(usuario, email, pass, response));
    }

    // Corrutina que envia la solicitud POST al servidor
    private IEnumerator CO_CrearUsuario(string usuario, string email, string pass, Action<Response> response)
    {
        // Creacion del formulario
        WWWForm form = new WWWForm();
        form.AddField("usuario", usuario);
        form.AddField("email", email);
        form.AddField("pass", pass);

        // Crea la solicitud POST al servidor con la URL correcta
        UnityWebRequest request = UnityWebRequest.Post("http://172.22.229.23/Game/crearUsuario.php", form);

        // IP:PUERTO DEL DOCKER DE PROXMOX

        yield return request.SendWebRequest();

        // Verificacion de la solicitud
        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Respuesta del servidor: " + request.downloadHandler.text);
            Response r = JsonUtility.FromJson<Response>(request.downloadHandler.text);
            response(r);
        }
        else
        {
            Debug.LogError("Error al conectarse al servidor: " + request.error);
            response(null);
        }
    }

    // Metodo para chequear si el usuario es correcto
    public void CheckUser(string usuario,string pass, Action<Response> response)
    {
        StartCoroutine(CO_checkUser(usuario, pass, response));
    }

    // Corrutina que envia la solicitud POST al servidor
    private IEnumerator CO_checkUser(string usuario, string pass, Action<Response> response)
    {
        // Creacion del formulario
        WWWForm form = new WWWForm();
        form.AddField("usuario", usuario);
        form.AddField("pass", pass);

        // Crea la solicitud POST al servidor con la URL correcta
        UnityWebRequest request = UnityWebRequest.Post("http://172.22.229.23/Game/checkUser.php", form);

        yield return request.SendWebRequest();

        // Verificacion de la solicitud
        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Respuesta del servidor: " + request.downloadHandler.text);
            Response r = JsonUtility.FromJson<Response>(request.downloadHandler.text);
            response(r);
        }
        else
        {
            Debug.LogError("Error al conectarse al servidor: " + request.error);
            response(null);
        }
    }

    // MEtodo para guardar puntos
    public void GuardarPuntuacion (int puntuacion)
    {
        if (string.IsNullOrEmpty(UsuarioConectado))
        {
            Debug.LogWarning("No hay usuario logeado. No ase puede guardar la puntuacion");
            return;
        }

        StartCoroutine(CO_GuardarPuntuacion(UsuarioConectado, puntuacion));
    }

    private IEnumerator CO_GuardarPuntuacion(string usuario, int puntuacion)
    {
        WWWForm form = new WWWForm();
        form.AddField("usuario", usuario);
        form.AddField("puntuacion", puntuacion);

        UnityWebRequest request = UnityWebRequest.Post("http://172.22.229.23/Game/guardarPuntuacion.php", form);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Puntuación guardada correctamente: " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Error al guardar puntuación: " + request.error);
        }
    }
}

// Clase para guardar la respues del servidor
[Serializable]
public class Response
{
    public bool valido = false;
    public string mensaje = "";
    public int esAdmin = 0;
}
