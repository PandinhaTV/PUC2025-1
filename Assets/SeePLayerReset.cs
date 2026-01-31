using UnityEngine;

public class SeePLayerReset : MonoBehaviour
{
    public GameObject player;
    public void TriggerSomething()
    {
        SceneController.Instance
            .NewTransition()
            .Load(SceneDatabase.Slots.Session, SceneDatabase.Scenes.Room3)
            .WithOverlay()
            .Perform();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckPlayerVisible();
        
    }
    void CheckPlayerVisible()
    {
        Vector3 dirToPlayer = (player.transform.position - transform.position).normalized;
        float dot = Vector3.Dot(transform.forward, dirToPlayer);

        if (dot > 0.7f)
        {
            if (Physics.Raycast(transform.position, dirToPlayer, out RaycastHit hit))
            {
                if (hit.transform == player.transform)
                {
                    // Enemy sees the player
                    TriggerSomething();
                }
            }
        }
    }
}
