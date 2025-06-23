using UnityEngine;

public class DoubleDoorTrigger : MonoBehaviour
{
    [SerializeField] private MultiDoor leftDoor;
    [SerializeField] private MultiDoor rightDoor;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<CharacterController>(out CharacterController controller))
        {
            // Debug.Log("Open the door");
            OpenBothDoors();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<CharacterController>(out CharacterController controller))
        {
            CloseBothDoors();
        }
    }

    private void OpenBothDoors()
    {
        if (leftDoor != null && !leftDoor.IsOpen)
        {
            leftDoor.Open(transform.position);
        }

        if (rightDoor != null && !rightDoor.IsOpen)
        {
            rightDoor.Open(transform.position);
        }
    }

    private void CloseBothDoors()
    {
        if (leftDoor != null && leftDoor.IsOpen)
        {
            leftDoor.Close();
        }

        if (rightDoor != null && rightDoor.IsOpen)
        {
            rightDoor.Close();
        }
    }
}


