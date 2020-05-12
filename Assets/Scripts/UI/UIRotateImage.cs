using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIRotateImage : MonoBehaviour
{
    private RectTransform rectTransform;
    private float _speed = 100f;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        var z = rectTransform.transform.eulerAngles.z - Time.deltaTime * _speed;
        rectTransform.rotation = Quaternion.Euler(new Vector3(0,0, z));
    }
}
