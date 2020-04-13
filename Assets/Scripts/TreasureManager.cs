using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ScallyWags
{
    /// <summary>
    /// Keeps track of the gold value on the ship
    /// </summary>
    [Serializable]
    public class TreasureManager
    {
        public int GoldValue => _goldValue;
        [SerializeField] int _goldValue;
        private ScoreItem[] gold;
        
        public void Init()
        {
            gold = GameObject.FindObjectsOfType<ScoreItem>();

            foreach (var item in gold)
            {
                _goldValue += item.GoldValue;
                item.Init(this);
            }

            if (_goldValue != 100)
            {
                Debug.LogError("Gold value not 100");
            }
        }

        public void Tick()
        {
            foreach (var item in gold)
            {
                item.Tick();
            }

            if (_goldValue <= 0)
            {
                SceneManager.LoadScene("LoseScene");
            }
        }

        public void ReCalculateGold()
        {
            _goldValue = 0;
            gold = GameObject.FindObjectsOfType<ScoreItem>();
            foreach (var item in gold)
            {
                _goldValue += item.GoldValue;
            }
        }
    }
}
