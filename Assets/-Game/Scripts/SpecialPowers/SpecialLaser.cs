using UnityEngine;

public class SpecialLaser : MonoBehaviour
{
    public float MaxLength;

    private LayerMask TargetableAgentLayer;
    private TargetableAgent agent;
    RaycastHit hit;

    void Start ()
    {
        TargetableAgentLayer = LayerMask.GetMask("TargetableAgent");
    }

    void Update()
    {
   
        if (Physics.Raycast(transform.position, transform.forward, out hit, MaxLength, TargetableAgentLayer))
        {
            agent = AllTargetableAgents.TargetableAgentColliderBond[hit.collider];
            agent.TakeDamageByLaser();
        }

    }
}
