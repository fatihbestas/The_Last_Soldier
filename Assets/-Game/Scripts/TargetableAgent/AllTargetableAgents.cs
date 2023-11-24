using System.Collections.Generic;
using UnityEngine;

public class AllTargetableAgents : MonoBehaviour
{

    public static Dictionary<Collider, TargetableAgent> TargetableAgentColliderBond = new Dictionary<Collider, TargetableAgent>();
    public static Dictionary<GameObject, TargetableAgent> TargetableAgentGameObjectBond = new Dictionary<GameObject, TargetableAgent>();

}
