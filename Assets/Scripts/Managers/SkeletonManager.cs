using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

namespace ScallyWags
{
    public class SkeletonManager
    {
        private HazardData _skeletonData;
        private List<Skeleton> _skeletons = new List<Skeleton>();
        private ShipCondition _enemyCondition;
        private ShipCondition _shipCondition;
        private EntityManager _entityManager;

        public SkeletonManager(HazardData skeleton, EntityManager entityManager, ShipManager shipManager)
        {
            _skeletonData = skeleton;
            _enemyCondition = shipManager.GetShip(ShipType.Enemy);
            _shipCondition = shipManager.GetShip(ShipType.Player);
            _entityManager = entityManager;
        }

        public void Tick()
        {
            foreach (var skeleton in _skeletons)
            {
                if (skeleton.gameObject.activeInHierarchy)
                {
                    skeleton.Tick();
                }
            }
        }
        

        public void Spawn(Vector3 pos)
        {
            var go = GameObject.Instantiate(_skeletonData.Prefab, pos, Quaternion.identity);

            var skeleton = go.GetComponent<Skeleton>();
            skeleton.Init();
            skeleton.EntityManager = _entityManager;
            _skeletons.Add(skeleton);
        }
    }
}