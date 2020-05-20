using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    private bool _pause;
    [SerializeField] private GameObject _pauseCanvas;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _pause = !_pause;
        }
        
        if (_pause)
        {
            _pauseCanvas.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            _pauseCanvas.SetActive(false);
            Time.timeScale = 1;
        }
    }
}
