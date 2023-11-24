using UnityEngine;

[CreateAssetMenu(fileName = "Civilian", menuName = "ScriptableObjects/CivilianData")]
public class CivilianData : ScriptableObject
{
    public TargetableAgent.AgentType civilianType;
    public string idleAnimTrigger;
    public string runAnimTrigger;
    public string[] deathAnimTrigger;

    public string GetDeathAnimTrigger()
    {
        return deathAnimTrigger[Random.Range(0, deathAnimTrigger.Length)];
    }
}
