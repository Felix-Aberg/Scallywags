using System.Collections.Generic;
using UnityEngine;

namespace ScallyWags
{
    public class SkeletonManager
    {
        private HazardData _skeletonData;
        private Player[] _players;
        private List<Skeleton> _skeletons = new List<Skeleton>();
        private ShipCondition _enemyCondition;
        private ShipCondition _shipCondition;

        public SkeletonManager(HazardData skeleton, Player[] players, ShipManager shipManager)
        {
            _skeletonData = skeleton;
            _players = players;
            _enemyCondition = shipManager.GetShip(ShipType.Enemy);
            _shipCondition = shipManager.GetShip(ShipType.Player);
        }

        public void Tick()
        {
            if (Vector3.Distance(_enemyCondition.transform.position, _shipCondition.transform.position) > 30f)
            {
                return;
            }
            
            foreach (var skeleton in _skeletons)
            {
                skeleton.Tick();
            }
        }

        public void Spawn(Vector3 pos)
        {
            var go = GameObject.Instantiate(_skeletonData.Prefab, pos, Quaternion.identity);

            var skeleton = go.GetComponent<Skeleton>();
            skeleton.SetPlayers(_players);
            skeleton.Init();
            _skeletons.Add(skeleton);
        }
    }
}