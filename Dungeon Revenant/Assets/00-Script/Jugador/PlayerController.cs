using UnityEngine;
using Spine.Unity;
using UnityEngine.SceneManagement;
using TMPro;
using ControladorGame;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SkeletonAnimation))]
public class PlayerController : MonoBehaviour
{
    [Header("Vida")]
    public int vidasMaximas = 5;
    public int vidasActuales;
    public TextMeshProUGUI vidaText;
    private string iconoCorazon = "❤";

    private bool puedeRecibirDaño = true;
    public GameObject gameOverUI;

    [Header("Puntuacion")]
    public int materia = 0;
    public TextMeshProUGUI materiaText;

    [Header("Movement")]
    public float velMovimiento = 5f;
    private string siguienteMovimiento;

    private Vector2 movimiento;
    private Rigidbody2D rb;
    private SkeletonAnimation personajeAnimacion;

    [Header("Ataque")]
    public GameObject hitboxEspada;
    private bool isAttack = false;

    public int armasRecogidas = 0;

    private string animacionActual = "";

    // No destruir el jugador al cambiar a una nueva escena
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        personajeAnimacion = GetComponent<SkeletonAnimation>();

        // Asignar los tectos de vida y materia en caso de que no esten ya definidos
        if (vidaText == null)
            vidaText = GameObject.Find("VidaText").GetComponent<TextMeshProUGUI>();

        if (materiaText == null)
            materiaText = GameObject.Find("MateriaText").GetComponent<TextMeshProUGUI>();

        // Inicializar los textos y actualizar visualmente las vidas
        materiaText.text = materia.ToString();
        vidasActuales = vidasMaximas;
        ActualizarHUDVida();

        // Controlar que la hitbox solo se active cuando golpea
        if (hitboxEspada != null)
            hitboxEspada.SetActive(false);

