using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScallyWags
{
    public class ActivatePlayers : MonoBehaviour
{
    [SerializeField] private PlayersSelected _playersSelected;
    private int _maxPlayers = 4;
    private string _startKey = "Pickup";
    private static ActivatePlayers instance;
    
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Update()
    {
        for (int i = 0; i < _playersSelected.PlayerCount; i++)
        {
            HandleInputs(i);
        }
    }

    private void HandleInputs(int index)
    {
        var buttonIndex = index + 1;
        if (Input.GetButtonDown(_startKey + buttonIndex))
        {
            _playersSelected.SetPlayerReady(index, true);
        }
    }
}
}
