using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemigoController : MonoBehaviour
{
    public Transform objetivo; 
    public float velocidad = 2f;
    public float rangoDeteccion = 5f;
    public float distanciaAtaque = 1f;

    private Rigidbody2D rb;
    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        AsignarObjetivo();
    }

    void FixedUpdate()
    {
        if (objetivo == null) return;

        float distancia = Vector2.Distance(transform.position, objetivo.position);

        Vector2 direccion = (objetivo.position - transform.position).normalized;

        if (distancia < rangoDeteccion && distancia > distanciaAtaque)
        {
            // Movimiento hacia el objetivo
            rb.MovePosition(rb.position + direccion * velocidad * Time.fixedDeltaTime);

            if (anim)
            {
                anim.SetBool("Run", true);
                anim.SetBool("Attack", false);
            }

            // Flip horizontal
            if (direccion.x != 0)
            {
                Vector3 escala = transform.localScale;
                escala.x = Mathf.Sign(direccion.x) * Mathf.Abs(escala.x);
                transform.localScale = escala;
            }
            // Reproducir animación de correr
            if (anim && !anim.GetCurrentAnimatorStateInfo(0).IsName("Enemy Run"))
            {
                anim.Play("Enemy Run");
            }
        }
        else if (distancia <= distanciaAtaque)
        {
            // Reproducir animación de ataque
            if (anim && !anim.GetCurrentAnimatorStateInfo(0).IsName("Enemy Attack 1"))
            {
                anim.Play("Enemy Attack 1");
            }
        }
        else
        {
            // Reproducir animación de idle
            if (anim && !anim.GetCurrentAnimatorStateInfo(0).IsName("Enemy Idle"))
            {
                anim.Play("Enemy Idle");
            }
        }
    }

    // Metodo especial utlizado para cuando se cambie de escena decirle a los enemigos que el objetivo es el jugador
    void OnEnable()
    {
        AsignarObjetivo();
    }

    // Metodo para asignar el jugador como objetivo a los enemigos
    void AsignarObjetivo()
    {
        if (objetivo == null)
        {
            GameObject jugador = GameObject.FindGameObjectWithTag("Player");
            if (jugador != null)
            {
                objetivo = jugador.transform;
            }
        }
    }
}