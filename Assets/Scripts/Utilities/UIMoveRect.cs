using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMoveRect: MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 20f;
    private RectTransform _rectTransform;
    // Start is called before the first frame update
    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        _rectTransform.anchoredPosition = new Vector2(_rectTransform.anchoredPosition.x, _rectTransform.anchoredPosition.y + Time.deltaTime * _moveSpeed);
    }
}
