using UnityEngine;

public class HitBehavior : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Hiệu ứng hit (máu, âm thanh,...)
        Debug.Log("Quái vật bị đánh trúng!");

        // Có thể thêm WaitForSeconds thông qua coroutine nếu cần
    }
}