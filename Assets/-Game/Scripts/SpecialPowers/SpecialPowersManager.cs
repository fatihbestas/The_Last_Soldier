using UnityEngine;

public class SpecialPowersManager : MonoBehaviour
{
    public static SpecialPowersManager instance;

    public float laserCooldown;

    private void Awake()
    {
#if UNITY_EDITOR
        if (instance != null) Debug.LogError(gameObject.name);
#endif
        instance = this;
    }

    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }

  
}
