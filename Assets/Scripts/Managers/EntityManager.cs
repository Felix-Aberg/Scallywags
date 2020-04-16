using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        // private GameObject _enemy1;

        private PlayerSpawn[] _spawnPos;
        
        public enum EntityType
        {
            Player,
            Enemy
        }

        public EntityManager(GameObject playerPrefab, PlayerSpawn[] spawnPos)
        {
            _playerPrefab = playerPrefab;
            _spawnPos = spawnPos;
        }

        public void Tick()
        {
            foreach (IEntity entity in _entities)
            {
                entity.Tick();
            }

            foreach (Player player in _players)
            {
                player.Tick();
            }
        }

        public void CreateEntity(EntityType type, int index)
        {
            switch (type)
            {
                case EntityType.Player:
                    CreatePlayer(_spawnPos[index-1].transform.position, index);
                    break;
                case EntityType.Enemy:
                    // CreateEnemy();
                    // AddEntity(entity);
                    throw new NotImplementedException();
                    break;
            }
        }

        private void RespawnEntity(EntityType type, int index)
        { 
            var player = GetPlayer(index);
            player.gameObject.SetActive(false);

            player.transform.position = _spawnPos[index-1].transform.position;
            // Todo delay & visual effects
            player.gameObject.SetActive(true);
        }

        private void CreatePlayer(Vector3 pos, int index)
        {
            var player = GameObject.Instantiate(_playerPrefab, pos, Quaternion.identity);
            IEntity entity = player.GetComponent<IEntity>();
            entity.Init(index);
            _players.Add(entity as Player);
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
