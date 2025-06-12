using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

public class PanelUsuariosManager : MonoBehaviour
{
    public GameObject itemPrefab;
    public Transform contenido;
    public GameObject panel;

    [System.Serializable]
    public class Usuario
    {
        public int id;
        public string username;
    }

    public void AbrirPanel()
    {
        panel.SetActive(true);
        StartCoroutine(CargarUsuarios());
    }

    public void CerrarPanel()
    {
        panel.SetActive(false);
    }

    IEnumerator CargarUsuarios()
    {
        UnityWebRequest www = UnityWebRequest.Get("http://172.22.229.23/Game/obtenerUsuarios.php");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error cargando usuarios: " + www.error);
            yield break;
        }

        Debug.Log("Respuesta del servidor: " + www.downloadHandler.text);

        foreach (Transform hijo in contenido)
            Destroy(hijo.gameObject);

        List<Usuario> lista = JsonConvert.DeserializeObject<List<Usuario>>(www.downloadHandler.text);

        foreach (Usuario u in lista)
        {
            Debug.Log("Instanciando usuario: " + u.username);
            GameObject item = Instantiate(itemPrefab);
            item.transform.SetParent(contenido, false);

            // Asignar texto
            item.GetComponentInChildren<TMP_Text>().text = u.username;

            // Asignar botón con id seguro
            Button btn = item.GetComponentInChildren<Button>();
            int idUsuario = u.id;
            btn.onClick.AddListener(() => EliminarUsuario(idUsuario));
        }
    }

    void EliminarUsuario(int id)
    {
        StartCoroutine(EliminarUsuarioCO(id));
    }

    IEnumerator EliminarUsuarioCO(int id)
    {
        WWWForm form = new WWWForm();
        form.AddField("id", id);
        UnityWebRequest www = UnityWebRequest.Post("http://172.22.229.23/Game/eliminarUsuarios.php", form);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Usuario eliminado");
            StartCoroutine(CargarUsuarios()); // refrescar
        }
        else
        {
            Debug.LogError("Error eliminando usuario: " + www.error);
        }
    }
}