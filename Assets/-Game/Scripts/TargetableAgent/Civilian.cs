using System;
using System.Collections;
using UnityEngine;

public class Civilian : TargetableAgent
{
    [SerializeField] private CivilianData data;

    private bool isDied;
    private TargetableAgentHitInfo hitInfo = new TargetableAgentHitInfo();

    public static event Action CivilianRescued;

    private void Awake()
    {
        hitInfo.isHuman = IsHuman();
        hitInfo.isEnemy = false;
    }

    private void OnEnable()
    {
        GameManager.OnLevelStart += LevelStart;
        GameManager.OnLevelEnd += LevelEnd;
        isDied = false;
    }

#if UNITY_EDITOR
    private void Start()
    {
        if(!gameObject.CompareTag("Civilian"))
        {
            Debug.LogError("Civilian Tag must be \"Civilian\" GameObject Name: " + gameObject.name);
        }
    }
#endif

    private void OnDisable()
    {
        GameManager.OnLevelStart -= LevelStart;
        GameManager.OnLevelEnd -= LevelEnd;
        myCollider.enabled = false;
        colliderForOverlap.enabled = false;
        StopAllCoroutines();
    }

    protected override IEnumerator Move()
    {
        isMoving = true;
        //animator.SetTrigger(data.runAnimTrigger);
        transform.LookAt(destination, Vector3.up);

        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, destination) <= 4)
            {
                isMoving = false;
                Rescued();
                break;
            }

            yield return null;
        }
    }

    private void Rescued()
    {
        CivilianRescued?.Invoke();
        StopAllCoroutines();
        MoveCollidedAgentsAgain();
        gameObject.SetActive(false);
    }

    public override void TakeDamage(float damage, Vector3 hitPoint)
    {
        Death();
        hitInfo.hitPoint = hitPoint;
        hitInfo.isDied = true;
        OnTargetableAgentGetHit(hitInfo);
    }

    public override void TakeDamageByLaser()
    {
        return;
    }

    protected override void Death()
    {
        isDied = true;
        isMoving = false;
        myCollider.enabled = false;
        colliderForOverlap.enabled = false;
        StopAllCoroutines();
        MoveCollidedAgentsAgain();
        //animator.SetTrigger(data.GetDeathAnimTrigger());
        StartCoroutine(DeathRoutine());
    }

    private IEnumerator DeathRoutine()
    {
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }

    protected override void Stop()
    {
        isMoving = false;
        Rescued();
    }

    public override void MoveAgain()
    {
        if (!isMoving && !isDied)
        {
            moveRoutine = Move();
            StartCoroutine(moveRoutine);
        }
    }

    public override bool IsHuman()
    {
        return data.civilianType == AgentType.Human;
    }

    protected override void LevelStart()
    {
        gameObject.SetActive(false);
    }

    protected override void LevelEnd(bool isLevelPassed)
    {
        if (isDied)
            return;
        StopAllCoroutines();
        //animator.SetTrigger(data.idleAnimTrigger);
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.activeInHierarchy)
            return;

        if (other.CompareTag("Overlap"))
        {
            if (other.GetComponentInParent<TargetableAgent>().DecideWhichOneToStop(GetDistanceToTarget(), this))
            {
                Stop();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Overlap"))
        {
            Invoke(nameof(MoveAgain), 0.1f);
        }
    }

    
}
