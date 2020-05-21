using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerMenu : MonoBehaviour
{
    [SerializeField] private string _characterSelect = "CharacterSelect";
    [SerializeField] private string _mainMenu = "MainMenu";
    private LoadScene _loadScene;

    void Start()
    {
        _loadScene = GetComponentInChildren<LoadScene>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Submit"))
        {
            _loadScene.LoadSceneByName(_characterSelect);
        }

        if (Input.GetButtonDown("Cancel"))
        {
            _loadScene.LoadSceneByName(_mainMenu);
        }
    }
}
