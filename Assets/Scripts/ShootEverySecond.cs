using System.Collections;
using System.Collections.Generic;
using ScallyWags;
using UnityEngine;

public class ShootEverySecond : MonoBehaviour
{
    private CannonInteraction _cannon;
    // Start is called before the first frame update
    void Start()
    {
        _cannon = GetComponent<CannonInteraction>();
        InvokeRepeating(nameof(Shoot), Random.Range(1f, 2.0f), 10);
    }

    void Shoot()
    {
        _cannon.Act();
    }
}
