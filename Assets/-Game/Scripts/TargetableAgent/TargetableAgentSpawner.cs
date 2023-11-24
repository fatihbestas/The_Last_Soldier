using System.Collections;
using UnityEngine;

public class TargetableAgentSpawner : MonoBehaviour
{
    [SerializeField] private TargetableAgentPath[] paths = new TargetableAgentPath[9];

    private TargetableAgentPath path;
    private GameObject tempObject;

    private WaveData[] wavesData;
    private int wavesArraySize;
    private int pointsArraySize;

    private void OnEnable()
    {
        GameManager.OnLevelStart += StartSpawning;
        GameManager.OnLevelEnd += StopSpawning;
    }

    private void OnDisable()
    {
        GameManager.OnLevelStart -= StartSpawning;
        GameManager.OnLevelEnd -= StopSpawning;
    }


    private void StartSpawning()
    {
        StartCoroutine(Spawn());
    }

    private void StopSpawning(bool isLevelPassed)
    {
        StopAllCoroutines();
    }

    private IEnumerator Spawn()
    {
        yield return new WaitForSeconds(1);

        wavesData = LevelManager.currentLevelData.wavesData;
        wavesArraySize = wavesData.Length;

        for (int i = 0; i < wavesArraySize; i++)
        {
            pointsArraySize = wavesData[i].pointsData.Length;
            for (int j = 0; j < pointsArraySize; j++) 
            {
                tempObject = wavesData[i].pointsData[j].LevelObjectToSpawn.pool.GetPooledObject();
                tempObject.SetActive(true);
                path = paths[wavesData[i].pointsData[j].spawnPointIndex];
                tempObject.transform.position = path.spawnPoint;
                AllTargetableAgents.TargetableAgentGameObjectBond[tempObject].Initialize(path.targetPoint, wavesData[i].enemyMoveSpeed);
                
            }
            yield return new WaitForSeconds(wavesData[i].interval);
        }
    }

}