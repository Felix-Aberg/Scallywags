using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

[SelectionBase]
public class ShipController : MonoBehaviour
{
    private Quaternion startRot;
    private float _swayAmount = 10;
    private float _halfSway;

    public void Start()
    {
        startRot = transform.rotation;
        _halfSway = _swayAmount * 0.5f;
    }
    public void Update()
    {
        float f = Mathf.PingPong(Time.time * 1, _swayAmount) - _halfSway;
        transform.rotation = startRot * Quaternion.AngleAxis(f, Vector3.right);
    }
}
