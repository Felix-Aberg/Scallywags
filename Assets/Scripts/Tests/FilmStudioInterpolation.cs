using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilmStudioInterpolation : MonoBehaviour
{
    public Vector3 startingLocation;
    public Vector3 endingLocation;
    public float travelTime;

    [SerializeField] private float distance;
    [SerializeField] private float speed;

    // Start is called before the first frame update
    void Start()
    {
        distance = Vector3.Distance(startingLocation, endingLocation);
        speed = distance / travelTime;
    }

    // Update is called once per frame
    void Update()
    {

        transform.position = Vector3.MoveTowards(startingLocation, endingLocation, speed);
    }
}
