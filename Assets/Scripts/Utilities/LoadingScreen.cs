using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private string sceneName;
    public Animator transition;
    public float transitionTime = 1.3f;
    void Start()
    {
        StartCoroutine(LoadNextScene(sceneName));
    }

    private IEnumerator LoadNextScene(string sceneName)
    {
        yield return new WaitForSeconds(5f);
        //AsyncOperation scene = SceneManager.LoadSceneAsync(sceneName);
        //while (!scene.isDone)
        //{
        //    yield return new WaitForEndOfFrame();
        //}
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        //SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
        SceneManager.LoadScene(sceneName);
    }
}
