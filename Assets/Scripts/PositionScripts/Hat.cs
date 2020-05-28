using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hat : MonoBehaviour
{
    // SkeletonHat
    public void Drop()
    {
        gameObject.transform.SetParent(null);
        var rb = gameObject.AddComponent<Rigidbody>();
        gameObject.AddComponent<SphereCollider>();
        rb.AddForce(transform.up*2f, ForceMode.Impulse);
    }
}
