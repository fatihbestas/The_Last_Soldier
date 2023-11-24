using UnityEngine;

public class PlayerGunManager : MonoBehaviour
{
    private Gun currentGun;
    public Gun CurrentGun
    {
        get
        { 
            return currentGun;
        }
        private set
        { 
            currentGun = value;
        }
    }

    // just to saving current gun info to PlayerPrefs. 
    private int currentGunId;

    public Gun[] allGuns;

    private void Awake()
    {
        currentGunId = PlayerPrefs.GetInt("currentGunId", 0);
        currentGun = GunIdToGunObject(currentGunId);
    }

    private void Start()
    {

#if UNITY_EDITOR
        ControllGunsInfo();
# endif
    }

    public void ChangeCurrentGun(Gun newGun)
    {
        currentGun = newGun;
        currentGunId = newGun.playerData.id;
        PlayerPrefs.SetInt("currentGunId", currentGunId);
    }

    private Gun GunIdToGunObject(int _id)
    {
        foreach (Gun gun in allGuns)
        {
            if(_id == gun.playerData.id)
                return gun;
        }

        Debug.LogError("There is a problem about guns");
        return null;
    }

#if UNITY_EDITOR
    private void ControllGunsInfo()
    {
        for (int i = 0; i < allGuns.Length; i++)
        {
            for(int j = 0; j < allGuns.Length; j++)
            {
                if(i != j)
                {
                    if (allGuns[i].playerData.id == allGuns[j].playerData.id)
                        Debug.LogError("There are multiple weapons with the same id.");

                    if (allGuns[i].playerData.name == allGuns[j].playerData.name)
                        Debug.LogError("There are multiple weapons with the same id.");
                }
            }
        }
    }
#endif
}
