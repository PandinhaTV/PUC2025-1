using UnityEngine;

public class ResetLvl : MonoBehaviour
{



    public Vector3 teleportPosition;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        CharacterController controller = other.GetComponent<CharacterController>();
        if (controller == null)
            return;

        // Disable controller to prevent snapping issues
        controller.enabled = false;
        other.transform.position = teleportPosition;
        controller.enabled = true;
    }


}


