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

    // Update is called once per frame
    void Update()
    {
        if(mainMenu.index == thisIndex)
        {
            animator.SetBool("selected", true);
            if(Input.GetAxis ("Submit") == 1)
            {
                animator.SetBool("pressed", true);
                switch (thisIndex)
                {
                    case 0:
                        transform.GetComponent<LoadScene>().LoadSceneByName(scene);
                        break;

                    case 1:
                        transform.GetComponent<LoadScene>().LoadSceneByName(scene);
                        break;

                    case 2:
                        Application.Quit();
                        break;
                }
            }
            else if(animator.GetBool("pressed")){
                animator.SetBool("pressed", false);
                animatorFunctions.disableOnce = true;
            }
        }
        else
        {
            animator.SetBool("selected", false);
        }

    }
}
