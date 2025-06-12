using UnityEngine;

public class Disparo : MonoBehaviour
{
    public float velocidad = 7f;
    private Vector2 direccion;

    // Este m�todo se llama justo despu�s de instanciar el proyectil para definir su direcci�n
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
                jugador.TomarDa�o();
            }

            Destroy(gameObject);
        }
        else if (collision.CompareTag("Pared"))
        {
            Destroy(gameObject);
        }
    }
}
