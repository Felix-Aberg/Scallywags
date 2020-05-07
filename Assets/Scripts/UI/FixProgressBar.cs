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

        public void Init(float start, float max)
        {
            _rect = GetComponent<RectTransform>();
            _slider = GetComponent<Slider>();
            _slider.value = start;
            _slider.maxValue = max;
            _slider.wholeNumbers = false;
            _camera = FindObjectOfType<Camera>();
        }

        public void UpdateValues(float start, float max)
        {
            _slider.value = start;
            _slider.maxValue = max;
        }

        public void UpdatePos(Vector3 pos)
        {
            // Calculate *screen* position (note, not a canvas/recttransform position)
            Vector2 screenPoint = _camera.WorldToScreenPoint(pos);

            // Convert screen position to Canvas / RectTransform space <- leave camera null if Screen Space Overlay
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_rect, screenPoint, null, out _canvasPos);

            transform.localPosition = _canvasPos;
        }
    }
}