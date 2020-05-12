using System.Collections;
using System.Collections.Generic;
using ScallyWags;
using UnityEngine;

public class TriggerEventOnStart : MonoBehaviour
{
    [SerializeField] private string eventName;

    // Start is called before the first frame update
    void Start()
    {
        EventManager.TriggerEvent(eventName, null);
    }
}
