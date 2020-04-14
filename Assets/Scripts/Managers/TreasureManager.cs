using System;
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
        private int _goldValue;
        private ScoreItem[] gold;
        private IntVariable _goldCounterUI;
        private int _startingGold = 100;
        
        public void Init(IntVariable goldCounter)
        {
            _goldCounterUI = goldCounter;
            if (goldCounter == null)
            {
                Debug.LogError("No gold counter asset assigned to main.cs script");
            }
            gold = GameObject.FindObjectsOfType<ScoreItem>();

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

            if (_goldValue <= 0)
            {
                SceneManager.LoadSceneAsync("LoseScene");
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
