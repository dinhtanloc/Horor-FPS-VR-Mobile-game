using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField]
    private Door Door;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<CharacterController>(out CharacterController controller))
        {
            if (Door != null) // Thêm kiểm tra null
            {
                if (!Door.IsOpen)
                {
                    // Debug.Log("Opening door as player entered trigger area.");
                    Door.Open(other.transform.position);
                }
            }
            else
            {
                Debug.LogError("Door is not assigned in the Inspector!");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<CharacterController>(out CharacterController controller))
        {
            if (Door.IsOpen)
            {
                // Debug.Log("Closing door as player exited trigger area.");
                Door.Close();
            }
        }
    }
}
