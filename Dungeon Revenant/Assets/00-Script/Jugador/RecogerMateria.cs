using UnityEngine;

public class RecogerMateria : MonoBehaviour
{
    private int cantidadMateria;

    public Sprite spriteAzul;
    public Sprite spriteRojo;

    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        // Número aleatorio entre 10 y 80
        cantidadMateria = Random.Range(10, 61);
        
        // Cambiar sprite segun la cantidad
        if (cantidadMateria >= 10 && cantidadMateria <= 40)
        {
            sr.sprite = spriteAzul;
        } else if (cantidadMateria > 40)
        {
            sr.sprite = spriteRojo;
        }
    }

    // Al tocar el jugador la materia, añadir la puntuación.
    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController jugador = other.GetComponentInParent<PlayerController>();
        if (jugador != null)
        {
            jugador.AñadirMateria(cantidadMateria);
            Debug.Log("Jugador recogió materia: " + cantidadMateria);
            Destroy(gameObject);
        }
    }
}