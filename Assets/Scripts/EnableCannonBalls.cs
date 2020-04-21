using System;
using UnityEngine;

namespace ScallyWags
{
    public class EnableCannonBalls : MonoBehaviour
{
    private Transform[] _cannonballs;
    void Start()
    {
        _cannonballs = GetComponentsInChildren<Transform>();
        
        foreach (var c in _cannonballs)
        {
            if (c == transform) continue;
            c.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        EventManager.StartListening("KrakenIntro", EnableGameObjects);
    }

    private void OnDisable()
    {
        EventManager.StopListening("KrakenIntro", EnableGameObjects);
    }

    private void EnableGameObjects(EventManager.EventMessage args)
    {
        foreach (var c in _cannonballs)
        {
            c.gameObject.SetActive(true);
        }
    }
}
}
