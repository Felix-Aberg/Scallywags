using UnityEngine;

namespace ScallyWags
{
    public class CreateUIElement : MonoBehaviour
        {
            public GameObject _goldLost;
            public GameObject _tauntBubble;
            private Camera _camera;
            
            // Start is called before the first frame update
            void Start()
            {
                _camera = FindObjectOfType<Camera>();
            }
        
            public void CreateElement(UIElement element, Vector3 pos)
            {
                switch (element)
                {
                    case UIElement.GoldLost:
                        InstantiateElement(_goldLost);
                        break;
                    case UIElement.SpeechBubble:
                        InstantiateElement(_tauntBubble, pos);
                        break;
                    default:
                        Debug.LogError("Missing UI element type");
                        break;
                }
            }

            private void InstantiateElement(GameObject prefab, Vector3 pos)
            {
                var uiElement = Instantiate(prefab, Vector3.zero, Quaternion.identity, gameObject.transform);
                var rectTransform = uiElement.GetComponent<RectTransform>();
                var screenPos = GetScreenPos(pos);
                rectTransform.anchoredPosition = new Vector2(screenPos.x, screenPos.y);
            }
            
            private void InstantiateElement(GameObject prefab)
            {
                var uiElement = Instantiate(prefab, Vector3.zero, Quaternion.identity, gameObject.transform);
                var rectTransform = uiElement.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2(0,0);
            }
            private Vector3 GetScreenPos(Vector3 worldPos)
            {
                Vector3 screenPos = _camera.WorldToScreenPoint(worldPos);
                Debug.Log("target is " + screenPos.x + " pixels from the left");
                return screenPos;
            }
        }
}