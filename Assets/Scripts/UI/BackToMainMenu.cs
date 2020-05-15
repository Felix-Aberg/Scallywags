using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMainMenu : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1.3f;

    private void Update()
    {
        if (Input.anyKey)
        {
            transition.SetTrigger("Start");
            StartCoroutine(WaitTime());
        }

        IEnumerator WaitTime()
        {
            yield return new WaitForSeconds(transitionTime);
            SceneManager.LoadScene("MainMenu");
        }
    }
}
