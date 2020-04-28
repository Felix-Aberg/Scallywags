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
    
        public void Init(FloatVariable roundTimeUI)
        {
            _roundTimeUI = roundTimeUI;
        }
    
        public void Tick()
        {
            _time -= Time.deltaTime;
            _roundTimeUI.Value = (float)_time;
            if (_time <= 0)
            {
                SceneManager.LoadScene("ScoreScene");
            }
        }
    }
}