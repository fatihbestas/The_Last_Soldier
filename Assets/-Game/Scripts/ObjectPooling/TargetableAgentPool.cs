using UnityEngine;

public class TargetableAgentPool : ObjectPool
{
    TargetableAgent agent;
    protected override void AddObjectToDictionaries(GameObject gameObject)
    {
        agent = gameObject.GetComponent<TargetableAgent>();
        AllTargetableAgents.TargetableAgentColliderBond.Add(agent.myCollider, agent);
        AllTargetableAgents.TargetableAgentGameObjectBond.Add(gameObject, agent);        
    }
}
