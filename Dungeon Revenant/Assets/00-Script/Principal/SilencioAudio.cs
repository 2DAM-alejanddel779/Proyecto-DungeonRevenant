using UnityEngine;
using UnityEngine.UI;

public class SilencioAudio : MonoBehaviour
{
    public Sprite sonidoSprite;
    public Sprite muteSprite;
    public Button muteButton;
    public AudioSource audioSource;

    private bool isMuted = false;

    void Start()
    {
        // Asegura que el botón tenga el sprite correcto al inicio
        UpdateIcon();
        muteButton.onClick.AddListener(ToggleMute);
    }

    void ToggleMute()
    {
        isMuted = !isMuted;
        audioSource.mute = isMuted;
        UpdateIcon();
    }

    void UpdateIcon()
    {
        if (isMuted)
        {
            muteButton.image.sprite = muteSprite;
        }
        else
        {
            muteButton.image.sprite = sonidoSprite;
        }
    }

}