        // Buscar la UI del game over al perder
        if (gameOverUI == null)
        {
            GameObject uiGameOver = GameObject.Find("GameOverUI");
            if (uiGameOver != null)
            {
                gameOverUI = uiGameOver;
            }
            else
            {
                Debug.LogWarning("No se encontró GameOverUI en la escena.");
            }
        }
    }

    //----------------------------------------
    //    MOVIMIENTO Y SPRITE
    //----------------------------------------

    void Update()
    {
        // Controlar movimientos horizontales y verticales del jugador
        movimiento.x = Input.GetAxisRaw("Horizontal");
        movimiento.y = Input.GetAxisRaw("Vertical");

        // Controlar si el jugador ataca y la animacion
        if (Input.GetKeyDown(KeyCode.Space) && !isAttack)
        {
            isAttack = true;
            personajeAnimacion.AnimationState.SetAnimation(0, "Attack", false);

            if (hitboxEspada != null)
                hitboxEspada.SetActive(true);

            // Comprobacion para si se pulsa alguna tecla para realizar la animacin Move o Idle
            if (movimiento != Vector2.zero)
            {
                siguienteMovimiento = "Move";
            }
            else
            {
                siguienteMovimiento = "Idle";
            }

            personajeAnimacion.AnimationState.AddAnimation(0, siguienteMovimiento, true, 0);

            Invoke(nameof(FinalizarAtaque), 0.4f);
        }

        // Compruba si el jugador se encuentra atacando 
        if (!isAttack)
        {
            if (movimiento != Vector2.zero)
            {
                SetAnimation("Move", true);
                CambiarSprite(movimiento.x);
            }
            else
            {
                SetAnimation("Idle", true);
            }
        }
    }

    // Metodo para mover el personaje
    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movimiento.normalized * velMovimiento * Time.fixedDeltaTime);
    }

    // Metodo para controlar animaciones
    void SetAnimation(string nombreAnimacion, bool repetir)
    {
        // Comprobacion de si la animacion actual es diferente a la que se quire poner
        if (animacionActual != nombreAnimacion)
        {
            // Cambia la animacion y guarda el nombre
            personajeAnimacion.AnimationState.SetAnimation(0, nombreAnimacion, repetir);
            animacionActual = nombreAnimacion;
        }
    }

    // Metodo para controlar el sprite del jugador dependiendo del movimiento
    void CambiarSprite(float direccionMovimiento)
    {
        if (direccionMovimiento > 0)
        {
            // Si el jugador mueve hacia la derecha, tambien movemos la hitbox de la espada
            personajeAnimacion.Skeleton.ScaleX = 1;
            if (hitboxEspada != null)
                hitboxEspada.transform.localPosition = new Vector3(0.5f, hitboxEspada.transform.localPosition.y, hitboxEspada.transform.localPosition.z);
            hitboxEspada.transform.localScale = new Vector3(1, 1, 1);
        }
        else if (direccionMovimiento < 0)
        {
            // Si el jugador meuve a la izquierda, tambien se mueve hitbox de la espada
            personajeAnimacion.Skeleton.ScaleX = -1;
            if (hitboxEspada != null)
                hitboxEspada.transform.localPosition = new Vector3(-0.5f, hitboxEspada.transform.localPosition.y, hitboxEspada.transform.localPosition.z);
            hitboxEspada.transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void CambiarSkin(string nombreSkin)
    {
        personajeAnimacion.Skeleton.SetSkin(nombreSkin);
        personajeAnimacion.Skeleton.SetSlotsToSetupPose();
        personajeAnimacion.AnimationState.Apply(personajeAnimacion.Skeleton);

        Debug.Log("Skin cambiado a: " + nombreSkin);
    }


    //----------------------------------------
    //    ATAQUE
    //----------------------------------------

    private void FinalizarAtaque()
    {
        isAttack = false;
        if (hitboxEspada != null)
            hitboxEspada.SetActive(false);
    }

    //----------------------------------------
    //    VIDA, DAÑO Y RECOGIDA
    //----------------------------------------

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "CorazonSprite")
        {
            vidasMaximas += 1;
            vidasActuales += 1;
            ActualizarHUDVida();
            Debug.Log("¡Has recogido un corazón! Vida máxima ahora es: " + vidasMaximas);
            Destroy(other.gameObject);
        }
        else if (other.gameObject.name.Contains("ArmaSprite"))
        {
            armasRecogidas++;
            Destroy(other.gameObject);

            Debug.Log("Arma recogida: " + armasRecogidas);

            if (armasRecogidas >= 3)
            {
                CambiarSkin("p3");
            }
        }
    }

    // Metodo para cuando el jugador recibe daño
    public void TomarDaño()
    {
        if (!puedeRecibirDaño) return;

        vidasActuales--;
        ActualizarHUDVida();

        puedeRecibirDaño = false;
        Invoke(nameof(ResetearPuedeRecibirDaño), 1f);

        if (vidasActuales <= 0)
        {
            GameOver();
        }
    }

    // Vuelve a permitir que el jugador reciba daño
    private void ResetearPuedeRecibirDaño()
    {
        puedeRecibirDaño = true;
    }

    private void ActualizarHUDVida()
    {
        if (vidaText == null)
        {
            Debug.LogError("vidaText es null en PlayerController");
            return;
        }

        vidaText.text = "";
        for (int i = 0; i < vidasActuales; i++)
        {
            vidaText.text += iconoCorazon + " ";
        }
    }

    //----------------------------------------
    //    PUNTUACION
    //----------------------------------------

    public void AñadirMateria(int cantidad)
    {
        materia += cantidad;
        ActualizarHUDMateria();
    }

    void ActualizarHUDMateria()
    {
        materiaText.text = materia.ToString();
    }

    //----------------------------------------
    //    GAME OVER Y MENU
    //----------------------------------------

    void GameOver()
    {
        // Pausa el juego y muestra el UI del game over
        Time.timeScale = 0f;
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);

            // Buscar el componente NetworkManager y guardar la puntuacion
            var networkManager = FindObjectOfType<NetworkManager>();
            if (networkManager != null)
            {
                networkManager.GuardarPuntuacion(materia);
            }
        }
        else
        {
            Debug.LogWarning("gameOverUI es null, asegúrate de que existe en la escena.");
        }
    }

   
    //----------------------------------------
    //    GUARDAR DATOS Y TELETRANSPORTE
    //----------------------------------------

    // Metodo para guardar los datos del jugador con un mtodo del game manager
    public void GuardarDatosEnGameManager()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.GuardarDatosJugador(this);
        }
    }

    // Metodo pra teletransportar al jugador
    public void Teletransportar(Vector2 posicion)
    {
        rb.position = posicion;
        rb.linearVelocity = Vector2.zero;
        transform.position = posicion;
    }
}
