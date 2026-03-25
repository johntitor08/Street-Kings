using UnityEngine;
using UnityEngine.AI;

public class EnemyPathfind : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform target;

    private void Awake() => agent = GetComponent<NavMeshAgent>();

    private void Update()
    {
        if (target != null)
            agent.SetDestination(target.position);
    }
}
