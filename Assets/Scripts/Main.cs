﻿using System.Collections;
using System.Collections.Generic;
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

        private PlayerSpawn[] _spawnPos;
        private TreasureManager _treasureManager;
        private RoundTimer _roundTimer;
        private CameraHandler _cameraHandler;
        private ShipManager _shipManager;
        private MortarManager _mortarManager;
        [SerializeField] private KrakenManager _krakenManager;
        
        // Monobehaviors
        private AudioSourcePoolManager _audioSourcePoolManager;
        private HazardManager _hazardManager;
        public LevelEventManager _levelEventManager;

        void Awake()
        {
            _levelEventManager = gameObject.AddComponent<LevelEventManager>();
            
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
            
            _shipManager = gameObject.AddComponent<ShipManager>();
            _shipManager.Init();

            _hazardManager = GetComponent<HazardManager>();
            _hazardManager.Init(roundTimeUI, _shipManager.GetShip(ShipType.Player));

            _mortarManager = new MortarManager();
            _mortarManager.Init();
            
            _krakenManager = new KrakenManager();
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
                _levelEventManager.SetLevelPlayState(LevelEventManager.LevelPlayState.Quit);
                Application.Quit();
            }

            if (_treasureManager.GoldValue <= 0)
            {
                _levelEventManager.SetLevelPlayState(LevelEventManager.LevelPlayState.Lost);
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