using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class NvrAutowalk : MonoBehaviour
{
    // Tốc độ di chuyển
    public float speed = 3.0F;

    // CharacterController và Camera
    private CharacterController myCC;
    private Transform vrCamera;

    void Start()
    {

        Input.gyro.enabled = true;
       
        myCC = GetComponent<CharacterController>();
        vrCamera = Camera.main.transform;
    }

    void Update()
    {
        // Lấy góc pitch (góc cúi đầu)
        float pitch = vrCamera.eulerAngles.x;
        Vector3 direction = vrCamera.forward;
        direction.y = 0f;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }

        // Xử lý góc vượt quá 180 (chuyển về -180 ~ 180)
        if (pitch > 180f) pitch -= 360f;

        // Nếu cúi đầu ≥ 30 độ thì mới di chuyển
        if (pitch >= 30f)
        {
            Vector3 forward = vrCamera.forward;
            forward.y = 0f; // Không bay lên
            forward.Normalize();



            myCC.SimpleMove(forward * speed);
        }
    }
}
