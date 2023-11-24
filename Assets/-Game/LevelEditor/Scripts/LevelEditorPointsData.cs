using UnityEngine;
using NaughtyAttributes;
using UnityEditor;
using System.Collections.Generic;

namespace LevelEditor
{
    public class LevelEditorPointsData : MonoBehaviour
    {
#if UNITY_EDITOR
        [Header("Ground Points")]
        [Dropdown("GetGroundValues")] public LevelObjectToSpawn groundPoint1;
        [Dropdown("GetGroundValues")] public LevelObjectToSpawn groundPoint2;
        [Dropdown("GetGroundValues")] public LevelObjectToSpawn groundPoint3;
        [Dropdown("GetGroundValues")] public LevelObjectToSpawn groundPoint4;
        [Dropdown("GetGroundValues")] public LevelObjectToSpawn groundPoint5;
        [Dropdown("GetGroundValues")] public LevelObjectToSpawn groundPoint6;
        [Dropdown("GetGroundValues")] public LevelObjectToSpawn groundPoint7;
        [Dropdown("GetGroundValues")] public LevelObjectToSpawn groundPoint8;
        [Dropdown("GetGroundValues")] public LevelObjectToSpawn groundPoint9;

        [Header("Air Points")]
        [Dropdown("GetAirValues")] public LevelObjectToSpawn airPoint1;
        [Dropdown("GetAirValues")] public LevelObjectToSpawn airPoint2;
        [Dropdown("GetAirValues")] public LevelObjectToSpawn airPoint3;
        [Dropdown("GetAirValues")] public LevelObjectToSpawn airPoint4;
        [Dropdown("GetAirValues")] public LevelObjectToSpawn airPoint5;
        [Dropdown("GetAirValues")] public LevelObjectToSpawn airPoint6;
        [Dropdown("GetAirValues")] public LevelObjectToSpawn airPoint7;
        [Dropdown("GetAirValues")] public LevelObjectToSpawn airPoint8;

        [Foldout("Fixed Variables")] public LevelEditorPoint[] groundPointsArray = new LevelEditorPoint[9];
        [Foldout("Fixed Variables")] public LevelEditorPoint[] airPointsArray = new LevelEditorPoint[8];
        [Foldout("Fixed Variables")] public LevelObjectToSpawn[] levelObjectToSpawns;
        [Foldout("Fixed Variables")] public Transform visualObjectParent;


        private DropdownList<LevelObjectToSpawn> GetGroundValues() // NaughtyAttributes Dropdown
        {
            DropdownList<LevelObjectToSpawn> list = new DropdownList<LevelObjectToSpawn>();

            list.Add("None", null);

            for (int i = 0; i < levelObjectToSpawns.Length; i++)
            {
                if (levelObjectToSpawns[i].prefab.TryGetComponent(out TargetableAgent targetableAgent))
                {
                    if (targetableAgent.IsHuman())
                        list.Add(levelObjectToSpawns[i].prefab.gameObject.name, levelObjectToSpawns[i]);
                }
                else
                {
                    Debug.LogError(levelObjectToSpawns[i].name + "data is wrong");
                    list.Add("Wrong Data", levelObjectToSpawns[i]);
                }
            }

            return list;
        }

        private DropdownList<LevelObjectToSpawn> GetAirValues() // NaughtyAttributes Dropdown
        {
            DropdownList<LevelObjectToSpawn> list = new DropdownList<LevelObjectToSpawn>();

            list.Add("None", null);

            for (int i = 0; i < levelObjectToSpawns.Length; i++)
            {
                if (levelObjectToSpawns[i].prefab.TryGetComponent(out TargetableAgent targetableAgent))
                {
                    if (!targetableAgent.IsHuman())
                        list.Add(levelObjectToSpawns[i].prefab.gameObject.name, levelObjectToSpawns[i]);
                }
                else
                {
                    Debug.LogError(levelObjectToSpawns[i].name + "data is wrong");
                    list.Add("Wrong Data", levelObjectToSpawns[i]);
                }
            }

            return list;
        }

        private void ClearVisualObjects()
        {
            int size = visualObjectParent.childCount;

            for (int i = size - 1; i > -1; i--)
            {
                DestroyImmediate(visualObjectParent.GetChild(i).gameObject);
            }
        }

        private void UpdateVisualObjects()
        {
            ClearVisualObjects();

            foreach (LevelEditorPoint item in groundPointsArray)
            {
                if (item.LevelObjectToSpawn != null)
                    Instantiate(item.LevelObjectToSpawn.prefab, visualObjectParent).transform.position = item.transform.position;
            }

            foreach (LevelEditorPoint item in airPointsArray)
            {
                if (item.LevelObjectToSpawn != null)
                    Instantiate(item.LevelObjectToSpawn.prefab, visualObjectParent).transform.position = item.transform.position;
            }
        }

