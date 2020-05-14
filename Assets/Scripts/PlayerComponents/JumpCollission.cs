using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpCollission : MonoBehaviour
{
    public bool grounded;
    [SerializeField] int objectsColliding;

    public void Tick()
    {
        if (objectsColliding == 0)
        {
            grounded = false;
        }
        else if (objectsColliding > 0)
        {
            grounded = true;
        }
        else
        {
            Debug.LogError("ERROR! Colliding with a negative amount of objects!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        objectsColliding++;
    }

    private void OnTriggerExit(Collider other)
    {
        objectsColliding--;
    }
}