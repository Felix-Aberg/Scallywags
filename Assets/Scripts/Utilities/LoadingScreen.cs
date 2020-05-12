using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private string sceneName;
    void Start()
    {
        StartCoroutine(LoadNextScene(sceneName));
    }

    private IEnumerator LoadNextScene(string sceneName)
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(sceneName);
    }
}
