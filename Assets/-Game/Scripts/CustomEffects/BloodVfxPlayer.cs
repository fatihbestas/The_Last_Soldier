using UnityEngine;

public class BloodVfxPlayer : MonoBehaviour
{
    [SerializeField] private BloodVfxPool bloodVfxPool;
    private GameObject bloodVfx;

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
        bloodVfx = bloodVfxPool.GetPooledGameObject();
        bloodVfx.transform.position = hitPoint;
        bloodVfx.SetActive(true);
    }
}
