using Unity.Behavior;
using UnityEngine;

public class MonsterAi : MonoBehaviour
{
    public Transform characterA;
    public Transform characterB;
    public LayerMask obstacleMask;

    public CanPlayerSeeMeCondition canSeePlayer;
    public BehaviorGraphAgent blackboard;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //blackboard.GetVariable()
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 start = characterA.position + Vector3.up * 1.6f; // eye height
        Vector3 end   = characterB.position + Vector3.up * 1.6f;

        Debug.DrawLine(start, end, Color.red);
        float radius = 0.2f;
        RaycastHit hit;
        if (Physics.SphereCast(start, radius, (end - start).normalized, out  hit, Vector3.Distance(start, end), obstacleMask))
        {
            Debug.Log("Blocked by: " + hit.collider.name);
           blackboard.SetVariableValue("CanPLayerSeeMe", false);
           
        }
        else
        {
           // canSeePlayer.SeesMe.Value = true;
            Debug.Log("Clear line of sight");
            blackboard.SetVariableValue("CanPLayerSeeMe", true);
        }
        

    }
}
