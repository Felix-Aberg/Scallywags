using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    [SerializeField] private bool useRigidBody;
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
        if (useRigidBody)
        {
            _rigidbody.DORotate(new Vector3(_halfSway, y, 0), 10).OnComplete(RotateAgain);
        }
        else
        {
            transform.DORotate(new Vector3(_halfSway, y, 0), 10).OnComplete(RotateAgainTransform);
        }

    }

    private void RotateAgain()
    {
        direction = !direction;
        if (direction)
        {
            _rigidbody.DORotate(new Vector3(_halfSway, y, 0), 10).OnComplete(RotateAgain);
        }
        else
        {
            _rigidbody.DORotate(new Vector3(-_halfSway, y, 0), 10).OnComplete(RotateAgain);
        }
    }
    
    private void RotateAgainTransform()
    {
        direction = !direction;
        if (direction)
        {
            transform.DORotate(new Vector3(_halfSway, y, 0), 10).OnComplete(RotateAgain);
        }
        else
        {
            transform.DORotate(new Vector3(-_halfSway, y, 0), 10).OnComplete(RotateAgain);
        }
    }
}
