using UnityEngine;
using TMPro;

public class clickManager : MonoBehaviour
{
    public TMP_InputField inputField;
    public AudioSource audioSource;
    public AudioClip keySound;

    private string previousText = "";

    // Cada vez que escribe llama al metodo OnValueChanged
    void Start()
    {
        inputField.onValueChanged.AddListener(OnValueChanged);
    }

    // Metodo para que cuando el usuario escriba algo reproduce el sonido keysound
    void OnValueChanged(string currentText)
    {
        if (currentText.Length > previousText.Length)
        {
            audioSource.PlayOneShot(keySound);
        }
        previousText = currentText;
    }
}