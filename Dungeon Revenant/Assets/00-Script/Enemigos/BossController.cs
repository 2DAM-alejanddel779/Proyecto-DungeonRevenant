using UnityEngine;

public class BossController : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidadMovimiento = 1f;
    public float cambioDireccion = 2f;
    private Vector2 movementDirection;
    private float tiempoDireccion;

    [Header("Disparo")]
    public GameObject proyectilPrefab;
    public Transform puntoDisparo;
    public float tiempoEntreDisparos = 7f;
    private float tiempoDisparo;

    void Start()
    {
        ElegirNuevaDireccion();
        tiempoDisparo = tiempoEntreDisparos;
    }

    void Update()
    {
        // Movimiento aleatorio
        tiempoDireccion -= Time.deltaTime;
        if (tiempoDireccion <= 0f)
        {
            ElegirNuevaDireccion();
        }

        transform.Translate(movementDirection * velocidadMovimiento * Time.deltaTime);

        // Disparo
        tiempoDisparo -= Time.deltaTime;
        if (tiempoDisparo <= 0f)
        {
            Disparar();
            tiempoDisparo = tiempoEntreDisparos + Random.Range(0f, 1f);
        }
    }

    void ElegirNuevaDireccion()
    {
        movementDirection = Random.insideUnitCircle.normalized;
        tiempoDireccion = cambioDireccion;
    }

    public void Disparar()
    {
        if (proyectilPrefab != null && puntoDisparo != null)
        {
            GameObject disparo = Instantiate(proyectilPrefab, puntoDisparo.position, Quaternion.identity);

            Transform jugador = GameObject.FindGameObjectWithTag("Player")?.transform;
            if (jugador != null)
            {
                Vector2 direccionDisparo = ((Vector2)jugador.position - (Vector2)puntoDisparo.position).normalized;
                Disparo disparoScript = disparo.GetComponent<Disparo>();
                if (disparoScript != null)
                {
                    disparoScript.Inicializar(direccionDisparo);
                }
            }
            else
            {
                // Si no hay jugador, dispara hacia abajo para probar
                Disparo disparoScript = disparo.GetComponent<Disparo>();
                if (disparoScript != null)
                {
                    disparoScript.Inicializar(Vector2.down);
                }
            }
        }
    }
}