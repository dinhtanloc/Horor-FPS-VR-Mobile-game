using UnityEngine;
using UnityEngine.AI;

public class WanderBehavior : StateMachineBehaviour
{
    private NavMeshAgent agent;
    private Transform target;
    private Vector3 destination;
    public float wanderRadius = 10f;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        target = animator.GetComponent<Transform>();

        Vector3 randomDir = Random.insideUnitSphere * wanderRadius;
        NavMeshHit hit;
        NavMesh.SamplePosition(target.position + randomDir, out hit, wanderRadius, NavMesh.AllAreas);
        destination = hit.position;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Vector3.Distance(animator.transform.position, destination) > agent.stoppingDistance)
        {
            agent.SetDestination(destination);
        }
        else
        {
            animator.SetBool("isWandering", false);
        }
    }
}