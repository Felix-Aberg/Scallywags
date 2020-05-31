using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ScallyWags
{
    /// <summary>
    /// This script handles updating and creating all objects in the game
    /// </summary>
    public class Main : MonoBehaviour
    {
        // UI
        [SerializeField] private IntVariable goldCounterUI;
        [SerializeField] private FloatVariable roundTimeUI;

        // Players to spawn
        [SerializeField] private PlayersSelected _players;

        // Prefabs
        public Animator transition;
        public float transitionTime = 1.3f;

        // Prefabs
        public GameObject playerPrefab;

        private EntityManager _entityManager;

        private int _numberOfPlayers = 4;
        private TreasureManager _treasureManager;
        private RoundTimer _roundTimer;
        private CameraHandler _cameraHandler;
        private ShipManager _shipManager;
        private MortarManager _mortarManager;
        [SerializeField] private KrakenManager _krakenManager;

        // Monobehaviors
        private AudioSourcePoolManager _audioSourcePoolManager;
        private HazardManager _hazardManager;
        private LevelEventManager _levelEventManager;
        
        // Pause
        private bool _paused;
        private bool _levelLoading;

        void Awake()
        {
            #if !UNITY_EDITOR
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            #endif

            _levelEventManager = gameObject.AddComponent<LevelEventManager>();
            
            _audioSourcePoolManager = gameObject.AddComponent<AudioSourcePoolManager>();

            // Spawn players
            _entityManager = gameObject.AddComponent<EntityManager>();
            _entityManager.Init(playerPrefab);

            // Setup camera
            _cameraHandler = FindObjectOfType<CameraHandler>();
            _treasureManager = gameObject.AddComponent<TreasureManager>();
            _roundTimer = gameObject.AddComponent<RoundTimer>();
            _shipManager = gameObject.AddComponent<ShipManager>();
            _hazardManager = GetComponent<HazardManager>();
            _mortarManager = gameObject.AddComponent<MortarManager>();
            _krakenManager = gameObject.AddComponent<KrakenManager>();
            
            _cameraHandler.Init();
            _treasureManager.Init(goldCounterUI);
            _roundTimer.Init(roundTimeUI);
            _shipManager.Init(_hazardManager);
            _mortarManager.Init();
            _krakenManager.Init(_hazardManager, _shipManager, _roundTimer);
            _hazardManager.Init(_roundTimer, roundTimeUI, _shipManager.GetShip(ShipType.Player), _players);

            for (int i = 1; i <= _numberOfPlayers; i++) // Player index starts from 1
            {
                var index = i - 1;
                if (_players.GetPlayerReady(index))
                {
                    _entityManager.CreatePlayer(i);
                }
            }
        }

        void Start()
        {
            EventManager.TriggerEvent("PlayBattleMusic", null);
        }

        void Update()
        {
            if (_paused)
            {
                return;
            }

            _hazardManager.Tick();
            _cameraHandler.Tick();
            _entityManager.Tick();
            _treasureManager.Tick();
            _roundTimer.Tick(_shipManager.GetShip(ShipType.Player));
            _shipManager.Tick();
            _krakenManager.Tick();

            if (_shipManager.resetRound && !_levelLoading)
            {
                _levelLoading = true;
                _levelEventManager.SetLevelPlayState(LevelEventManager.LevelPlayState.Lost);
                StartCoroutine(LoadScene("LoseScene"));
            }
            
            if (_treasureManager.GoldValue <= 0)
            {
                _levelEventManager.SetLevelPlayState(LevelEventManager.LevelPlayState.Lost);
                StartCoroutine(LoadScene("LoseScene"));
            }

            PressButtonToJoin();
        }

        private void PressButtonToJoin()
        {
            if (Input.GetButton("Pickup1"))
            {
                if (_entityManager.GetPlayer(1) == null)
                {
                    _entityManager.CreatePlayer(1);
                    _players.SetPlayerReady(0, true);
                }
            }
            if (Input.GetButton("Pickup2"))
            {
                if (_entityManager.GetPlayer(2) == null)
                {
                    _entityManager.CreatePlayer(2);
                    _players.SetPlayerReady(1, true);
                }
            }
            if (Input.GetButton("Pickup3"))
            {
                if (_entityManager.GetPlayer(3) == null)
                {
                    _entityManager.CreatePlayer(3);
                    _players.SetPlayerReady(2, true);
                }
            }
            if (Input.GetButton("Pickup4"))
            {
                if (_entityManager.GetPlayer(4) == null)
                {
                    _entityManager.CreatePlayer(4);
                    _players.SetPlayerReady(3, true);
                }
            }
        }

        public void PauseGame(bool value)
        {
            _paused = value;
        }

        private IEnumerator LoadScene(string scene)
        {
            yield return new WaitForSeconds(3f);
            transition.SetTrigger("Start");
            yield return new WaitForSeconds(transitionTime);
            SceneManager.LoadSceneAsync(scene);
        }

        private void OnDisable()
        {
            _treasureManager = null;
            _roundTimer = null;
            _mortarManager = null;
            _krakenManager = null;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}