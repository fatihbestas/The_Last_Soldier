using UnityEngine;

public class Gun : MonoBehaviour
{
    public GunData playerData;
    public FriendsGunData friendsData;
    public GunAnimationData animationData;

    [SerializeField] private Animator muzzleFlashAnimator;

    private int currentLevel;

    public void PlayMuzzleFlashVfx()
    {
        muzzleFlashAnimator.SetTrigger("Flash");
    }

    public float GetPlayerFirePower()
    {
        return playerData.firePower;
    }

    public float GetPlayerShootSpeed()
    {
        return playerData.speed;
    }
}
