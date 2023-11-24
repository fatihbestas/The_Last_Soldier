using UnityEngine;

[CreateAssetMenu(fileName = "GunName", menuName = "ScriptableObjects/GunData")]
public class GunData : ScriptableObject
{
    public int id;
    public string gunName;
    public float firePower;
    public float firePowerIncrease;
    public float speed;
    public float speedIncrease;
    public float maxSpeed;
    public float maxLevel;
    public float bonusForSoldiers;
    public float bonusForVehicles;

#if UNITY_EDITOR
    private void OnEnable()
    {
        if (speed <= 0 || maxSpeed <= 0)
        {
            Debug.LogError("shoot speed cannot be zero");
        }
    }
# endif
}
