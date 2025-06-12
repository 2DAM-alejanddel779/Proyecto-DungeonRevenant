using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    public void OnVolverMenuPressed()
    {
        Time.timeScale = 1f;

        var gameManager = ControladorGame.GameManager.instance;
        if (gameManager != null)
        {
            gameManager.ResetGame();
            UnityEngine.SceneManagement.SceneManager.LoadScene("Principal");
        }
        else
        {
            Debug.LogWarning("No se encontró GameManager para volver al menú.");
        }
    }
}