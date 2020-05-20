using System.Collections;
using System.Collections.Generic;
using ScallyWags;
using UnityEditor;
using UnityEngine;

public class ActivatePlayers : MonoBehaviour
{
    [SerializeField] private PlayersSelected _playersSelected;
    private LoadScene _loadScene;
    private int _maxPlayers = 4;
    private string _startKey = "Pickup";
    private bool _playerReady;
    private string _sceneName = "Loading";

    private float _startCounter;
    private float _startDelay = 2f;
    private StartGameProgressBar _startProgressBar;

    // Start is called before the first frame update
    void Start()
    {
        _loadScene = GetComponent<LoadScene>();
        for (int i = 0; i < _playersSelected.PlayerCount; i++)
        {
            _playersSelected.SetPlayerReady(i, false);
        }

        _startProgressBar = FindObjectOfType<StartGameProgressBar>();
    }

    // Update is called once per frame
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
        if (Input.GetButtonDown(_startKey + buttonIndex))
        {
            _playersSelected.SetPlayerReady(index, true);
            _playerReady = true;
            _startCounter = 0;
        }

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
