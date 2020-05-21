using System.Collections;
using System.Collections.Generic;
using ScallyWags;
using TMPro;
using UnityEngine;

public class SetPlayerReady : MonoBehaviour
{
    [SerializeField] private Color _readyColor;
    private TextMeshProUGUI[] _text;
    [SerializeField] private PlayersSelected _playersSelected;
    // Start is called before the first frame update
    void Start()
    {
        _text = GetComponentsInChildren<TextMeshProUGUI>();

        foreach (var text in _text)
        {
            text.text = "Press A to join";
        }
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < _playersSelected.PlayerCount; i++)
        {
            if (_playersSelected.GetPlayerReady(i))
            {
                var index = i + 1;
                _text[i].text = "P" + index + " Ready";
                _text[i].color = _readyColor;
            }
        }
    }
}
