using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[SelectionBase]
public class ShipController : MonoBehaviour
{
    private float _swayAmount = 7;
    private float _halfSway;
    private bool direction;
    private Rigidbody _rigidbody;
    private float y;

    public void Start()
    {
        _halfSway = _swayAmount * 0.5f;
        _rigidbody = GetComponent<Rigidbody>();
        y = transform.rotation.eulerAngles.y;
        _rigidbody.DORotate(new Vector3(_halfSway, y, 0), 5).OnComplete(RotateAgain);
    }

    private void RotateAgain()
    {
        direction = !direction;
        if (direction)
        {
            _rigidbody.DORotate(new Vector3(_halfSway, y, 0), 5).OnComplete(RotateAgain);
        }
        else
        {
            _rigidbody.DORotate(new Vector3(-_halfSway, y, 0), 5).OnComplete(RotateAgain);
        }
    }
}
