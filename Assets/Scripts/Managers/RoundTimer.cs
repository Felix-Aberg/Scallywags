using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ScallyWags
{
    public class RoundTimer
    {
        private FloatVariable _roundTimeUI;
        private double _time = 300;
        private LevelEventManager _levelEventManager;
    
        public void Init(FloatVariable roundTimeUI)
        {
            _levelEventManager = GameObject.FindObjectOfType<LevelEventManager>();
            _roundTimeUI = roundTimeUI;
        }
    
        public void Tick(ShipCondition ship)
        {
            if (ship.GetHealth() <= 0) return;
            
            _time -= Time.deltaTime;
            _roundTimeUI.Value = (float)_time;
            if (_time <= 0)
            {
                _levelEventManager.SetLevelPlayState(LevelEventManager.LevelPlayState.Won);
                SceneManager.LoadScene("ScoreScene");
            }
        }
    }
}