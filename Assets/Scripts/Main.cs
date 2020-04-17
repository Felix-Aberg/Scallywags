using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        private PlayerSpawn[] _spawnPos;
        private TreasureManager _treasureManager;
        private RoundTimer _roundTimer;
        private CameraHandler _cameraHandler;
        private ShipManager _shipManager;
        private MortarManager _mortarManager;
        
        // Monobehaviors
        private AudioSourcePoolManager _audioSourcePoolManager;
        private HazardManager _hazardManager;

        void Awake()
        {
            _audioSourcePoolManager = gameObject.AddComponent<AudioSourcePoolManager>();

            // Spawn players
            _spawnPos = GameObject.FindObjectsOfType<PlayerSpawn>();
            _entityManager = new EntityManager(playerPrefab, _spawnPos);
            for (int i = 1; i < _spawnPos.Length+1; i++) // Player index starts from 1
            {
                _entityManager.CreateEntity(EntityManager.EntityType.Player, i);
            }
            
            // Setup camera
            _cameraHandler = FindObjectOfType<CameraHandler>();
            _cameraHandler.Init(_entityManager.GetAllPlayers());
            
            _treasureManager = new TreasureManager();
            _treasureManager.Init(goldCounterUI);
            
            _roundTimer = new RoundTimer();
            _roundTimer.Init(roundTimeUI);

            _hazardManager = GetComponent<HazardManager>();
            _hazardManager.Init();

            _shipManager = gameObject.AddComponent<ShipManager>();
            _shipManager.Init();
            
            _mortarManager = new MortarManager();
            _mortarManager.Init();
        }

        void Update()
        {
            _hazardManager.Tick();
            _cameraHandler.Tick();
            _entityManager.Tick();
            _treasureManager.Tick();
            _roundTimer.Tick();
            _shipManager.Tick();

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
    }
}