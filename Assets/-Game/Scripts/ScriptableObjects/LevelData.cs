using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "LevelName", menuName = "ScriptableObjects/LevelData")]
public class LevelData : ScriptableObject
{
    public int levelNumber;
    public int moneyReward;
    public float enemyHealth;
    public float enemyFirePower;
    public float enemyShootSpeed;

    [ReadOnly] public int totalEnemyCount;
    [ReadOnly] public int totalHumanEnemyCount;
    [ReadOnly] public int totalAircraftCount;
    [ReadOnly] public int totalCivilianCount;
    [ReadOnly] public float spawnIntervalStart;
    [ReadOnly] public float spawnIntervalEnd;
    [ReadOnly] public float enemyMoveSpeedStart;
    [ReadOnly] public float enemyMoveSpeedEnd;

    public WaveData[] wavesData;
    public LevelObjectToSpawn[] differentObjects;

#if UNITY_EDITOR
    private void OnDisable()
    {
        if(enemyShootSpeed <= 0 || enemyFirePower <= 0 || enemyHealth <= 0 ||
            spawnIntervalStart <= 0 || spawnIntervalEnd <= 0 || enemyMoveSpeedStart <= 0 || enemyMoveSpeedEnd <= 0)
        {
            Debug.LogError("Level data is wrong. Level Number: " + levelNumber + "Name: " + name);
        }
    }
#endif

}

[System.Serializable]
public class WaveData 
{
    public PointData[] pointsData;
    public float interval;
    public float enemyMoveSpeed;
}

[System.Serializable]
public class PointData 
{
    public int spawnPointIndex;
    public LevelObjectToSpawn LevelObjectToSpawn;
}


