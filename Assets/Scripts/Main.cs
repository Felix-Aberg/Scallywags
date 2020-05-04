using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
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
        private NavMeshManager _navMeshManager;
        
        // Monobehaviors
        private AudioSourcePoolManager _audioSourcePoolManager;
        private HazardManager _hazardManager;

        void Awake()
        {
            _audioSourcePoolManager = gameObject.AddComponent<AudioSourcePoolManager>();

            // Spawn players
            _entityManager = new EntityManager(playerPrefab);
            for (int i = 1; i <= _numberOfPlayers; i++) // Player index starts from 1
            {
                _entityManager.CreatePlayer(i);
            }
            
            // Setup camera
            _cameraHandler = FindObjectOfType<CameraHandler>();
            _treasureManager = new TreasureManager();
            _roundTimer = new RoundTimer();
            _navMeshManager = gameObject.AddComponent<NavMeshManager>();
            _shipManager = gameObject.AddComponent<ShipManager>();
            _hazardManager = GetComponent<HazardManager>();
            _mortarManager = new MortarManager();
            _krakenManager = new KrakenManager();
            
            _cameraHandler.Init(_entityManager.GetAllPlayers());
            _treasureManager.Init(goldCounterUI);
            _roundTimer.Init(roundTimeUI);
            _navMeshManager.Init(_shipManager.GetShip(ShipType.Player));
            _shipManager.Init();
            _hazardManager.Init(roundTimeUI, _shipManager.GetShip(ShipType.Player));
            _mortarManager.Init();
            _krakenManager.Init();
        }

        void Update()
        {
            _hazardManager.Tick();
            _cameraHandler.Tick();
            _entityManager.Tick();
            _treasureManager.Tick();
            _roundTimer.Tick(_shipManager.GetShip(ShipType.Player));
            _shipManager.Tick();
            _krakenManager.Tick();

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }

            if (_treasureManager.GoldValue <= 0)
            {
                StartCoroutine(LoadScene("LoseScene"));
            }
        }

        private IEnumerator LoadScene(string scene)
        {
            yield return new WaitForSeconds(3f);
            SceneManager.LoadSceneAsync(scene);
        }
    }
}