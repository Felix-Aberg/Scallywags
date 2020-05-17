using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ScallyWags
{
    public class RoundTimer : MonoBehaviour
    {
        private FloatVariable _roundTimeUI;
        private double _time = 300;
        private LevelEventManager _levelEventManager;
        private bool _stopped;

        public void Init(FloatVariable roundTimeUI)
        {
            _levelEventManager = GameObject.FindObjectOfType<LevelEventManager>();
            _roundTimeUI = roundTimeUI;
        }
    
        public void Tick(ShipCondition ship)
        {
            if (_stopped) return;
            if (ship.IsSinking()) return;
            
            _time -= Time.deltaTime;
            _roundTimeUI.Value = (float)_time;
            if (_time <= 0)
            {
                _levelEventManager.SetLevelPlayState(LevelEventManager.LevelPlayState.Won);
                EventManager.TriggerEvent("RoundOver", null);
                StartCoroutine(EndCinematics());
                _time = 0;
                _stopped = true;
            }
        }

        private IEnumerator EndCinematics()
        {
            yield return new WaitForSeconds(15f);
            SceneManager.LoadScene("ScoreScene");
        }
    }
}