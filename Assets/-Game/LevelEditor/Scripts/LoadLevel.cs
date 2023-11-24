using NaughtyAttributes;
using UnityEngine;

namespace LevelEditor
{
    public class LoadLevel : MonoBehaviour
    {
#if UNITY_EDITOR
        [Foldout("Fixed Variables")] public NewLevel newLevel;
        [Foldout("Fixed Variables")] public LevelEditor levelEditor;

        public LevelData levelData;

        [Button]
        public void Load()
        {
            newLevel.levelNumber = levelData.levelNumber;
            newLevel.CreateNewLevel();
            levelEditor.waveCount = levelData.wavesData.Length;
            levelEditor.moneyReward = levelData.moneyReward;
            levelEditor.enemyHealth = levelData.enemyHealth;
            levelEditor.enemyFirePower = levelData.enemyFirePower;
            levelEditor.enemyShootSpeed = levelData.enemyShootSpeed;
            levelEditor.spawnIntervalStart = levelData.spawnIntervalStart;
            levelEditor.spawnIntervalEnd = levelData.spawnIntervalEnd;
            levelEditor.enemyMoveSpeedStart = levelData.enemyMoveSpeedStart;
            levelEditor.enemyMoveSpeedEnd = levelData.enemyMoveSpeedEnd;
            levelEditor.UpdateWaves();

            for (int i = 0; i < levelData.wavesData.Length; i++)
            {
                if (levelData.wavesData[i].interval != levelEditor.waveList[i].interval)
                {
                    levelEditor.waveList[i].interval = levelData.wavesData[i].interval;
                    levelEditor.waveList[i].UpdateIntervalValue();
                }

                if (levelData.wavesData[i].enemyMoveSpeed != levelEditor.waveList[i].enemyMoveSpeed)
                {
                    levelEditor.waveList[i].enemyMoveSpeed = levelData.wavesData[i].enemyMoveSpeed;
                    levelEditor.waveList[i].UpdateEnemyMoveSpeed();
                }

                for (int j = 0; j < levelData.wavesData[i].pointsData.Length; j++)
                {
                    int index = levelData.wavesData[i].pointsData[j].spawnPointIndex;
                    if (index < 9)
                    {
                        levelEditor.waveList[i].pointsData.groundPointsArray[index].LevelObjectToSpawn = levelData.wavesData[i].pointsData[j].LevelObjectToSpawn;
                    }
                    else if(index < 17) 
                    {
                        levelEditor.waveList[i].pointsData.airPointsArray[index].LevelObjectToSpawn = levelData.wavesData[i].pointsData[j].LevelObjectToSpawn;
                    }
                    else
                    {
                        Debug.LogError("Spawn Point index error.");
                    }
                    levelEditor.waveList[i].pointsData.OverwritePointsData();

                }
            }
        }
#endif
    }

}
