using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour
{
    [SerializeField] MainMenu mainMenu;
    [SerializeField] Animator animator;
    [SerializeField] AnimatorFunctions animatorFunctions;
    [SerializeField] int thisIndex;
    [SerializeField] string scene;

    private int playerCount = 4;
    private string _startKey = "Jump";
    private bool _pressed;
    private LoadScene _loadScene;

    private void Start()
    {
        _loadScene = GetComponent<LoadScene>();
    }

    void Update()
    {
        for (int i = 0; i < playerCount; i++)
        {
            HandleInputs(i);
        }
        
        if(mainMenu.index == thisIndex)
        {
            animator.SetBool("Selected", true);
            if(_pressed)
            {
                _pressed = false;
                animator.SetBool("Pressed", true);
                switch (thisIndex)
                {
                    case 0:
                        _loadScene.LoadSceneByName(scene);
                        break;

                    case 1:
                        _loadScene.LoadSceneByName(scene);
                        break;

                    case 2:
                        Application.Quit();
                        break;
                }
            }
            else if(animator.GetBool("Pressed")){
                animator.SetBool("Pressed", false);
                animatorFunctions.disableOnce = true;
            }
        }
        else
        {
            animator.SetBool("Selected", false);
        }

    }
    
    private void HandleInputs(int index)
    {
        var buttonIndex = index + 1;
        if (Input.GetButtonDown(_startKey + buttonIndex))
        {
            _pressed = true;
        }
    }
}
