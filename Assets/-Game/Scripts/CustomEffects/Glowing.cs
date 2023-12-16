using System.Collections;
using UnityEngine;

public class Glowing : MonoBehaviour
{
    public Material mat;
    [Range(0f, 1f)]
    public float maxAlpha;
    [Range(0f, 5f)]
    public float speed;

    private Vector4 temp;

    private void OnEnable()
    {
        temp = mat.color;
        temp.w = 0;
        mat.color = temp;
        StartCoroutine(Glow());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator Glow()
    {
        while (true)
        {
            temp = mat.color;
            temp.w += speed * Time.deltaTime;

            if (temp.w >= maxAlpha)
            {
                temp.w = maxAlpha;
                mat.color = temp;
                StartCoroutine(Fade());
                break;
            }

            mat.color = temp;
            yield return null;
        }
    }

    private IEnumerator Fade()
    {
        while (true)
        {
            temp = mat.color;
            temp.w -= speed * Time.deltaTime;

            if (temp.w <= 0)
            {
                temp.w = 0;
                mat.color = temp;
                StartCoroutine(Glow());
                break;
            }

            mat.color = temp;
            yield return null;
        }
    }

}
