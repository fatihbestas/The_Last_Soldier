using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject patrol;
    [SerializeField] private GameObject bakedBase;
    [SerializeField] private float fogDensityStart;
    [SerializeField] private float fogDensity;
    [SerializeField] private int civilianMaxHitCount;

    private int civilianCurrentHitCount;
    private int totalEnemyCount;
    private int killedEnemyCount;

    public static event Action GamePlayStarted;
    public static event Action OnLevelStart;
    public static event Action<bool> OnLevelEnd;
    public static event Action OnTutorialStart;

    private void OnEnable()
    {
        UIManager.StartButtonPressed += StartGamePlay;
        TargetableAgent.TargetableAgentGetHit += OnTargetableAgentGetHit;
        PlayerHealth.PlayerDied += LevelFailed;
        UIManager.PlayAgainButtonPressed += StartTutorial;
        UIManager.ContinueButtonPressed += StartTutorial;
        Tutorial.TutorialEnded += StartLevel;
    }

    private void OnDisable()
    {
        UIManager.StartButtonPressed -= StartGamePlay;
        TargetableAgent.TargetableAgentGetHit -= OnTargetableAgentGetHit;
        PlayerHealth.PlayerDied -= LevelFailed;
        UIManager.PlayAgainButtonPressed -= StartTutorial;
        UIManager.ContinueButtonPressed -= StartTutorial;
        Tutorial.TutorialEnded -= StartLevel;
    }

    private void Start()
    {
        bakedBase.SetActive(true);
        patrol.SetActive(true);
        RenderSettings.fogDensity = fogDensityStart;
    }

    public void StartGamePlay()
    {
        GamePlayStarted?.Invoke();
        StartCoroutine(SwitchBaseAndStartLevel());
    }

    public void StartTutorial()
    {
        OnTutorialStart?.Invoke();
    }

    private void StartLevel()
    {
        killedEnemyCount = 0;
        totalEnemyCount = LevelManager.currentLevelData.totalEnemyCount;
        civilianCurrentHitCount = 0;
        OnLevelStart?.Invoke();
    }

    private void LevelCompleted()
    {
        OnLevelEnd?.Invoke(true);
        PlayerPrefs.Save();
    }

    private void LevelFailed()
    {
        OnLevelEnd?.Invoke(false);
    }

    public void OnTargetableAgentGetHit(TargetableAgentHitInfo hitInfo)
    {
        if(hitInfo.isDied)
        {
            if(hitInfo.isEnemy) EnemyDied();
            else CivilianDied();
        }
    }

    private void EnemyDied()
    {
        killedEnemyCount++;
        if (killedEnemyCount >= totalEnemyCount)
        {
            LevelCompleted();
        }
    }

    private void CivilianDied()
    {
        civilianCurrentHitCount++;
        if (civilianCurrentHitCount >= civilianMaxHitCount)
        {
            LevelFailed();
        }
    }

    private IEnumerator SwitchBaseAndStartLevel()
    {
        yield return new WaitForSeconds(0.5f);
        RenderSettings.fogDensity = fogDensity;
        yield return new WaitForSeconds(0.5f);
        bakedBase.SetActive(false);
        patrol.SetActive(false);
        StartTutorial();
    }
}
