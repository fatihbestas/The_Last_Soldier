using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObjects/EnemyData")]
public class EnemyData : ScriptableObject
{
    public float healthMultiplier;
    public TargetableAgent.AgentType enemyType;

#if UNITY_EDITOR
    private void OnDisable()
    {
        if(healthMultiplier <= 0)
        {
            Debug.LogError("Enemy Data is wrong. File Name: " + name);
        }
    }
#endif

}
