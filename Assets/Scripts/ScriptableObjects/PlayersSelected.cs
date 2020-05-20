using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScallyWags
{
    [CreateAssetMenu(menuName = "PlayersReady")]
    public class PlayersSelected : ScriptableObject
    {
        public bool[] _playersReady = new bool[4];
        public int PlayerCount = 4;
        
        /// <summary>
        /// Set players ready (indexing 0-3)
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public void SetPlayerReady(int index, bool value)
        {
            if (index < 0 || index > 3) return;

            _playersReady[index] = value;
        }

        /// <summary>
        /// Check if player is ready (indexing 0-3)
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public bool GetPlayerReady(int index)
        {
            if (index < 0 || index > 3) return false;

            return _playersReady[index];
        }
    }
}