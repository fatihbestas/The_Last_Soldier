using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField] private LevelData testLevel;
#endif    
    [SerializeField] private LevelData[] levelsData;
    [SerializeField] private Transform TargetableAgentPoolsParent;
    [SerializeField] private GameObject TargetableAgentPoolPrefab;

    public static LevelData currentLevelData;
    private int currentLevelIndex;
    private int levelIndexWhenGameOpened;
    private List<short> createdPoolIDs = new List<short>();

    public static event Action<int> LevelPassed;

    private void Awake()
    {
        GetLevelIndexFromDisk();
#if UNITY_EDITOR
        if(testLevel != null)
            currentLevelData = testLevel;
        else
            currentLevelData = levelsData[currentLevelIndex];
#else
        currentLevelData = levelsData[currentLevelIndex];
#endif
        levelIndexWhenGameOpened = currentLevelIndex;
        CreateTargetableAgentPools();
    }

#if UNITY_EDITOR

    private void Start()
    {
        for (int i = 0; i < levelsData.Length; i++)
        {
            if (i + 1 != levelsData[i].levelNumber)
            {
                Debug.LogError("LevelsData objects order is wrong. GameObject name: " + gameObject.name);
            }
        }
    }

#endif

    private void OnEnable()
    {
        GameManager.OnLevelEnd += OnLevelEnd;
    }

    private void OnDisable()
    {
        GameManager.OnLevelEnd -= OnLevelEnd;
    }

    private void OnLevelEnd(bool isLevelPassed)
    {
        
        if (!isLevelPassed)
            return;

        if((currentLevelIndex + 1) < levelsData.Length)
        {
            LevelPassed?.Invoke(currentLevelData.moneyReward);
            currentLevelIndex++;
            currentLevelData = levelsData[currentLevelIndex];
            PlayerPrefs.SetInt("currentLevelIndex", currentLevelIndex);
        }

        if(currentLevelIndex >= levelIndexWhenGameOpened + 5)
        {
            StartCreatingNewPools();
        }
    }

    private void GetLevelIndexFromDisk() 
    {
        currentLevelIndex = PlayerPrefs.GetInt("currentLevelIndex", 0);
    }
    
    private void CreateTargetableAgentPools()
    {
        // Create object pools for first five levels
        
        for (int i = currentLevelIndex; i <= currentLevelIndex + 5; i++)
        {
            if (levelsData.Length <= i)
                break;

            foreach (LevelObjectToSpawn levelObjectToSpawn in levelsData[i].differentObjects)
            {
                if(!createdPoolIDs.Contains(levelObjectToSpawn.id))
                {
                    createdPoolIDs.Add(levelObjectToSpawn.id);
                    levelObjectToSpawn.pool = Instantiate(TargetableAgentPoolPrefab, TargetableAgentPoolsParent).GetComponent<TargetableAgentPool>();
                    levelObjectToSpawn.pool.objectToPool = levelObjectToSpawn.prefab;
                    levelObjectToSpawn.pool.CreatePool();

                }
            }
        }
        
    }

    private void StartCreatingNewPools()
    {
        // After passing five levels, check if a new object pool is required.
        // Start creating if necessary.
        // Create only one object per frame.
        throw new NotImplementedException();
    }

}
