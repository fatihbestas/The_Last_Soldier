using System.Collections;
using UnityEngine;

public class TargetableAgentSpawner : MonoBehaviour
{
    [SerializeField] private TargetableAgentPath[] paths = new TargetableAgentPath[9];

    private TargetableAgentPath path;
    private TargetableAgent tempAgent;

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
                tempAgent = wavesData[i].pointsData[j].LevelObjectToSpawn.pool.GetPooledComponent();
                tempAgent.gameObject.SetActive(true);
                path = paths[wavesData[i].pointsData[j].spawnPointIndex];
                tempAgent.transform.position = path.spawnPoint;
                tempAgent.Initialize(path.targetPoint, wavesData[i].enemyMoveSpeed);

            }
            yield return new WaitForSeconds(wavesData[i].interval);
        }
    }

}