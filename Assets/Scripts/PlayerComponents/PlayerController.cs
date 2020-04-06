using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// Holds player movement logic
/// </summary>
public class PlayerController
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _turnSpeed = 10f;
    void Init()
    {
        
    }

    public void Tick(Transform player)
    {
        // Tempcontrols

        var horizontal = Input.GetAxis("Horizontal");
        var Vertical = Input.GetAxis("Vertical");

        var moveDir = new Vector3(horizontal * Time.deltaTime * _speed, 0, Vertical * Time.deltaTime * _speed);
        player.transform.position += moveDir;
        
        if (moveDir != Vector3.zero)
        {
            var rot = Quaternion.LookRotation(moveDir);
            player.rotation = Quaternion.RotateTowards(player.transform.rotation, rot, _turnSpeed);
        }
    }
}
