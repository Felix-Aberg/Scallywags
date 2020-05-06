using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void TakeDamage();
    void TakeDamage(Vector3 hitDir, float hitForce);
}
