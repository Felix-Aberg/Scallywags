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

    // Start is called before the first frame update
    void Start()
    {
        _loadScene = GetComponent<LoadScene>();
        for (int i = 0; i < _playersSelected.PlayerCount; i++)
        {
            _playersSelected.SetPlayerReady(i, false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < _playersSelected.PlayerCount; i++)
        {
            HandleInputs(i);
        }

        if (_playerReady)
        {
            _loadScene.LoadSceneByName(_sceneName);
        }
    }

    private void HandleInputs(int index)
    {
        var buttonIndex = index + 1;
        if (Input.GetButtonDown(_startKey + buttonIndex))
        {
            _playersSelected.SetPlayerReady(index, true);
            _playerReady = true;
        }
    }
}
