using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TargetableAgent : MonoBehaviour
{
    public enum AgentType
    {
        Human,
        Aircraft
    }

    public int id;
    public Collider myCollider;
    public Collider colliderForOverlap;
    [SerializeField] protected Animator animator;

    protected Vector3 destination;
    protected IEnumerator moveRoutine;
    protected float moveSpeed;
    protected bool isMoving;

    protected List<TargetableAgent> collidedAgents = new List<TargetableAgent>();

    public static event Action<TargetableAgentHitInfo> TargetableAgentGetHit;

    protected virtual void OnTargetableAgentGetHit(TargetableAgentHitInfo hitInfo)
    {
        TargetableAgentGetHit?.Invoke(hitInfo);
    }

    public virtual void Initialize(Vector3 destination, float waveMoveSpeed)
    {
        this.destination = destination;
        isMoving = true;
        moveRoutine = Move();
        StartCoroutine(moveRoutine);
        myCollider.enabled = true;
        colliderForOverlap.enabled = true;
        moveSpeed = waveMoveSpeed;
    }

    protected abstract IEnumerator Move();

    public abstract void TakeDamage(float damage, Vector3 hitPoint);

    public abstract void TakeDamageByLaser();

    protected abstract void Death();

    protected virtual float GetDistanceToTarget()
    {
        return Vector3.Distance(transform.position, destination);
    }

    public virtual bool DecideWhichOneToStop(float otherAgentDistance, TargetableAgent collidedAgent)
    {
        if (otherAgentDistance > GetDistanceToTarget())
        {
            collidedAgents.Add(collidedAgent);
            return true;
        }

        return false;
    }

    protected abstract void Stop();

    public virtual void MoveCollidedAgentsAgain()
    {
        foreach (TargetableAgent item in collidedAgents)
        {
            if (item.gameObject.activeInHierarchy)
                item.MoveAgain();
        }
        collidedAgents.Clear();
    }

    public abstract void MoveAgain();

    public abstract bool IsHuman();

    protected abstract void LevelEnd(bool isLevelPassed);

    protected abstract void LevelStart();

}

public class TargetableAgentHitInfo
{
    public bool isHuman;
    public bool isEnemy;
    public bool isDied;
    public Vector3 hitPoint;
}
