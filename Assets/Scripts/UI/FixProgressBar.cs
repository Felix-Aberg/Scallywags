using System;
using UnityEngine;
using UnityEngine.UI;

namespace ScallyWags
{
    public class FixProgressBar : MonoBehaviour
    {
        private Slider _slider;
        private RectTransform _rect;
        private Camera _camera;
        private Vector2 _canvasPos;
        private Vector3 _pos;

        public void Init(float start, float max, Vector3 pos)
        {
            _rect = GetComponent<RectTransform>();
            _slider = GetComponent<Slider>();
            _camera = FindObjectOfType<Camera>();
            _slider.value = start;
            _slider.maxValue = max;
            _slider.wholeNumbers = false;

            _pos = pos;
        }

        private void Update()
        {
            UpdatePos();
        }
        
        public void UpdateValues(float start, float max)
        {
            _slider.value = start;
            _slider.maxValue = max;
        }

        private void UpdatePos()
        {
            // Calculate *screen* position (note, not a canvas/recttransform position)
            var screenPoint = _camera.WorldToScreenPoint(_pos);

            transform.position = screenPoint;
        }
    }
}