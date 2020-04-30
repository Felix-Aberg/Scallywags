using System.Collections;
using System.Collections.Generic;
using ScallyWags;
using UnityEngine;
using UnityEngine.AI;

public class Skeleton : MonoBehaviour
{
    private Player[] _players;
    private NavMeshAgent _navMeshAgent;

    [SerializeField] private bool _focusTreasure;

    void Start()
    {
        _players = FindObjectsOfType<Player>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }
    
    void Update()
    {
        if (_navMeshAgent.hasPath) return;
        
        _navMeshAgent.SetDestination(_players[0].transform.position);
    }
}
