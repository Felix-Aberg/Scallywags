using UnityEngine;
using UnityEngine.UI;

namespace ScallyWags
{
    public class FixProgressBar : MonoBehaviour
    {
        private Slider _slider;
        private RectTransform _rect;
        private Camera _camera;

        public void Init(float start, float max)
        {
            _rect = GetComponent<RectTransform>();
            _slider = GetComponent<Slider>();
            _slider.value = start;
            _slider.maxValue = max;
            _camera = FindObjectOfType<Camera>();
        }

        public void UpdateValues(float start, float max, Vector3 pos)
        {
            _slider.value = start;
            _slider.maxValue = max;

            // Calculate *screen* position (note, not a canvas/recttransform position)
            Vector2 canvasPos;
            Vector2 screenPoint = _camera.WorldToScreenPoint(pos);
 
            // Convert screen position to Canvas / RectTransform space <- leave camera null if Screen Space Overlay
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_rect, screenPoint, null, out canvasPos);
 
            // Set
            transform.localPosition = canvasPos;
        }
    }
}