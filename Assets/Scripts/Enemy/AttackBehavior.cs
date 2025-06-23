using UnityEngine;

public class AttackBehavior : StateMachineBehaviour
{
    private Transform player;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (player == null) return;

        float distance = Vector3.Distance(animator.transform.position, player.position);

        if (distance > 2f)
        {
            animator.SetBool("isAttacking", false);
        }
    }
}