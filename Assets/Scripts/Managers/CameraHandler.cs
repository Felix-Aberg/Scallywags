using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace ScallyWags
{
    public class CameraHandler : MonoBehaviour
    {
        private EntityManager _entityManager;
        private List<Player> _players;
        private CinemachineTargetGroup _targetGroup;
        private CinemachineVirtualCamera _virtualCamera;
         
        public void Init(List<Player> players)
        {
            _targetGroup = FindObjectOfType<CinemachineTargetGroup>();
            _virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
            _players = players;

            // Add players to camera
            foreach (var player in _players)
            {
                _targetGroup.AddMember(player.gameObject.transform, 1, 0);
            }

            _virtualCamera.Follow = _targetGroup.transform;
            _virtualCamera.LookAt = _targetGroup.transform;
        }
        
        public void Tick()
        {
            for (int i = 0; i < _players.Count; i++)
            {
                if (_players[i].transform.position.y < 0)
                {
                    _targetGroup.m_Targets[i].weight = 0;
                }
                else
                {
                    _targetGroup.m_Targets[i].weight = 1;
                }
            }
        }
    }
}