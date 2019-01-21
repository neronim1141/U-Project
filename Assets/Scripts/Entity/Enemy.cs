using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;
    
    // Start is called before the first frame update
    void Start()
    {
         _navMeshAgent=this.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        Transform player= GameObject.FindGameObjectWithTag("Player").transform;
        if(_navMeshAgent.isOnNavMesh && player!=null )
            _navMeshAgent.SetDestination(player.position);
        
    }
}
