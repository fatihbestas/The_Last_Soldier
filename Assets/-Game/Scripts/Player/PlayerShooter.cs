using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;

public class PlayerShooter : MonoBehaviour
{
    [SerializeField] private Transform playerVisualPart;
    [SerializeField] private Vector2 joystickSize = new Vector2(400, 400);
    [SerializeField] private FloatingJoystick joystick;
    [SerializeField] private Transform shootOrigin;
    [SerializeField] private Transform aimTarget;
    [SerializeField] private Transform followTarget;
    [SerializeField] private Transform targetsOrigin;
    [SerializeField] private Transform targetsParent;
    [SerializeField] private float cursorUpperLimit;
    [SerializeField] private float cursorRightLimit;
    [SerializeField] private float playerXRotationMax;

    private PlayerAnimationManager playerAnimationManager;
    private PlayerGunManager playerGunManager;

    private Vector2 knobPosition;
    private float maxMovement;
    private ETouch.Touch currentTouch;
    private Finger movementFinger;
    private Vector2 movementAmount;

    private LayerMask TargetableAgentLayer;
    private RaycastHit hit;

    private IEnumerator aimCoroutine;
    private IEnumerator cursorRoutine;
    private bool isSubscribedToTouchEvent;

    private float firePower;
    private float shootSpeed;

    private void Awake()
    {
        playerAnimationManager = GetComponent<PlayerAnimationManager>();
        playerGunManager = GetComponent<PlayerGunManager>();
        maxMovement = joystickSize.x / 2f;
        movementAmount = Vector2.zero;
    }

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        GameManager.OnLevelStart += OnLevelStart;
        GameManager.OnLevelEnd += OnLevelEnd;
    }

    private void Start()
    {
        PlayIdleAnimation();
        TargetableAgentLayer = LayerMask.GetMask("TargetableAgent");
    }

    private void OnDisable()
    {
        EnhancedTouchSupport.Disable();
        GameManager.OnLevelStart -= OnLevelStart;
        GameManager.OnLevelEnd -= OnLevelEnd;
    }

    public void SubscribeTouchEvents()
    {
        isSubscribedToTouchEvent = true;
        ETouch.Touch.onFingerDown += HandleFingerDown;
        ETouch.Touch.onFingerUp += HandleFingerUp;
        ETouch.Touch.onFingerMove += HandleFingerMove;
    }

    public void UnSubscribeTouchEvents()
    {
        isSubscribedToTouchEvent = false;
        movementAmount = Vector2.zero;
        movementFinger = null;
        ETouch.Touch.onFingerDown -= HandleFingerDown;
        ETouch.Touch.onFingerUp -= HandleFingerUp;
        ETouch.Touch.onFingerMove -= HandleFingerMove;
    }

    private void HandleFingerMove(Finger movedFinger)
    {
        if (movedFinger != movementFinger)
            return;

        currentTouch = movedFinger.currentTouch;

        knobPosition = currentTouch.screenPosition - joystick.rectTransform.anchoredPosition;

        joystick.knob.anchoredPosition = knobPosition;
        movementAmount = knobPosition / maxMovement;

        movementAmount.x = Mathf.Clamp(movementAmount.x, -1, 1);
        movementAmount.y = Mathf.Clamp(movementAmount.y, -1, 1);
    }

    private void HandleFingerUp(Finger lastFinger)
    {
        if (lastFinger != movementFinger)
            return;

        StopAiming(true);

        movementFinger = null;
        joystick.gameObject.SetActive(false);
    }

    private void HandleFingerDown(Finger touchedFinger)
    {
        if (movementFinger != null)
            return;

        if (touchedFinger.screenPosition.y > Screen.height / 2f)
            return;
            
        StartAiming(); 
    
        movementFinger = touchedFinger;
        joystick.gameObject.SetActive(true);
        joystick.rectTransform.anchoredPosition = touchedFinger.screenPosition - movementAmount * maxMovement;
        
    }

    private void OnLevelStart()
    {
        MoveCursorToDefaultPosition();
        SubscribeTouchEvents();
        firePower = playerGunManager.CurrentGun.GetPlayerFirePower();
        shootSpeed = playerGunManager.CurrentGun.GetPlayerShootSpeed();
    }

    private void OnLevelEnd(bool isLevelPassed)
    {
        UnSubscribeTouchEvents();
        StopAiming(false);
    }

    private void PlayIdleAnimation()
    {
        playerAnimationManager.SetIdleAnim();  
    }

    private void PlayAimAnimation()
    {
        playerAnimationManager.SetAimAnim();
    }

    private void StartMovingCursor()
    {
        cursorRoutine = MoveCursorByPlayer();
        StartCoroutine(cursorRoutine);
    }

    private void StopMovingCursor()
    {
        if (cursorRoutine != null)
            StopCoroutine(cursorRoutine);
    }

    private void StartAiming()
    {
        PlayAimAnimation();
        StartMovingCursor();

        aimCoroutine = Aim();
        StartCoroutine(aimCoroutine);
    }

    private void StopAiming(bool playIdle)
    {
        StopMovingCursor();

        if (aimCoroutine != null) StopCoroutine(aimCoroutine);

        if (playIdle) PlayIdleAnimation();
    }

    private IEnumerator Aim()
    {
        while (true)
        {
            if (Physics.Raycast(shootOrigin.position, shootOrigin.forward, out hit, 200, TargetableAgentLayer))
            {
                Shoot();
                yield return new WaitForSeconds(1f / shootSpeed);
            }
            else
            {
                yield return null;
            }
        }
    }

    private void Shoot()
    {
        playerAnimationManager.SetShootAnim();
        playerGunManager.CurrentGun.PlayMuzzleFlashVfx();

        AllTargetableAgents.TargetableAgentColliderBond[hit.collider].TakeDamage(firePower, hit.point);
    }

    private IEnumerator MoveCursorByPlayer()
    {
        while (true)
        {
            MoveCursor(movementAmount);

            yield return null;
        }
    }

    private void MoveCursorToDefaultPosition()
    {
        MoveCursor(Vector2.zero);
    }

    private void MoveCursor(Vector2 movementAmount)
    {
        targetsOrigin.rotation = Quaternion.Euler(movementAmount.y * cursorUpperLimit, movementAmount.x * cursorRightLimit, 0);

        //cursor.transform.localScale = Mathf.Lerp(cursorMaxScale, cursorMinScale, (movementAmount.y + 1.0f) / 2) * Vector3.one;

        playerVisualPart.localPosition = Vector3.zero;
        //playerVisualPart.LookAt(aimTarget, Vector3.up);
        playerVisualPart.localRotation = Quaternion.Euler(Mathf.Clamp(movementAmount.y * cursorUpperLimit, -playerXRotationMax, playerXRotationMax), movementAmount.x * cursorRightLimit, 0);
    }

    private void OnApplicationQuit()
    {
        if (isSubscribedToTouchEvent)
            UnSubscribeTouchEvents();
    }
}
