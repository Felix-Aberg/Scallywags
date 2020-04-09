using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterSeconds : MonoBehaviour
{
    public float destroyAfterSeconds;
    void Start()
    {
        StartCoroutine("DestroyGameObject");
    }

    IEnumerator DestroyGameObject()
    {
        yield return new WaitForSeconds(destroyAfterSeconds);
        Destroy(gameObject);
    }
}
