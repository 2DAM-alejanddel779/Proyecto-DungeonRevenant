using UnityEngine;
using System.Collections;

public class DisparoBurbuja : MonoBehaviour
{
    [SerializeField] private float speed;
    private Transform player;
    private Rigidbody2D rb;

    void Start()
    {
        player = FindObjectOfType<PlayerController>().transform;
        rb = GetComponent<Rigidbody2D>();
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        LanzarProyectil();
    }

    private void LanzarProyectil()
    {
        Vector2 direccionJugador = (player.position - transform.position).normalized;
        rb.linearVelocity = direccionJugador * speed;

        StartCoroutine(DestruirProyectil());
    }

    IEnumerator DestruirProyectil()
    {
        float tiempoDestruccion = 5f;
        yield return new WaitForSeconds(tiempoDestruccion);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController jugador = other.GetComponentInParent<PlayerController>();
            if (jugador != null)
            {
                jugador.TomarDaño(); 
            }

            Destroy(gameObject); 
        }
    }
}
