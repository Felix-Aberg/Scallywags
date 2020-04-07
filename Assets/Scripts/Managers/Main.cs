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
        // Prefabs
        public GameObject _playerPrefab;

        private EntityManager _entityManager;

        private GameObject[] spawnPos;

        void Start()
        {
            // Spawn players
            spawnPos = GameObject.FindGameObjectsWithTag("Spawn");
            _entityManager = new EntityManager(_playerPrefab);
            for (int i = 1; i < spawnPos.Length+1; i++) // Players start from 1
            {
                _entityManager.CreateEntity(EntityManager.EntityType.Player, spawnPos[i].transform.position, i);
            }
        }

        void Update()
        {
            _entityManager.Tick();
        }
    }
}