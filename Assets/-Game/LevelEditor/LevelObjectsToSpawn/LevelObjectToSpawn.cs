using UnityEngine;

[CreateAssetMenu(fileName = "LevelObjectToSpawn", menuName = "ScriptableObjects/LevelObjectToSpawn")]
public class LevelObjectToSpawn : ScriptableObject
{
    public short id;
    public GameObject prefab;
    public TargetableAgentPool pool;


#if UNITY_EDITOR
    private void OnDisable()
    {
        if (prefab.TryGetComponent(out TargetableAgent targetableAgent))
        {
            if(id != targetableAgent.id)
                Debug.LogError("ObjectToSpawn ScriptableObject id data or prefab id data is wrong. Name: " + name);
        }
        else
        {
            Debug.LogError("Prefab data is wrong");
        }
    }
#endif
}
