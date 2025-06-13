using UnityEngine;
using UnityEngine.SceneManagement;
using ControladorGame;

public class Teletransporte : MonoBehaviour
{
    public string nombreDestino;       
    public string destinoSpawnID;      

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.GuardarDatosEnGameManager();
                GameManager.instance.SetSpawnID(destinoSpawnID);

                // Cargar la escena destino usando el namespace completo
                UnityEngine.SceneManagement.SceneManager.LoadScene(nombreDestino);
            }
            else
            {
                Debug.LogError("El objeto Player no tiene el script PlayerController");
            }
        }
    }
}
