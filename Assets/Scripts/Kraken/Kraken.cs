﻿using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using ScallyWags;
using UnityEngine;
using Random = UnityEngine.Random;

public class Kraken : MonoBehaviour, IDamageable
{
    private int _maxHealth = 3;
    private int _health;
    private Animator _anim;

    private float _attackTimer;
    private float _attackDelay;
    private ShipCondition _playerShip;
    private float y;
    private KrakenAttack _krakenAttack;
    private KrakenManager _krakenManager;
    private string _krakenAttackEventName = "KrakenAttacks";

    public void Init(KrakenManager krakenManager, int health)
    {
        _krakenManager = krakenManager;
        
        _krakenAttack = GetComponentInChildren<KrakenAttack>();
        _health = health;
        if (_health == 0)
        {
            _health = 3;
            #if UNITY_EDITOR
                Debug.LogError("EventData missing health");
            #endif
        }
        _anim = GetComponent<Animator>();
        _attackDelay = Random.Range(1, 4);
        y = transform.position.y;

        var ships = FindObjectsOfType<ShipCondition>();
        foreach (var ship in ships)
        {
            if (ship.ShipType == ShipType.Player)
            {
                _playerShip = ship;
            }
        }
    }

    public void Tick()
    {
        if (_playerShip.IsSinking())
        {
            _health = 0;
        }
        UpdateDepth();
        AttackDecision();
        DieDecision();
    }

    private void DieDecision()
    {
        if (_health <= 0)
        {
            _anim.SetBool("Dead", true);
            _krakenManager.RemoveKraken(this);
        }
    }

    private void AttackDecision()
    {
        _attackTimer += Time.deltaTime;

        if (_attackTimer > _attackDelay)
        {
            _attackTimer = 0;
            _attackDelay = Random.Range(1, 4);
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
        int attack = Random.Range(0, 10);

        if (attack < 5)
        {
            _anim.SetInteger("Attack", 0);
        }

        if (attack >= 5)
        {
            _anim.SetInteger("Attack", 1);
        }
    }

    public void Die()
    {
        _health = 0;
    }

    public void TakeDamage()
    {
        _anim.SetTrigger("Damage");
        _health -= 1;
    }

    public void TakeDamage(Vector3 hitDir, float hitForce)
    {
        TakeDamage();
    }

    public Vector3 GetPos()
    {
        return transform.position;
    }
}
