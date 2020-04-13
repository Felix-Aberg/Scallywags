using System.Collections;
using System.Collections.Generic;
using Cinemachine;
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
        public GameObject _playerPrefab;

        private EntityManager _entityManager;

        private PlayerSpawn[] _spawnPos;
        private TreasureManager _treasureManager;
        private RoundTimer _roundTimer;

        // Camera
        private CinemachineTargetGroup _targetGroup;
        
        // Monobehaviors
        private ShipController _shipController;
        private AudioSourcePoolManager _audioSourcePoolManager;

        void Awake()
        {
            _audioSourcePoolManager = gameObject.AddComponent<AudioSourcePoolManager>();
            
            // Find ship
            _shipController = FindObjectOfType<ShipController>();
            _shipController.Init();   
            
            // Setup camera
            _targetGroup = FindObjectOfType<CinemachineTargetGroup>();
            
            // Spawn players
            _spawnPos = GameObject.FindObjectsOfType<PlayerSpawn>();
            _entityManager = new EntityManager(_playerPrefab, _spawnPos);
            for (int i = 1; i < _spawnPos.Length+1; i++) // Player index starts from 1
            {
                _entityManager.CreateEntity(EntityManager.EntityType.Player, i);
            }

            // Add players to camera
            foreach (var player in _entityManager.GetAllPlayers())
            {
                _targetGroup.AddMember(player.gameObject.transform, 1, 0);
            }

            _treasureManager = new TreasureManager();
            _treasureManager.Init(goldCounterUI);
            
            _roundTimer = new RoundTimer();
            _roundTimer.Init(roundTimeUI);
        }

        void Update()
        {
            _shipController.Tick();
            _entityManager.Tick();
            _treasureManager.Tick();
            _roundTimer.Tick();

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
            
            if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                _entityManager.GetPlayer(1).SetKeyboard();
            }
        }
    }
}