        [Button]
        public void Clear()
        {
            ClearVisualObjects();

            groundPoint1 = null; groundPoint2 = null; groundPoint3 = null;
            groundPoint4 = null; groundPoint5 = null; groundPoint6 = null;
            groundPoint7 = null; groundPoint8 = null; groundPoint9 = null;
            airPoint1 = null; airPoint2 = null; airPoint3 = null; airPoint4 = null;
            airPoint5 = null; airPoint6 = null; airPoint7 = null; airPoint8 = null;

            for (int i = 0; i < groundPointsArray.Length; i++)
            {
                groundPointsArray[i].LevelObjectToSpawn = null;
            }

            for (int i = 0; i < airPointsArray.Length; i++)
            {
                airPointsArray[i].LevelObjectToSpawn = null;
            }

            CheckLevelObjectToSpawnData();
        }

        [Button]
        public void Save()
        {
            groundPointsArray[0].LevelObjectToSpawn = groundPoint1;
            groundPointsArray[1].LevelObjectToSpawn = groundPoint2;
            groundPointsArray[2].LevelObjectToSpawn = groundPoint3;
            groundPointsArray[3].LevelObjectToSpawn = groundPoint4;
            groundPointsArray[4].LevelObjectToSpawn = groundPoint5;
            groundPointsArray[5].LevelObjectToSpawn = groundPoint6;
            groundPointsArray[6].LevelObjectToSpawn = groundPoint7;
            groundPointsArray[7].LevelObjectToSpawn = groundPoint8;
            groundPointsArray[8].LevelObjectToSpawn = groundPoint9;

            airPointsArray[0].LevelObjectToSpawn = airPoint1;
            airPointsArray[1].LevelObjectToSpawn = airPoint2;
            airPointsArray[2].LevelObjectToSpawn = airPoint3;
            airPointsArray[3].LevelObjectToSpawn = airPoint4;
            airPointsArray[4].LevelObjectToSpawn = airPoint5;
            airPointsArray[5].LevelObjectToSpawn = airPoint6;
            airPointsArray[6].LevelObjectToSpawn = airPoint7;
            airPointsArray[7].LevelObjectToSpawn = airPoint8;

            UpdateVisualObjects();
        }

        public void OverwritePointsData()
        {
            groundPoint1 = groundPointsArray[0].LevelObjectToSpawn;
            groundPoint2 = groundPointsArray[1].LevelObjectToSpawn;
            groundPoint3 = groundPointsArray[2].LevelObjectToSpawn;
            groundPoint4 = groundPointsArray[3].LevelObjectToSpawn;
            groundPoint5 = groundPointsArray[4].LevelObjectToSpawn;
            groundPoint6 = groundPointsArray[5].LevelObjectToSpawn;
            groundPoint7 = groundPointsArray[6].LevelObjectToSpawn;
            groundPoint8 = groundPointsArray[7].LevelObjectToSpawn;
            groundPoint9 = groundPointsArray[8].LevelObjectToSpawn;

            airPoint1 = airPointsArray[0].LevelObjectToSpawn;
            airPoint2 = airPointsArray[1].LevelObjectToSpawn;
            airPoint3 = airPointsArray[2].LevelObjectToSpawn;
            airPoint4 = airPointsArray[3].LevelObjectToSpawn;
            airPoint5 = airPointsArray[4].LevelObjectToSpawn;
            airPoint6 = airPointsArray[5].LevelObjectToSpawn;
            airPoint7 = airPointsArray[6].LevelObjectToSpawn;
            airPoint8 = airPointsArray[7].LevelObjectToSpawn;

            UpdateVisualObjects();
        }

        [Button]
        public void SaveAndNextWave()
        {
            Save();
            int index = GetComponent<LevelEditorWaveData>().waveIndex + 1;
            Selection.activeObject = GameObject.Find("Wave_" + index.ToString());
            SceneView.FrameLastActiveSceneView();

        }

        private void CheckLevelObjectToSpawnData()
        {
            List<int> IDs = new List<int>();

            foreach (LevelObjectToSpawn item in levelObjectToSpawns)
            {
                if (IDs.Contains(item.id))
                    Debug.LogError("All LevelObjectToSpawn ScriptableObjects must have different IDs. Also ScriptableObject id and its prefab id must be same.");
                IDs.Add(item.id);
            }
        }
#endif
    }
}