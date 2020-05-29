using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private string sceneName;
    public Animator transition;
    public float transitionTime = 1.3f;
    bool isLoaded = false;
    public GameObject loadingText;
    public GameObject continueText;
    public GameObject loadingBoat;
    bool once;
    void Start()
    {
        StartCoroutine(LoadNextScene());
    }

    private void Update()
    {
        if (!once && isLoaded && Input.anyKeyDown)
        {
            once = true;
            transition.SetTrigger("Start");
            StartCoroutine(ActualLoadNextScene(sceneName));
        }
    }

    private IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(5f);
        isLoaded = true;
        loadingText.SetActive(false);
        loadingBoat.SetActive(false);
        continueText.SetActive(true);

    }

    private IEnumerator ActualLoadNextScene(string sceneName)
    {
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(sceneName);
        
    }
}
