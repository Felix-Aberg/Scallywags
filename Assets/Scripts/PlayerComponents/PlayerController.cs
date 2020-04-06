using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// Holds player movement logic
/// </summary>
public class PlayerController
{
    [SerializeField] private float _speed = 5;
    void Init()
    {
        
    }

    public void Tick(Transform player)
    {
        // Tempcontrols

        var horizontal = Input.GetAxis("Horizontal");
        var Vertical = Input.GetAxis("Vertical");
        
        var moveDir = new Vector3(horizontal * Time.deltaTime * _speed, 0, Vertical * Time.deltaTime *_speed);
        player.rotation = Quaternion.LookRotation(moveDir);
        player.transform.position += moveDir;
    }
}
