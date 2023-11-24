using UnityEngine;

public class StaticObjectController : MonoBehaviour
{
#if UNITY_EDITOR

    private void Start()
    {
        StaticController(transform);
    }

    private void StaticController(Transform parent)
    {
        if (parent.childCount == 0)
        {
            if (parent.gameObject.isStatic == false)
            {
                Debug.LogError("There are non-static objects in the scene that should be static. Name is: " + parent.gameObject.name);
                return;
            }
            else return;   
        }

        if(parent.gameObject.isStatic == false)
            Debug.LogError("There are non-static objects in the scene that should be static. Name is: " + parent.gameObject.name);

        foreach (Transform child in parent)
        {
            StaticController(child);
        }
    }

#endif
}
