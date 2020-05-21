using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToMenu : MonoBehaviour
{
    private LoadScene _loadScene;
    [SerializeField] private string _sceneName;

    // Start is called before the first frame update
    void Start()
    {
        _loadScene = FindObjectOfType<LoadScene>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _loadScene.LoadSceneByName(_sceneName);
        }
    }
}
