using System;
using System.Collections;
using UnityEngine;

public class Enemy : TargetableAgent
{
    [SerializeField] private EnemyData data;
    [SerializeField] private Gun gun;
    public GameObject laserHitVfx;

    private IEnumerator shootRoutine;

    private float health;
    private float firePower;
    private float shootSpeed;

    private bool isDiedByLaser = false;

    private TargetableAgentHitInfo hitInfo = new TargetableAgentHitInfo();

    public static event Action<float> HitPlayer;

    private void Awake()
    {
        hitInfo.isHuman = IsHuman();
        hitInfo.isEnemy = true;
        StopLaserHitVfx();
    }

    private void OnEnable()
    {
        GameManager.OnLevelStart += LevelStart;
        GameManager.OnLevelEnd += LevelEnd;
        isDiedByLaser = false;
        health = LevelManager.currentLevelData.enemyHealth * data.healthMultiplier;
        firePower = LevelManager.currentLevelData.enemyFirePower;
        shootSpeed = LevelManager.currentLevelData.enemyShootSpeed;
    }

#if UNITY_EDITOR
    private void Start()
    {
        if (!gameObject.CompareTag("Enemy"))
        {
            Debug.LogError("Enemy Tag must be \"Enemy\" GameObject Name: " + gameObject.name);
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
        animator.SetTrigger(gun.animationData.runAnimTrigger);
        transform.LookAt(destination, Vector3.up);

        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, destination) <= 0.1)
            {
                isMoving = false;
                shootRoutine = Shoot();
                StartCoroutine(shootRoutine);
                break;
            }

            yield return null;
        }
    }

    public override void TakeDamage(float damage, Vector3 hitPoint)
    {
        health -= damage;
        hitInfo.hitPoint = hitPoint;

        if (health <= 0)
        {
            Death();
            hitInfo.isDied = true;
            OnTargetableAgentGetHit(hitInfo);
            return;
        }
        hitInfo.isDied = false;
        OnTargetableAgentGetHit(hitInfo);
    }
    public override void TakeDamageByLaser()
    {
        if (isDiedByLaser)
            return;

        isDiedByLaser = true;
        health = -1;
        PlayLaserHitVfx();
        Death();
        hitInfo.hitPoint = Vector3.zero;
        hitInfo.isDied = true;
        OnTargetableAgentGetHit(hitInfo);
    }

    private void PlayLaserHitVfx()
    {
        laserHitVfx.SetActive(true);
    }

    private void StopLaserHitVfx()
    {
        laserHitVfx.SetActive(false);
    }

    private IEnumerator Shoot()
    {
        while (true)
        {
            HitPlayer?.Invoke(firePower);
            animator.SetTrigger(gun.animationData.shootAnimTrigger);
            gun.PlayMuzzleFlashVfx();
            yield return new WaitForSeconds(1f/shootSpeed);
        }
    }

    protected override void Death()
    {
        isMoving = false;
        myCollider.enabled = false;
        colliderForOverlap.enabled = false;
        StopAllCoroutines();
        animator.SetTrigger(gun.animationData.GetDeathAnimTrigger());
        MoveCollidedAgentsAgain();
        StartCoroutine(DeathRoutine());
    }

    private IEnumerator DeathRoutine()
    {
        yield return new WaitForSeconds(1);
        StopLaserHitVfx();
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }

    protected override void Stop()
    {
        isMoving = false;
        animator.SetTrigger(gun.animationData.AimIdleAnimTrigger);
        if (moveRoutine != null)
            StopCoroutine(moveRoutine);
    }

    public override void MoveAgain()
    {
        if (!isMoving && health > 0)
        {
            moveRoutine = Move();
            StartCoroutine(moveRoutine);
        }
    }

    public override bool IsHuman() 
    {
        return data.enemyType == AgentType.Human; 
    }

    protected override void LevelStart()
    {
        gameObject.SetActive(false);
    }

    protected override void LevelEnd(bool isLevelPassed)
    {
        if (health <= 0)
            return;
        StopAllCoroutines();
        animator.SetTrigger(gun.animationData.AimIdleAnimTrigger);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!other.gameObject.activeInHierarchy)
            return; 

        if(other.CompareTag("Overlap"))
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
