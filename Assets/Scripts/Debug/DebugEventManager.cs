using System.Collections;
using System.Collections.Generic;
using ScallyWags;
using UnityEngine;

public class DebugEventManager : MonoBehaviour
{
    public HazardData enemyShip;
    public HazardData kraken;
    public HazardData crab;
    
    void OnGUI()
    {

        if (GUI.Button(new Rect(10, 70, 100, 30), "EnemyShip"))
        {
            EventManager.EventMessage eventMessage = new EventManager.EventMessage(enemyShip);
            EventManager.TriggerEvent("EnemyShip", eventMessage);
        }
        
        if (GUI.Button(new Rect(10, 110, 100, 30), "Kraken"))
        {
            EventManager.EventMessage eventMessage = new EventManager.EventMessage(kraken);
            EventManager.TriggerEvent("Kraken", eventMessage);
        }
        
        if (GUI.Button(new Rect(10, 150, 100, 30), "Crabs"))
        {
            EventManager.EventMessage eventMessage = new EventManager.EventMessage(crab);
            EventManager.TriggerEvent("SpawnEntity", eventMessage);
        }
        
        if (GUI.Button(new Rect(10, 200, 100, 30), "Pause"))
        {
            EventManager.EventMessage eventMessage = new EventManager.EventMessage(null);
            EventManager.TriggerEvent("Pause", eventMessage);
        }
    }
}
