using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using ScallyWags;
using UnityEngine;

public class FireInteraction : MonoBehaviour, IInteraction
{
    private ShipCondition _shipCondition;
    private float _timer;
    private float _delay = 10f;
    
    [SerializeField] private int waterBuckets = 0;
    private int waterRequired = 5;

    private int damageDone;
    
    public void Start()
    {
        _shipCondition = GetComponentInParent<ShipCondition>();
    }

    void Update()
    {
        if (_shipCondition == null) return;

        _timer += Time.deltaTime;

        if (_timer > _delay)
        {
            _timer = 0;
            _shipCondition.TakeDamage();
            damageDone++;
        }
    }

    public void Act()
    {
        waterBuckets++;
        var newScale = new Vector3(Mathf.Max(transform.localScale.x - 0.1f, 0.3f), Mathf.Max(transform.localScale.y - 0.1f, 0.3f), Mathf.Max(transform.localScale.z - 0.1f, 0.3f));

        transform.DOScale(newScale, 0.2f);
        
        if (waterBuckets >= waterRequired)
        {
            PutOutFire();
        }
    }

    private void PutOutFire()
    {
        Debug.Log("Fire put out");
        waterBuckets = 0;
        _shipCondition.FixDamage(damageDone);
        gameObject.SetActive(false);
    }
}
