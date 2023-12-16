using System.Collections;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    public float speed;
    public Transform[] points;

    private Animator animator;
    private int target;

    private void Start()
    {
        animator = GetComponent<Animator>();
        transform.position = points[0].position;
        target = 1;
        transform.LookAt(points[target].position);
        StartCoroutine(GoToTarget());
    }

    private IEnumerator GoToTarget()
    {
        transform.LookAt(points[target].position);
        animator.SetTrigger("Run");

        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, points[target].position, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, points[target].position) == 0) 
            {
                StartCoroutine(Wait());
                break;
            }
            else yield return null;
        }
    }

    private IEnumerator Wait()
    {
        animator.SetTrigger("Idle");
        yield return new WaitForSeconds(2);
        target = (target + 1) % points.Length;
        StartCoroutine(GoToTarget());
    }
}
