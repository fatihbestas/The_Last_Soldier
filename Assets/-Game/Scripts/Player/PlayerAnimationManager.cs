using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private PlayerGunManager gunManager;

    private enum Animations
    {
        Idle,
        Run,
        Aim,
        Shoot,
    }

    private Animations currentAnimation;

    private void Awake()
    {
        gunManager = GetComponent<PlayerGunManager>();

        currentAnimation = Animations.Idle;
        animator.SetTrigger(gunManager.CurrentGun.animationData.idleAnimTrigger);
        animator.speed = 1.0f;
    }

    public void SetIdleAnim()
    {
        if (currentAnimation == Animations.Idle)
            return;

        currentAnimation = Animations.Idle;
        animator.SetTrigger(gunManager.CurrentGun.animationData.idleAnimTrigger);
    }

    //public void SetRunAnim()
    //{
    //    if (currentAnimation == Animations.Run)
    //        return;

    //    currentAnimation = Animations.Run;

    //    animator.SetTrigger(gunManager.CurrentGun.animationData.runAnimTrigger);
    //}

    public void SetAimAnim()
    {
        if (currentAnimation == Animations.Aim)
            return;

        currentAnimation = Animations.Aim;
        animator.SetTrigger(gunManager.CurrentGun.animationData.AimIdleAnimTrigger);
    }

    public void SetShootAnim()
    {
        currentAnimation = Animations.Shoot;

        animator.SetTrigger(gunManager.CurrentGun.animationData.shootAnimTrigger);
    }
}
