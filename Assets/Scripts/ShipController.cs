using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class ShipController : MonoBehaviour
{
    private Quaternion startRot;

    public void Init()
    {
        startRot = transform.rotation;
    }
    public void Tick()
    {
        float f = Mathf.PingPong(Time.time * 3, 10);
        transform.rotation = startRot * Quaternion.AngleAxis(f, Vector3.forward);
    }
}
