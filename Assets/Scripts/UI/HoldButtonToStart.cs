using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScallyWags
{
    public class HoldButtonToStart : MonoBehaviour
    {
        [SerializeField] private PlayersSelected _playersSelected;
        private LoadScene _loadScene;
        private string _startKey = "Jump";
        private string _sceneName = "Loading";

        private float _startCounter;
        private float _startDelay = 2f;
        private StartGameProgressBar _startProgressBar;

        void Start()
        {
            _loadScene = GetComponent<LoadScene>();
            _startProgressBar = FindObjectOfType<StartGameProgressBar>();
            for (int i = 0; i < _playersSelected.PlayerCount; i++)
            {
                _playersSelected.SetPlayerReady(i, false);
            }     
        }

        void Update()
        {
            for (int i = 0; i < _playersSelected.PlayerCount; i++)
            {
                HandleInputs(i);
            }

            if (_startCounter > _startDelay)
            {
                _loadScene.LoadSceneByName(_sceneName);
            }

            _startProgressBar.UpdateSlider(_startCounter, _startDelay);
        }

        private void HandleInputs(int index)
        {
            var buttonIndex = index + 1;

            if (Input.GetButton(_startKey + buttonIndex))
            {
                _startCounter += Time.deltaTime;
            }

            if (Input.GetButtonUp(_startKey + buttonIndex))
            {
                _startCounter = 0;
            }
        }
    }
}
