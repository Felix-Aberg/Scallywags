using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using ScallyWags;
using UnityEngine;
using Random = UnityEngine.Random;

public class Kraken : MonoBehaviour, IDamageable
{
    private int _maxHealth = 1;
    private int _health;
    private Animator _anim;

    private float _attackTimer;
    private float _attackDelay;
    private ShipCondition _playerShip;
    private float y;
    private KrakenAttack _krakenAttack;

    void Start()
    {
        _krakenAttack = GetComponentInChildren<KrakenAttack>();
        _health = _maxHealth;
        _anim = GetComponent<Animator>();
        _attackDelay = Random.Range(5, 10);
        y = transform.position.y;

        var ships = FindObjectsOfType<ShipCondition>();
        foreach (var ship in ships)
        {
            if (ship.ShipType == ShipManager.ShipType.Player)
            {
                _playerShip = ship;
            }
        }
    }
    
    void Update()
    {
        UpdateDepth();
        AttackDecision();
        DieDecision();
    }

    private void DieDecision()
    {
        if (_health <= 0)
        {
            _anim.SetBool("Dead", true);
        }
    }

    private void AttackDecision()
    {
        _attackTimer += Time.deltaTime;

        if (_attackTimer > _attackDelay)
        {
            _attackTimer = 0;
            _attackDelay = Random.Range(5, 10);
            Attack();
            _krakenAttack.EnableCollider();
        }
    }

    private void UpdateDepth()
    {
        transform.DOMoveY(y - _playerShip.DepthOffset(), 1f);
    }

    private void Attack()
    {
        int attack = Random.Range(0, 2);

        if (attack == 1)
        {
            _anim.SetTrigger("Slam");
            return;
        }

        if (attack == 2)
        {
            _anim.SetTrigger("Slap"); 
            return;
        }
    }

    public void TakeDamage()
    {
        _anim.SetTrigger("Damage");
        _health -= 1;
        Debug.Log("Kraken Damage");
    }
}
