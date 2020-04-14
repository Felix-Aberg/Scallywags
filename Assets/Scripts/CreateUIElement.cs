using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace ScallyWags
{
    public class CreateUIElement : MonoBehaviour
        {
            public GameObject _goldLost;
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
                        var uielement = Instantiate(_goldLost, Vector3.zero, Quaternion.identity, gameObject.transform);
                        var rectTransform = uielement.GetComponent<RectTransform>();
                        rectTransform.anchoredPosition = new Vector2(0,0);
                        break;
                    default:
                        Debug.LogError("Missing UI element type");
                        break;
                }
            }
        
            private Vector3 GetScreenPos(Vector3 worldPos)
            {
                Vector3 screenPos = _camera.WorldToScreenPoint(worldPos);
                Debug.Log("target is " + screenPos.x + " pixels from the left");
                return screenPos;
            }
        }
}