using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPrompt : MonoBehaviour
{
    private RectTransform _rect;
    private Camera _camera;
    private Vector2 _canvasPos;
    private Vector3 _pos;

    public void Init(Vector3 pos, float yOffset)
    {
        _rect = GetComponent<RectTransform>();
        _camera = FindObjectOfType<Camera>();
        pos.y += yOffset;
        _pos = pos;
    }

    private void Update()
    {
        UpdatePos();
    }

    public void UpdatePos()
    {
        // Calculate *screen* position (note, not a canvas/recttransform position)
        var screenPoint = _camera.WorldToScreenPoint(_pos);

        transform.position = screenPoint;
    }
}
