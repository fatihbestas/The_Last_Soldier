using NaughtyAttributes;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace LevelEditor
{
    public class SaveLevel : MonoBehaviour
    {
#if UNITY_EDITOR
        [Foldout("Fixed Variables")] public LevelEditor levelEditor;
        [Foldout("Fixed Variables")] public NewLevel NewLevel;

        [HideInInspector] public string levelDataPath = Path.Combine("Assets", "-Game", "GameData", "LevelsData");
        [HideInInspector] public string levelDataName = "Level";
        LevelData levelData;

        private void WriteLevelDataToSO()
        {
            levelData = ScriptableObject.CreateInstance<LevelData>();
            levelData.levelNumber = NewLevel.levelNumber;
            levelData.moneyReward = levelEditor.moneyReward;
            levelData.enemyHealth = levelEditor.enemyHealth;
            levelData.enemyFirePower = levelEditor.enemyFirePower;
            levelData.enemyShootSpeed = levelEditor.enemyShootSpeed;
            levelData.spawnIntervalStart = levelEditor.spawnIntervalStart;
            levelData.spawnIntervalEnd = levelEditor.spawnIntervalEnd;
            levelData.enemyMoveSpeedStart = levelEditor.enemyMoveSpeedStart;
            levelData.enemyMoveSpeedEnd = levelEditor.enemyMoveSpeedEnd;

            int waveCount = levelEditor.waveList.Count;
            levelData.wavesData = new WaveData[waveCount];

            for (int i = 0; i < levelData.wavesData.Length; i++)
            {
                levelData.wavesData[i] = new WaveData();
            }

            for (int i = 0; i < waveCount; i++)
            {
                levelData.wavesData[i].interval = levelEditor.waveList[i].interval;
                levelData.wavesData[i].enemyMoveSpeed = levelEditor.waveList[i].enemyMoveSpeed;

                // Only the data of the required points will be saved
                int pointCount = 0;
                for (int j = 0; j < levelEditor.waveList[i].pointsData.groundPointsArray.Length; j++)
                {
                    if (levelEditor.waveList[i].pointsData.groundPointsArray[j].LevelObjectToSpawn != null)
                    {
                        pointCount++;
                    }
                }
                for (int j = 0; j < levelEditor.waveList[i].pointsData.airPointsArray.Length; j++)
                {
                    if (levelEditor.waveList[i].pointsData.airPointsArray[j].LevelObjectToSpawn != null)
                    {
                        pointCount++;
                    }
                }

                levelData.wavesData[i].pointsData = new PointData[pointCount];

                for (int j = 0; j < levelData.wavesData[i].pointsData.Length; j++)
                {
                    levelData.wavesData[i].pointsData[j] = new PointData();
                }

                int index = 0;
                for (int j = 0; j < levelEditor.waveList[i].pointsData.groundPointsArray.Length; j++)
                {
                    if (levelEditor.waveList[i].pointsData.groundPointsArray[j].LevelObjectToSpawn != null)
                    {
                        levelData.wavesData[i].pointsData[index].spawnPointIndex = levelEditor.waveList[i].pointsData.groundPointsArray[j].spawnPointIndex;
                        levelData.wavesData[i].pointsData[index].LevelObjectToSpawn = levelEditor.waveList[i].pointsData.groundPointsArray[j].LevelObjectToSpawn;
                        index++;
                    }
                }
                for (int j = 0; j < levelEditor.waveList[i].pointsData.airPointsArray.Length; j++)
                {
                    if (levelEditor.waveList[i].pointsData.airPointsArray[j].LevelObjectToSpawn != null)
                    {
                        levelData.wavesData[i].pointsData[index].spawnPointIndex = levelEditor.waveList[i].pointsData.airPointsArray[j].spawnPointIndex;
                        levelData.wavesData[i].pointsData[index].LevelObjectToSpawn = levelEditor.waveList[i].pointsData.airPointsArray[j].LevelObjectToSpawn;
                        index++;
                    }
                }
            }

            List<LevelObjectToSpawn> differentObjects = new List<LevelObjectToSpawn>();
            foreach (WaveData waveData in levelData.wavesData)
            {
                foreach (PointData pointData in waveData.pointsData)
                {
                    if (!differentObjects.Contains(pointData.LevelObjectToSpawn))
                    {
                        differentObjects.Add(pointData.LevelObjectToSpawn);
                    }
                }
            }

            levelData.differentObjects = new LevelObjectToSpawn[differentObjects.Count];
            for (int i = 0; i < differentObjects.Count; i++)
            {
                levelData.differentObjects[i] = differentObjects[i];
            }

            int totalEnemyCount = 0;
            int totalHumanEnemyCount = 0;
            int totalAircraftCount = 0;
            int totalCivilianCount = 0;
            foreach (WaveData waveData in levelData.wavesData)
            {
                foreach (PointData pointData in waveData.pointsData)
                {
                    if (pointData.LevelObjectToSpawn.prefab.CompareTag("Enemy"))
                    {
                        totalEnemyCount++;
                        if(pointData.LevelObjectToSpawn.prefab.GetComponent<TargetableAgent>().IsHuman())
                        {
                            totalHumanEnemyCount++;
                        }
                        else
                        {
                            totalAircraftCount++;
                        }
                    }
                    else if(pointData.LevelObjectToSpawn.prefab.CompareTag("Civilian"))
                    {
                        totalCivilianCount++;
                    }
                    else
                    {
                        Debug.Log("Wrong Data");
                    }
                }
            }
            levelData.totalEnemyCount = totalEnemyCount;
            levelData.totalHumanEnemyCount = totalHumanEnemyCount;
            levelData.totalAircraftCount = totalAircraftCount;
            levelData.totalCivilianCount= totalCivilianCount;
        }

        [Button]
        public void Save()
        {
            // If an file already exists the path you specify it will be overwritten with your new file. 
            WriteLevelDataToSO();
            string relativeName = levelDataName + NewLevel.levelNumber.ToString() + ".asset";
            string relativePath = Path.Combine(levelDataPath, relativeName);
            AssetDatabase.CreateAsset(levelData, relativePath);
            Debug.Log("Level Saved");
        }
#endif
    }
}
