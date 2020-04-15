using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

[SelectionBase]
public class ShipController : MonoBehaviour
{
    private Quaternion startRot;

    public void Start()
    {
        startRot = transform.rotation;
    }
    public void Update()
    {
        float f = Mathf.PingPong(Time.time * 1, 10) - 5;
        transform.rotation = startRot * Quaternion.AngleAxis(f, Vector3.right);
    }
}
