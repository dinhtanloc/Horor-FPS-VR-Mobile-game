using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(Animator), typeof(NavMeshAgent))]
public class ZombieAI : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public float chaseRange = 10f;
    public float attackRange = 2f;
    public float wanderRadius = 5f; // bán kính di chuyển random

    [Header("Health Settings")]
    public float health = 100f;

    private Animator animator;
    private NavMeshAgent agent;
    private Vector3 lastKnownPlayerPosition;
    private bool isWandering = false;
    private bool isChasing = false;
    private Coroutine wanderCoroutine;

    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        if (animator == null)
            Debug.LogError("Animator chưa được gán!");
        if (agent == null)
            Debug.LogError("NavMeshAgent chưa được gán!");
        if (!agent.isOnNavMesh)
            Debug.LogError("Zombie không nằm trên NavMesh!");

        agent.enabled = true;
    }

    void Update()
    {
        if (health <= 0f)
        {
            Die();
            return;
        }

        float distance = Vector3.Distance(transform.position, player.position);
        animator.SetFloat("DistanceToPlayer", distance);

        if (distance <= attackRange)
        {
            AttackPlayer();
        }
        else if (distance <= chaseRange)
        {
            ChasePlayer(distance);
        }
        else
        {
            WanderAround();
        }

        // Debug tốc độ di chuyển
        if (agent != null)
        {
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0f)
        {
            animator.SetTrigger("isDead");
        }
        else
        {
            animator.SetTrigger("hitTrigger");
        }
    }

    void ChasePlayer(float distance)
    {
        isChasing = true;
        isWandering = false;
        animator.SetBool("isChasing", true);
        animator.SetBool("isWandering", false);

        if (wanderCoroutine != null)
        {
            StopCoroutine(wanderCoroutine);
            wanderCoroutine = null;
        }

        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

        if (agent != null && agent.isOnNavMesh)
        {
            agent.SetDestination(player.position);
        }

        if (distance <= chaseRange && !animator.GetCurrentAnimatorStateInfo(0).IsName("Roar"))
        {
            animator.SetTrigger("roar");
        }
    }

    void AttackPlayer()
    {
        animator.SetBool("isChasing", false);
        animator.SetBool("isAttacking", true);
        isChasing = false;
        isWandering = false;

        if (wanderCoroutine != null)
        {
            StopCoroutine(wanderCoroutine);
            wanderCoroutine = null;
        }

        lastKnownPlayerPosition = player.position;
    }

    void WanderAround()
    {
        animator.SetBool("isChasing", false);
        animator.SetBool("isAttacking", false);
        isChasing = false;

        if (!isWandering)
        {
            isWandering = true;
            animator.SetBool("isWandering", true);
            if (wanderCoroutine != null)
            {
                StopCoroutine(wanderCoroutine);
            }
            wanderCoroutine = StartCoroutine(Wander());
        }
    }

    IEnumerator Wander()
    {
        while (isWandering)
        {
            Vector3 randomDirection = GetRandomWanderPoint(transform.position, wanderRadius);

            if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, 10f, NavMesh.AllAreas))
            {
                if (agent != null && agent.isOnNavMesh)
                {
                    agent.SetDestination(hit.position);
                }
            }

            yield return new WaitUntil(() => agent.remainingDistance <= agent.stoppingDistance || Time.time > Time.time + 3f);
            yield return new WaitForSeconds(1f);
        }
    }

    Vector3 GetRandomWanderPoint(Vector3 origin, float dist)
    {
        Vector3 randDir = Random.insideUnitSphere * dist;
        randDir += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randDir, out navHit, 10f, NavMesh.AllAreas);
        return navHit.position;
    }

    void Die()
    {
        animator.SetBool("isWandering", false);
        animator.SetBool("isChasing", false);
        animator.SetBool("isAttacking", false);
        animator.SetBool("isDead", true);

        if (wanderCoroutine != null)
        {
            StopCoroutine(wanderCoroutine);
            wanderCoroutine = null;
        }

        if (agent != null)
        {
            agent.isStopped = true;
        }

        Destroy(gameObject, 5f);
    }
}