using System.Collections;
using UnityEngine;

public class CrosshairController : MonoBehaviour
{
    [SerializeField] private Transform crosshairGroup;
    [SerializeField] private RectTransform up;
    [SerializeField] private RectTransform down;
    [SerializeField] private RectTransform right;
    [SerializeField] private RectTransform left;
    [SerializeField] private float defaultPosition;
    [SerializeField] private float moveRatePerHit;
    [SerializeField] private float maxHitPosition;
    [SerializeField] private float hitSpeed;
    [SerializeField] private float cooldownSpeed;
    [SerializeField] private Animator glowAnimator;

    private IEnumerator hitCoroutine;
    private IEnumerator cooldownCoroutine;
    private float targetPoint;

    private void OnEnable()
    {
        GameManager.OnLevelStart += OnLevelStart;
        GameManager.OnLevelEnd += OnLevelEnd;
        TargetableAgent.TargetableAgentGetHit += OnTargetableAgentGetHit;
    }

    private void Start()
    {
        crosshairGroup.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        GameManager.OnLevelStart -= OnLevelStart;
        GameManager.OnLevelEnd -= OnLevelEnd;
        TargetableAgent.TargetableAgentGetHit -= OnTargetableAgentGetHit;
    }

    private void OnLevelStart()
    {
        crosshairGroup.gameObject.SetActive(true);
        MoveCrosshairParts(defaultPosition);
    }

    private void OnLevelEnd(bool isLevelPassed)
    {
        crosshairGroup.gameObject.SetActive(false);
    }

    private void MoveCrosshairParts(float position)
    {
        up.anchoredPosition = Vector2.up * position;
        down.anchoredPosition = Vector2.down * position;
        right.anchoredPosition = Vector2.right * position;
        left.anchoredPosition = Vector2.left * position;
    }

    public void OnTargetableAgentGetHit(TargetableAgentHitInfo hitInfo)
    {
        StopCooldown();
        StopHitCoroutine();
        StartHitCoroutine();

        if (hitInfo.isDied) glowAnimator.SetTrigger("Death");
        else glowAnimator.SetTrigger("Hit");
    }

    private void StartHitCoroutine()
    {
        targetPoint = up.anchoredPosition.y + moveRatePerHit;
        if (targetPoint > maxHitPosition)
        {
            targetPoint = maxHitPosition;
        }
        hitCoroutine = HitCoroutine(targetPoint);
        StartCoroutine(hitCoroutine);
    }

    private void StopHitCoroutine()
    {
        if (hitCoroutine != null)
            StopCoroutine(hitCoroutine);
    }

    private void StartCooldown()
    {
        cooldownCoroutine = Cooldown();
        StartCoroutine(cooldownCoroutine);
    }

    private void StopCooldown()
    {
        if(cooldownCoroutine != null)
            StopCoroutine(cooldownCoroutine);
    }

    private IEnumerator HitCoroutine(float targetPoint)
    {
        while (true) 
        {
            MoveCrosshairParts(up.anchoredPosition.y + hitSpeed * Time.deltaTime);

            if (up.anchoredPosition.y >= targetPoint)
            {
                MoveCrosshairParts(targetPoint);
                StartCooldown();
                break;
            }

            yield return null;
        }
    }

    private IEnumerator Cooldown()
    {
        while (true)
        {
            MoveCrosshairParts(up.anchoredPosition.y - cooldownSpeed * Time.deltaTime);

            if (up.anchoredPosition.y <= defaultPosition)
            {
                MoveCrosshairParts(defaultPosition);
                break;
            }


            yield return null;
        }
    }

    //private void SetColorAlphaValue(Image image, float value)
    //{
    //    tempColor = image.color;
    //    tempColor.a = value;
    //    image.color = tempColor;
    //}
    
}
