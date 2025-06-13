using UnityEngine;
using UnityEngine.SceneManagement;

namespace ControladorGame
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        private string spawnID = "";

        public int puntuacion;
        public int vida;
        public int materia;

        [SerializeField] private GameObject hudPrefab;
        [SerializeField] private GameObject musicPrefab;

        public GameObject hudCanvas;
        public GameObject musicObject;
        
        public PlayerController player;

        // Volver al menu principal
        public bool volverAlMenu = false;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);

                // Instanciar HUD solo si no existe
                GameObject hudExistente = GameObject.Find("HUD");
                if (hudExistente == null && hudPrefab != null)
                {
                    hudCanvas = Instantiate(hudPrefab);
                    hudCanvas.name = "HUD";
                    DontDestroyOnLoad(hudCanvas);
                }
                else
                {
                    hudCanvas = hudExistente;
                }

                // Instanciar música solo si no existe
                GameObject musicaExistente = GameObject.Find("Music");
                if (musicaExistente == null && musicPrefab != null)
                {
                    musicObject = Instantiate(musicPrefab);
                    musicObject.name = "Music";
                    DontDestroyOnLoad(musicObject);
                }
                else
                {
                    musicObject = musicaExistente;
                }

                UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
            }
            else
            {
                Destroy(gameObject); 
            }
        }

        void Start()
        {
            ReiniciarEstadoEnemigos();
        }

        public void SetSpawnID(string id)
        {
            spawnID = id;
        }

        //----------------------------------------
        //    GUARDAR Y CARGAR DATOS
        //----------------------------------------
        public void GuardarDatosJugador(PlayerController p)
        {
            player = p;
            vida = p.vidasActuales;
            materia = p.materia;
        }

        public void CargarDatosJugador(PlayerController player)
        {
            player.vidasActuales = vida;
            player.materia = materia;

            if (hudCanvas != null)
            {
                var vidaText = hudCanvas.transform.Find("Corazon").GetComponent<TMPro.TextMeshProUGUI>();
                var materiaText = hudCanvas.transform.Find("Puntuacion").GetComponent<TMPro.TextMeshProUGUI>();
                var gameOverUI = hudCanvas.transform.Find("PanelGameOver").gameObject;


                player.vidaText = vidaText;
                player.materiaText = materiaText;
                player.gameOverUI = gameOverUI;
            }
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "Principal")
            {
                Debug.Log("Escena Principal cargada, volverAlMenu = " + volverAlMenu);
                return;
            }

            if (hudCanvas == null)
            {
                GameObject hudExistente = GameObject.Find("HUD");
                if (hudExistente == null && hudPrefab != null)
                {
                    hudCanvas = Instantiate(hudPrefab);
                    hudCanvas.name = "HUD";
                    DontDestroyOnLoad(hudCanvas);
                }
                else
                {
                    hudCanvas = hudExistente;
                }
            }

            player = FindObjectOfType<PlayerController>();
            if (player == null)
            {
                Debug.LogError("No hay referencia al jugador persistente");
                return;
            }

            CargarDatosJugador(player);

            if (string.IsNullOrEmpty(spawnID))
            {
                Debug.Log("No hay spawnID configurado");
                return;
            }

            Spawn[] spawns = FindObjectsOfType<Spawn>();
            foreach (Spawn s in spawns)
            {
                if (s.spawnID == spawnID)
                {
                    Debug.Log("Posicionando jugador en spawnID: " + spawnID);
                    player.Teletransportar(s.transform.position);
                    break;
                }
            }

            spawnID = "";
        }

        //----------------------------------------
        //    Reinicio del juego
        //----------------------------------------

        // Reiniciar los enemigos para que vuelvan a aparecer en la siguiente partida
        public void ReiniciarEstadoEnemigos()
        {
            string[] enemigos = { "slime1", "slime2", "slime3", "slime4", "slime5", "slime6", "slime7", "slime8", "rata1", "rata2", "rata3", "rata4", "golem1", "golem2", "golem3", "golem4", "golem5", "golem6", "murcielago1", "murcielago2", "murcielago3", "murcielago4", "murcielago5", "murcielago6", "murcielago7", "cangrejo1", "cangrejo2", "cangrejo3", "cangrejo4", "cangrejo5", "cangrejo6", "cangrejo7", "cangrejo8", "cangrejo9", "cangrejo10", "cabeza1", "cabeza2", "cabeza3", "boss" };

            foreach (string id in enemigos)
            {
                PlayerPrefs.DeleteKey("EnemigoMuerto_" + id);
            }

            PlayerPrefs.Save();
        }

        public void ResetGame()
        {
            // Resetea las variables que guardas del jugador
            vida = 0;
            materia = 0;
            spawnID = "";

            // Reinicia estado de enemigos
            ReiniciarEstadoEnemigos();

            // Destruye el HUD actual para que se vuelva a instanciar limpio en la próxima escena
            if (hudCanvas != null)
            {
                Destroy(hudCanvas);
                hudCanvas = null;
            }

            // También puedes destruir la música si quieres reiniciarla
            if (musicObject != null)
            {
                Destroy(musicObject);
                musicObject = null;
            }

            // Si tienes referencia a player, la borras para que se vuelva a asignar
            player = null;

            // Importante: marcar que hemos vuelto al menú principal
            volverAlMenu = true;
        }

    }
}
