using UnityEngine;

public class Disparo : MonoBehaviour
{
    public float velocidad = 7f;
    private Vector2 direccion;

    // Este método se llama justo después de instanciar el proyectil para definir su dirección
    public void Inicializar(Vector2 direccionInicial)
    {
        direccion = direccionInicial.normalized;
    }

    void Update()
    {
        transform.Translate(direccion * velocidad * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Restar vida al jugador
            PlayerController jugador = collision.GetComponent<PlayerController>();
            if (jugador != null)
            {
                jugador.TomarDaño();
            }

            Destroy(gameObject);
        }
        else if (collision.CompareTag("Pared"))
        {
            Destroy(gameObject);
        }
    }
}
