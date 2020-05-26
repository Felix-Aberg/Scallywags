using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableRandomHat : MonoBehaviour
{
    [SerializeField] private GameObject _captainHat;
    [SerializeField] private GameObject _scarf;
    void Start()
    {
        _captainHat.SetActive(false);
        _scarf.SetActive(false);
        
        var random = Random.Range(0, 2);
        if (random == 0)
        {
            _captainHat.SetActive(true);
        }
        else
        {
            _scarf.SetActive(true);
        }
    }
}
