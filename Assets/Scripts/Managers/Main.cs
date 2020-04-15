﻿using System.Collections;
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
        private CameraHandler _cameraHandler;

        // Monobehaviors
        private ShipController _shipController;
        private AudioSourcePoolManager _audioSourcePoolManager;
        private HazardManager _hazardManager;

        void Awake()
        {
            _audioSourcePoolManager = gameObject.AddComponent<AudioSourcePoolManager>();
            
            // Find ship
            _shipController = FindObjectOfType<ShipController>();
            _shipController.Init();

            // Spawn players
            _spawnPos = GameObject.FindObjectsOfType<PlayerSpawn>();
            _entityManager = new EntityManager(_playerPrefab, _spawnPos);
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
        }

        void Update()
        {
            _hazardManager.Tick();
            _cameraHandler.Tick();
            _shipController.Tick();
            _entityManager.Tick();
            _treasureManager.Tick();
            _roundTimer.Tick();

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
    }
}