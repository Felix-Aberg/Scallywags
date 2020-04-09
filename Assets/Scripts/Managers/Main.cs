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
        // Prefabs
        public GameObject _playerPrefab;

        private EntityManager _entityManager;

        private PlayerSpawn[] spawnPos;

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
            spawnPos = GameObject.FindObjectsOfType<PlayerSpawn>();
            _entityManager = new EntityManager(_playerPrefab, spawnPos);
            for (int i = 1; i < spawnPos.Length+1; i++) // Player index starts from 1
            {
                _entityManager.CreateEntity(EntityManager.EntityType.Player, i);
            }

            // Add players to camera
            foreach (var player in _entityManager.GetAllPlayers())
            {
                _targetGroup.AddMember(player.gameObject.transform, 1, 0);
            }
        }

        void Update()
        {
            _shipController.Tick();
            _entityManager.Tick();
        }
    }
}