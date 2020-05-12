using System;
using System.Collections;
using System.Collections.Generic;
using ScallyWags;
using UnityEngine;

public class SpawnSplash : MonoBehaviour
{
    [SerializeField] private GameObject splashEffect;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Water>())
        {
            var pos = transform.position;
            pos.y = 1f;
            Instantiate(splashEffect, pos, Quaternion.identity);
            EventManager.TriggerEvent("Splash", null);
        }
    }
}
