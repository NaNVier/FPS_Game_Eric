using System.Security.Cryptography;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_AI : MonoBehaviour
{
    [SerializeField] Transform target;

    [SerializeField] float chaseRange = 5.0f;

    [SerializeField] float turnSpeed = 5.0f;

    float distanceToTarget = Mathf.Infinity;

    private bool isProvoked = false;

    NavMeshAgent nma;

    EnemyHealth health;

    void Start()
    {
        nma = GetComponent<NavMeshAgent>();

        health = GetComponent<EnemyHealth>();
    }

    void Update()
    {
        if (health.isDead)
        {
            enabled = false;

            nma.enabled = false;

            return;
        }

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
        FaceTarget();

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
        GetComponent<Animator>().SetBool("attack", false);

        GetComponent<Animator>().SetTrigger("run");

        nma.SetDestination(target.position);
    }

    private void AttackTarget()
    {
        GetComponent<Animator>().SetBool("attack", true);
    }

    private void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;

        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));

        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
    }

    public void OnDamageTaken()
    {
        isProvoked = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }
}
