using UnityEngine;

public class Singleton : MonoBehaviour
{
    public static Singleton instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // Destruye al duplicado si ya hay uno
        }
    }
}
