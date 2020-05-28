using System.Collections;
using System.Collections.Generic;
using ScallyWags;
using UnityEngine;

public class DebugEventManager : MonoBehaviour
{
    public HazardData enemyShip;
    public HazardData kraken;
    public HazardData crab;
    
    #if UNITY_EDITOR
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
        
        if (GUI.Button(new Rect(10, 190, 100, 30), "Pause"))
        {
            EventManager.EventMessage eventMessage = new EventManager.EventMessage(null);
            EventManager.TriggerEvent("Pause", eventMessage);
        }
        
        if (GUI.Button(new Rect(10, 230, 100, 30), "EnableItems"))
        {
            var buckets = FindObjectOfType<EnableBuckets>();
            buckets.EnableTools();
            
            var swords = FindObjectOfType<EnableSwords>();
            swords.EnableTools();
            
            var cannonballs = FindObjectOfType<EnableCannonBalls>();
            cannonballs.EnableTools();
        }
    }
    #endif
}
