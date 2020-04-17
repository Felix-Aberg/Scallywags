using System.Collections;
using System.Collections.Generic;
using ScallyWags;
using UnityEngine;

public class ShootEverySecond : MonoBehaviour
{
    private CannonInteraction _cannon;
    private float _timer;
    private float _delay;
    
    void Start()
    {
        _cannon = GetComponent<CannonInteraction>();
        _delay = Random.Range(3f, 5f);
    }

    void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= _delay)
        {
            _timer = 0;
            _delay = Random.Range(3f, 5f);
            Shoot();
        }
    }

    void Shoot()
    {
        _cannon.Act();
    }
}
