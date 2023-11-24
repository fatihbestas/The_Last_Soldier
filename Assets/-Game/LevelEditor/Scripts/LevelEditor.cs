using NaughtyAttributes;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace LevelEditor
{
    public class LevelEditor : MonoBehaviour
    {
#if UNITY_EDITOR

        [Header("General Settings")]
        public int moneyReward;
        public float enemyHealth;
        public float enemyFirePower;
        public float enemyShootSpeed;

        [Header("Wave Settings")]
        public int waveCount;

        [Tooltip("Spawn interval will decrease linearly from the beginning of the level to the end of the level. " +
            "But you can manually change the spawn interval value of any wave you want.")]
        public float spawnIntervalStart;

        [Tooltip("Enter a positive number that is less than SpawnIntervalStart.")]
        public float spawnIntervalEnd;

        [Tooltip("Enemy Move speed will increase linearly from the beginning of the level to the end of the level. " +
            "But you can manually change the enemy move speed value of any wave you want.")]
        public float enemyMoveSpeedStart;

        [Tooltip("Enter a positive number that is greater than EnemyMoveSpeedStart.")]
        public float enemyMoveSpeedEnd;

        [Foldout("Fixed Variables")] public List<LevelEditorWaveData> waveList = new List<LevelEditorWaveData>();

        [Foldout("Fixed Variables")] public Transform wavesParent;
        [Foldout("Fixed Variables")] public GameObject wavePrefab;
        [Foldout("Fixed Variables")] public TextMeshPro totalLevelDurationText;
        [Foldout("Fixed Variables")] public Material[] materials;

        public void NewLevel()
        {
            int size = wavesParent.childCount;

            for (int i = size - 1; i > -1; i--)
            {
                DestroyImmediate(wavesParent.GetChild(i).gameObject);
            }

            waveList.Clear();
            UpdateTotalDuration();
        }

        [Button]
        public void UpdateWaves()
        {
            if (waveCount <= 0)
            {
                Debug.LogError("waveCount must be positive number");
                return;
            }

            if (waveCount < wavesParent.childCount)
            {
                Debug.LogWarning("The wave number cannot be reduced for now. This feature will be added later.");
                return;
            }

            for (int i = wavesParent.childCount; i < waveCount; i++)
            {
                GameObject wave = Instantiate(wavePrefab, wavesParent);
                wave.transform.position = new Vector3(0, 0, 10 + i * 4);
                wave.name = "Wave_" + i.ToString();
                LevelEditorWaveData waveData = wave.GetComponent<LevelEditorWaveData>();
                waveData.waveIndex = i;
                waveData.pointsData.Clear();

                waveList.Add(waveData);
            }

            for (int i = 0; i < waveList.Count; i++)
            {
                waveList[i].planeMesh.material = materials[i % materials.Length];
            }

            UpdateAllIntervalValues();
            UpdateAllMoveSpeedValues();
        }

        public void UpdateAllIntervalValues()
        {
            float interval = spawnIntervalStart;
            float intervalDecreaseRate = (spawnIntervalStart - spawnIntervalEnd) / (waveCount - 1 > 0 ? waveCount - 1 : 1);

            foreach (LevelEditorWaveData item in waveList)
            {
                if (!item.intervalChangedManualy)
                    item.interval = interval - intervalDecreaseRate * item.waveIndex;

                if (item.interval < 0)
                {
                    Debug.LogError("Spawn Interwal value must ve positive float value");
                }

                item.UpdateTexts();
            }

            UpdateTotalDuration();
        }

        public void UpdateAllMoveSpeedValues()
        {
            float moveSpeed = enemyMoveSpeedStart;
            float increaseRate = (enemyMoveSpeedEnd - enemyMoveSpeedStart) / (waveCount - 1 > 0 ? waveCount - 1 : 1);

            foreach(LevelEditorWaveData item in waveList)
            {
                if (!item.moveSpeedChangedManualy)
                    item.enemyMoveSpeed = moveSpeed + increaseRate * item.waveIndex;

                if (item.enemyMoveSpeed < 0)
                {
                    Debug.LogError("Enemy move speed must ve positive float value");
                }

                item.UpdateTexts();
            }
        }

        public void UpdateTotalDuration()
        {
            float totalDuration = 0;

            foreach (LevelEditorWaveData item in waveList)
            {
                totalDuration += item.interval;
            }

            totalLevelDurationText.text = "Approximate level duration: " + totalDuration.ToString("F2") + " seconds.";
        }
#endif
    }
}