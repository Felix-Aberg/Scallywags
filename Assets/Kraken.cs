using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kraken : MonoBehaviour, IDamageable
{
    private int _maxHealth = 1;
    private int _health;

    private Animator _anim;
    void Start()
    {
        _health = _maxHealth;
        _anim = GetComponent<Animator>();
    }
    
    void Update()
    {
        if (_health <= 0)
        {
            _anim.SetBool("Dead", true);
        }
    }
    public void TakeDamage()
    {
        _anim.SetTrigger("Damage");
        _health -= 1;
        Debug.Log("Kraken Damage");
    }
}
