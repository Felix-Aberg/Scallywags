using DG.Tweening;
using TMPro;
using UnityEngine;

namespace ScallyWags
{
    public class CreateUIElement : MonoBehaviour
        {
            public GameObject _goldLost;
            public GameObject _tauntBubble;    
            public GameObject _progressBar;
            private Camera _camera;
 

            // Start is called before the first frame update
            void Start()
            {
                _camera = FindObjectOfType<Camera>();
            }
        
            public GameObject CreateElement(UIElement element, Vector3 pos, string text = "")
            {
                switch (element)
                {
                    case UIElement.GoldLost:
                        return InstantiateElement(_goldLost, text);
                        break;
                    case UIElement.SpeechBubble:
                        return InstantiateElement(_tauntBubble, pos);
                        break;
                    case UIElement.ProgressBar:
                        return InstantiateElement(_progressBar, pos);
                        break;
                    default:
                        Debug.LogError("Missing UI element type");
                        break;
                }

                return null;
            }

            private GameObject InstantiateElement(GameObject prefab, Vector3 pos)
            {
                if (_camera == null)
                {
                    _camera = FindObjectOfType<Camera>();
                }
                
                var uiElement = Instantiate(prefab, Vector3.zero, Quaternion.identity, gameObject.transform);
                var rect = uiElement.GetComponent<RectTransform>();
                
                // Calculate *screen* position (note, not a canvas/recttransform position)
                Vector2 screenPoint = _camera.WorldToScreenPoint(pos);
 
                // Convert screen position to Canvas / RectTransform space <- leave camera null if Screen Space Overlay
                RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, screenPoint, null, out var canvasPos);
 
                // Set
                uiElement.transform.localPosition = canvasPos;
                return uiElement;
            }
            
            private GameObject InstantiateElement(GameObject prefab, string text)
            {
                var uiElement = Instantiate(prefab, Vector3.zero, Quaternion.identity, gameObject.transform);
                var rectTransform = uiElement.GetComponent<RectTransform>();
                uiElement.GetComponent<TextMeshProUGUI>().text = text;
                rectTransform.anchoredPosition = new Vector2(0,0);
                return uiElement;
            }
            private Vector3 GetScreenPos(Vector3 worldPos)
            {
                Vector3 screenPos = _camera.WorldToScreenPoint(worldPos);
                return screenPos;
            }
            
            Vector2 WorldToCanvasPosition(Canvas canvas, RectTransform canvasRect, Camera camera, Vector3 position)
            {
                Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(camera, position);
                Vector2 result;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPoint, canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : camera, out result);
                return canvas.transform.TransformPoint(result);
            }
        }
}