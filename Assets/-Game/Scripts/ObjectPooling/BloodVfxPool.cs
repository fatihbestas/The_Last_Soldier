using UnityEngine;

public class BloodVfxPool : ObjectPool
{
    private GameObject bloodVfx;

#if UNITY_EDITOR
    private void Start()
    {
        if(objectToPool.GetComponent<ParticleSystem>().main.stopAction != ParticleSystemStopAction.Disable)
        {
            Debug.LogError("Blood Splash Particle system stop action must be \"Disable\". " +
                "Otherwise Object Pooling will not work correctly.");
        }

        if (poolSize <= 0)
        {
            Debug.LogError("Pool size error. Object Name: " + gameObject.name);
        }

        if (objectToPool == null)
        {
            Debug.LogWarning("objectToPool error. Object Name:" + gameObject.name);
        }
    }

#endif

    private void OnEnable()
    {
        TargetableAgent.TargetableAgentGetHit += OnTargetableAgentGetHit;
    }

    private void OnDisable()
    {
        TargetableAgent.TargetableAgentGetHit -= OnTargetableAgentGetHit;
    }

    public void OnTargetableAgentGetHit(TargetableAgentHitInfo hitInfo)
    {
        if (hitInfo.isHuman) PlayBloodVfx(hitInfo.hitPoint);
    }

    private void PlayBloodVfx(Vector3 hitPoint)
    {
        bloodVfx = GetPooledObject();
        bloodVfx.transform.position = hitPoint;
        bloodVfx.SetActive(true);
    }
}
