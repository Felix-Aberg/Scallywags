using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1.3f;

    public void LoadSceneByName(string sceneName)
    {
        transition.SetTrigger("Start");
        StartCoroutine(WaitTime(sceneName));
        
    }

    IEnumerator WaitTime(string sceneName)
    {
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(sceneName);
    }

    
}
