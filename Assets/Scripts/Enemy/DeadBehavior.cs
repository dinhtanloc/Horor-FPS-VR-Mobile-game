using UnityEngine;
using UnityEngine.AI;

public class DeadBehavior : StateMachineBehaviour
{
    public float destroyDelay = 5f;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Rigidbody rb = animator.GetComponent<Rigidbody>();
        if (rb)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        Collider col = animator.GetComponent<Collider>();
        if (col)
        {
            col.enabled = false;
        }

        animator.GetComponent<NavMeshAgent>().isStopped = true;

        // Tự hủy sau một khoảng thời gian
        Object.Destroy(animator.gameObject, destroyDelay);
    }
}