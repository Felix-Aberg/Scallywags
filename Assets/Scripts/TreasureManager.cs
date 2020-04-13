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
        int _goldValue;
        private ScoreItem[] gold;
        private IntVariable _goldCounterUI;
        
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

            if (_goldValue != 100)
            {
                Debug.LogError("Gold value not correct (100 expected) got: " + _goldValue);
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
