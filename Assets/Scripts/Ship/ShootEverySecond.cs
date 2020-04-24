using System.Collections;
using System.Collections.Generic;
using ScallyWags;
using TMPro;
using UnityEngine;

public class ShootEverySecond : MonoBehaviour
{
    private EnemyCannon _cannon;
    private float _timer;
    private float _delay;
    private ShipCondition _ship;
    void Start()
    {
        _cannon = GetComponent<EnemyCannon>();
        _ship = GetComponentInParent<ShipCondition>();
        _delay = Random.Range(3f, 5f);
    }

    void Update()
    {
        if (_ship.GetHealth() <= 0)
        {
            return;
        }
        
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
