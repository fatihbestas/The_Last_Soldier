using UnityEngine;

public class MoveToMousePosition : MonoBehaviour
{
    void Update()
    {
        this.transform.position = Input.mousePosition;
    }
}
