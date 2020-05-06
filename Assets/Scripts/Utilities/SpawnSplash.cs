using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSplash : MonoBehaviour
{
    [SerializeField] private GameObject splashEffect;
    private void OnTriggerEnter(Collider other)
    {
        var pos = other.transform.position;
        pos.y = 1f;
        Instantiate(splashEffect, pos, Quaternion.identity);
    }
}
