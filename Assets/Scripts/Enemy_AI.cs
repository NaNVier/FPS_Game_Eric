using System.Security.Cryptography;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_AI : MonoBehaviour
{
    [SerializeField] Transform target;

    [SerializeField] float chaseRange = 5.0f;

    float distanceToTarget = Mathf.Infinity;

    private bool isProvoked = false;

    NavMeshAgent nma;

    void Start()
    {
        nma = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        distanceToTarget = Vector3.Distance(target.position, transform.position);

        if (isProvoked)
        {
            EngageTarget();
        }
        else if(distanceToTarget <= chaseRange)
        {
            isProvoked = true;
        }

    }

    private void EngageTarget()
    {
        if (distanceToTarget >= nma.stoppingDistance)
        {
            ChaseTarget();
        }

        if (distanceToTarget <= nma.stoppingDistance)
        {
            AttackTarget();
        }
    }

    private void ChaseTarget()
    {
        nma.SetDestination(target.position);
    }

    private void AttackTarget()
    {
        //temp for now to show that the enemy is attacking
        print(name + " is attacking " + target.name);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }
}
