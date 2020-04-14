using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIFadeText : MonoBehaviour
{
    [SerializeField] private float _fadeSpeed = 1f;
    private TextMeshProUGUI _textMesh;
    
    void Start()
    {
        _textMesh = GetComponent<TextMeshProUGUI>();
    }
    
    void Update()
    {
        Color temp = _textMesh.color;
        temp.a = Mathf.MoveTowards(_textMesh.color.a, 0, Time.deltaTime * _fadeSpeed);
        _textMesh.color = temp;
    }
}
