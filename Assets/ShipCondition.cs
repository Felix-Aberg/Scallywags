using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShipCondition : MonoBehaviour
{
    private enum ShipType
    {
        Enemy,
        Player
    }

    [SerializeField] private ShipType _shipType;
    [SerializeField] private int _shipHealth;
    private int _shipMaxHealth = 10;
    private float _startingDepth;
    private float _sinkingPerDamage = 0.5f;
    
    // Start is called before the first frame update
    void Start()
    {
        var pos = transform.position;
        transform.position = new Vector3(pos.x, 0, pos.z);
        _shipHealth = _shipMaxHealth;
        _startingDepth = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (_shipHealth <= 0)
        {
            if (_shipType == ShipType.Player)
            {
                SceneManager.LoadScene("LoseScene");
            }
        }
    }

    public void FixDamage()
    {
        var y = transform.position.y + _sinkingPerDamage;
        y = Mathf.Min(y, _startingDepth);
        transform.position = new Vector3(transform.position.x, y, transform.position.z);
    }

    public void TakeDamage()
    {
        _shipHealth -= 1;
        var pos = transform.position;
        transform.position = new Vector3(pos.x, pos.y - _sinkingPerDamage, pos.z);
    }
}
