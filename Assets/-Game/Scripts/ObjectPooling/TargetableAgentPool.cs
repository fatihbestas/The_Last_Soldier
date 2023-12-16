using UnityEngine;

public class TargetableAgentPool : ObjectPool<TargetableAgent>
{
    TargetableAgent agent;
    protected override void AddObjectToDictionary(GameObject gameObject)
    {
        agent = gameObject.GetComponent<TargetableAgent>();
        AllTargetableAgents.TargetableAgentColliderBond.Add(agent.myCollider, agent);
    }
}
