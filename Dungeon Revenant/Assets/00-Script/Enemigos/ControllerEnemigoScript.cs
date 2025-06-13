using UnityEngine;

public class ControllerEnemigoScript : MonoBehaviour
{
    public int vidaMaxima = 20;
    public int vidaActual;

    private Animator anim;

    public string enemigoID;

    // Variables booleana para recibir, hacer da�o y saber si ataca
    private bool isGolpeado = false;
    private bool estaAtacando = false;

    // Materia
    public GameObject materiaPrefab;

    // ObjetoAleatorio
    public Sprite spriteArma;
    public Sprite spriteCorazon;

    public float probArma = 10f;
    public float probabilidadCorazon = 10f;

    void Start()
    {
        // Comprobar si enemigo ya muri� antes y si es as� desactivarlo
        if (PlayerPrefs.GetInt("EnemigoMuerto_" + enemigoID, 0) == 1)
        {
            gameObject.SetActive(false);
            return;
        }

        vidaActual = vidaMaxima;
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // Comprobaci�n de si esta en la animacion de ataque
        if (anim)
        {
            estaAtacando = anim.GetCurrentAnimatorStateInfo(0).IsTag("Ataque");
        }
    }

    public void TomarDa�o(int da�o)
    {
        if (isGolpeado) return;
        
        isGolpeado=true;

        vidaActual -= da�o;
        Debug.Log(gameObject.name + " recibi� " + da�o + " de da�o. Vida restante: " + vidaActual);

        if (vidaActual <= 0)
        {
            Muerte();
        }
        else
        {
            if (anim)
            {
                anim.Play("Enemy Hit");
            }
            Invoke(nameof(ResetearGolpeado), 0.2f);
        }
    }

    void Muerte()
    {
        Debug.Log(gameObject.name + " ha muerto.");

        // Guardar en PlayerPrefs que este enemigo muri�
        PlayerPrefs.SetInt("EnemigoMuerto_" + enemigoID, 1);
        PlayerPrefs.Save();

        if (anim)
        {
            // Cambio de animacion
            anim.Play("Enemy Death");

            // Generar 1-3 materia y desplazarla un poco para que no se vea visualmente juntas
            int dropCount = Random.Range(1, 4); 
            for (int i = 0; i < dropCount; i++)
            {
                Vector3 dropPos = transform.position + (Vector3)(Random.insideUnitCircle * 0.5f);
                Instantiate(materiaPrefab, dropPos, Quaternion.identity);
            }

            // Probabilidad de dropear objetos
            int dropTipo = Random.Range(1, 3);

            if (dropTipo == 1)
            {
                if (Random.value * 100 <= probArma)
                {
                    CrearDrop(spriteArma, "ArmaSprite");
                }
            }
            else if (dropTipo == 2)
            {
                if (Random.value * 100 <= probabilidadCorazon)
                {
                    CrearDrop(spriteCorazon, "CorazonSprite");
                }
            }

            // Eliminar slime
            Destroy(gameObject, 1.5f);
        }
        else
        {
            int dropCount = Random.Range(2, 4);
            for (int i = 0; i < dropCount; i++)
            {
                Vector3 dropPos = transform.position + (Vector3)(Random.insideUnitCircle * 0.5f);
                Instantiate(materiaPrefab, dropPos, Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }

    void ResetearGolpeado()
    {
        isGolpeado = false;
    }

    // Metodo para detectar si el enemigo colisiona con el jugador
    void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("Trigger detectado con: " + other.name);
        if (other.CompareTag("Player") && estaAtacando)
        {
            PlayerController jugador = other.GetComponentInParent<PlayerController>();
            if (jugador != null)
            {
                Debug.Log("�El slime atac� al jugador!");
                jugador.TomarDa�o();
            }
        }
    }

    void CrearDrop(Sprite sprite, string tipo)
    {
        GameObject drop = new GameObject(tipo);
        SpriteRenderer sr = drop.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;
        sr.sortingOrder = 5;

        drop.transform.position = transform.position;

        // Escalado seg�n el tipo
        if (tipo == "ArmaSprite")
        {
            drop.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
        }
        else if (tipo == "CorazonSprite")
        {
            drop.transform.localScale = new Vector3(1.5f, 1.5f, 1f);
        }

        CircleCollider2D col = drop.AddComponent<CircleCollider2D>();
        col.isTrigger = true;

        drop.tag = tipo;
    }
}