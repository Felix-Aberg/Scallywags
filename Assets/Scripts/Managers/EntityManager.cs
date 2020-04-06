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

        public enum EntityType
        {
            Player,
            Enemy
        }

        public EntityManager(GameObject playerPrefab)
        {
            _playerPrefab = playerPrefab;
        }

        public void Tick()
        {
            foreach (var entity in _entities)
            {
                entity.Tick();
            }
        }

        public void CreateEntity(EntityType type, Vector3 pos, int index)
        {
            switch (type)
            {
                case EntityType.Player:
                    CreatePlayer(pos, index);
                    break;
                case EntityType.Enemy:
                    // CreateEnemy();
                    throw new NotImplementedException();
                    break;
            }
        }

        private void CreatePlayer(Vector3 pos, int index)
        {
            var player = GameObject.Instantiate(_playerPrefab, pos, Quaternion.identity);
            IEntity entity = player.GetComponent<IEntity>();
            entity.Init(index);
            _players.Add(entity as Player);
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
