using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinScreenController : MonoBehaviour
{
    [SerializeField] private string _mainMenu;
    private LoadScene _loadScene;
    private bool canPressContinue;
    public Image panel;
    public GameObject text;
    public GameObject anyToContinue;
    public float fadeTime;
    
    void Start()
    {
        panel = transform.Find("Panel").GetComponent<Image>();
        text = transform.Find("Panel").Find("Text").gameObject;
        anyToContinue = transform.Find("Panel").Find("Any To Continue").gameObject;
        Color blank = new Color(255f, 255f, 255f, 0f);
        panel.color = blank;
        text.SetActive(false);
        anyToContinue.SetActive(false);
        _loadScene = GetComponentInChildren<LoadScene>();
        StartCoroutine(WaitTime());
    }

    private void Update()
    {
        if (canPressContinue)
        {
            Color temp = panel.color;
            temp.a += fadeTime * Time.deltaTime;
            panel.color = temp;
            
            if (temp.a > 0.99f)
            {
                text.SetActive(true);
                anyToContinue.SetActive(true);
            }

            if (Input.anyKeyDown)
            {
                _loadScene.LoadSceneByName("MainMenu");
            }
        }


    }
    IEnumerator WaitTime()
    {
        yield return new WaitForSeconds(4);
        canPressContinue = true;
    }

   
}