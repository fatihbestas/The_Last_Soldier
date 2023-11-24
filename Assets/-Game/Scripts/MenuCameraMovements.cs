using System.Collections;
using UnityEngine;

public class MenuCameraMovements : MonoBehaviour
{
    public float followTargetTurnSpeed;
    public Transform followTargetParent;

    private IEnumerator coroutine;

    private IEnumerator MoveFollowTarget()
    {
        while (true)
        {
            followTargetParent.rotation *= Quaternion.Euler(0, followTargetTurnSpeed * Time.deltaTime, 0);
            yield return null;
        }
    }

    public void StartCameraMovement()
    {
        coroutine = MoveFollowTarget();
        StartCoroutine(coroutine);
    }

    public void StopCameraMovement()
    {
        StopCoroutine(coroutine);
    }
}
