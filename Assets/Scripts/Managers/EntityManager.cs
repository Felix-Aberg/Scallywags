using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ScallyWags
{
    /// <summary>
    /// Creates entities and holds a list of entities in game
    /// </summary>
    public class EntityManager
    {
        private List<IEntity> _entities = new List<IEntity>();
        private List<Player> _players = new List<Player>();

        private GameObject _playerPrefab;
        private GameObject _crabPrefab;
        private GameObject _skeletonPrefab;

        private PlayerSpawn[] _spawnPos;
        private EnemySpawn[] _enemySpawnPos;
        private int _enemyIndex = 0;

        public EntityManager(GameObject playerPrefab)
        {
            _playerPrefab = playerPrefab;
            _spawnPos = GameObject.FindObjectsOfType<PlayerSpawn>();
            _enemySpawnPos = GameObject.FindObjectsOfType<EnemySpawn>();

            EventManager.StartListening("SpawnEntity", EntityRequest);
        }

        public void Tick()
        {
            foreach (IEntity entity in _entities)
            {
                if (entity.IsDead())
                {
                    continue;
                }
                entity.Tick();
            }

            foreach (Player player in _players)
            {
                player.Tick();
            }
        }
        
        private void EntityRequest(EventManager.EventMessage msg)
        {
            if (msg.HazardData.Prefab == null)
            {
                Debug.LogError("No prefab set for spawning entity");
                return;
            }
            
            if(msg.HazardData.NumberOfHazards == 0)
            {
                Debug.LogError("Spawning zero entities");
                return;  
            }
            for(int i = 0; i < msg.HazardData.NumberOfHazards; i++)
            {
                if (_enemyIndex > _enemySpawnPos.Length-1) _enemyIndex = 0;
                CreateEnemy(_enemySpawnPos[_enemyIndex].transform.position, msg.HazardData.Prefab);
                _enemyIndex++;
            }
        }
        public void CreatePlayer(int index)
        {
            var pos = _spawnPos[index - 1].transform.position;
            var player = GameObject.Instantiate(_playerPrefab, pos, Quaternion.identity);
            IEntity entity = player.GetComponent<IEntity>();
            entity.Init(index);
            _players.Add(entity as Player);
        }

        private void CreateEnemy(Vector3 pos, GameObject prefab)
        {
            var enemy = GameObject.Instantiate(prefab, pos, Quaternion.identity);
            IEntity entity = enemy.GetComponent<IEntity>();
            entity.Init();
            AddEntity(entity);
        }
        private Player GetPlayer(int index)
        {
            foreach (var player in _players)
            {
                if (player.Index == index)
                {
                    return player;
                }
            }

            Debug.LogError("No player with index found: " + index);
            return null;
        }

        private void AddEntity(IEntity entity)
        {
            if (!_entities.Contains(entity))
            {
                _entities.Add(entity);
            }
            else
            {
                Debug.LogError("Trying to add same entity to entities list");
            }
        }

        public List<IEntity> GetAllEntities()
        {
            return _entities;
        }

        public List<Player> GetAllPlayers()
        {
            return _players;
        }
    }
}
