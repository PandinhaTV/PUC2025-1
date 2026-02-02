using UnityEngine;

public class CollisionWithPlayer : MonoBehaviour
{
    public float detectRange = 20f;
    public LayerMask detectionLayers; // Set this to include "Player" and "Obstacles"
    
    void Update()
    {
        // Define the ray starting point and direction
        Vector3 rayOrigin = transform.position;
        Vector3 rayDirection = transform.forward;

        RaycastHit hit;

        // Cast the ray
        if (Physics.Raycast(rayOrigin, rayDirection, out hit, detectRange, detectionLayers))
        {
            // Check if the object hit has the "Player" tag
            if (hit.collider.CompareTag("Player"))
            {
                Debug.Log("I see the player!");
                SceneController.Instance
                    .NewTransition()
                    .Load(SceneDatabase.Slots.Session, SceneDatabase.Scenes.Room3)
                    .WithOverlay()
                    .Perform();
            
                Debug.DrawRay(rayOrigin, rayDirection * hit.distance, Color.green);
            }
            else
            {
                // If it hits a wall/obstacle instead
                Debug.Log("Wall is in the way.");
                Debug.DrawRay(rayOrigin, rayDirection * hit.distance, Color.red);
            }
        }
        else
        {
            // If the ray hits nothing within range
            Debug.DrawRay(rayOrigin, rayDirection * detectRange, Color.white);
        }
    }
    
        
}
