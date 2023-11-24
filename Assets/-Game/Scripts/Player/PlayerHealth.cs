using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth;

    private float health;

    public static event Action<float> LevelStarted;
    public static event Action<float> HealthDecreased;
    public static event Action PlayerDied;

    private void Awake()
    {
        maxHealth = PlayerPrefs.GetFloat("health", maxHealth);
    }

    private void OnEnable()
    {
        GameManager.OnLevelStart += OnLevelStart;
        Enemy.HitPlayer += GetHit;
        TargetableAgent.TargetableAgentGetHit += OnTargetableAgentGetHit;
    }

    private void OnDisable()
    {
        GameManager.OnLevelStart -= OnLevelStart;
        Enemy.HitPlayer -= GetHit;
        TargetableAgent.TargetableAgentGetHit -= OnTargetableAgentGetHit;
    }

    private void OnLevelStart()
    {
        health = maxHealth;
        LevelStarted?.Invoke(health);
    }

    private void GetHit(float damage)
    {
        if(health <= 0) return; // If multiple hits are executed in the same frame, the PlayerDied action is called only once.

        health -= damage;
        HealthDecreased?.Invoke(health);
        if (health <= 0) PlayerDied?.Invoke();
    }

    public void OnTargetableAgentGetHit(TargetableAgentHitInfo hitInfo)
    {
        if (!hitInfo.isEnemy) ApplyCivilianShootPenalty();
    }

    private void ApplyCivilianShootPenalty()
    {
        GetHit(maxHealth / 3f);
    }


}
