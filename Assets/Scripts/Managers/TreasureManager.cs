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
    public class TreasureManager : MonoBehaviour
    {
        public int GoldValue => _goldValue;
        private int _goldValue;
        private ScoreItem[] gold;
        private IntVariable _goldCounterUI;
        private int _startingGold = 400;
        
        private LevelEventManager _levelEventManager;
        
        public void Init(IntVariable goldCounter)
        {
            _goldCounterUI = goldCounter;
            if (goldCounter == null)
            {
                Debug.LogError("No gold counter asset assigned to main.cs script");
            }
            gold = GameObject.FindObjectsOfType<ScoreItem>();
            _levelEventManager = GameObject.FindObjectOfType<LevelEventManager>();

            foreach (var item in gold)
            {
                _goldValue += item.GoldValue;
                item.Init(this);
            }

            if (_goldValue != _startingGold)
            {
                Debug.LogError("Gold value not correct ("+ _startingGold +" expected) got: " + _goldValue);
            }
        }

        public void Tick()
        {
            // Update UI
            _goldCounterUI.Value = _goldValue;
            
            foreach (var item in gold)
            {
                item.Tick();
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
            
            _levelEventManager.UpdateScore(_goldValue);
        }
    }
}
