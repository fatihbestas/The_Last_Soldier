using UnityEngine;

[CreateAssetMenu(fileName = "GunName", menuName = "ScriptableObjects/GunAnimationData")]
public class GunAnimationData : ScriptableObject
{
    // Animation triggers
    public string idleAnimTrigger;
    public string shootAnimTrigger;
    public string runAnimTrigger;
    public string runAndShootAnimTrigger;
    public string changeAnimTrigger;
    public string takeDamageAnimTrigger;
    public string AimIdleAnimTrigger;
    public string[] deathAnimTrigger;

    public string GetDeathAnimTrigger()
    {
        return deathAnimTrigger[Random.Range(0, deathAnimTrigger.Length)];
    }
}
