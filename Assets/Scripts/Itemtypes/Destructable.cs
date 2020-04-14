using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable : MonoBehaviour, IDamageable
{
    public void TakeDamage()
    {
        gameObject.SetActive(false);
    }
}
