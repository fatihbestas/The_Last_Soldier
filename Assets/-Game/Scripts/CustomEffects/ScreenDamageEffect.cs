using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenDamageEffect : MonoBehaviour
{
    [SerializeField] private Image effect;
    [SerializeField] private float damageRate;
    [SerializeField] private float minAlpha;
    [SerializeField] private float fadeSpeed;

    private Vector4 tempColor;
    private IEnumerator fadeRoutine;

    private void Awake()
    {
        tempColor = effect.color;
        tempColor.w = 0;
        effect.color = tempColor;
    }

    private void OnEnable()
    {
        PlayerHealth.HealthDecreased += TakeDamage;
    }

    private void OnDisable()
    {
        PlayerHealth.HealthDecreased -= TakeDamage;
    }

    private IEnumerator Fade()
    {
        while(true)
        {
            tempColor = effect.color;
            tempColor.w -= fadeSpeed * Time.deltaTime;

            if(tempColor.w <= 0) 
            {
                tempColor.w = 0;
                effect.color = tempColor;
                fadeRoutine = null;
                break;
            }

            effect.color = tempColor;
            yield return null;
        }
    }

    public void TakeDamage(float damage)
    {
        tempColor = effect.color;
        tempColor.w += damageRate;
        if(tempColor.w < minAlpha) tempColor.w = minAlpha;
        if(tempColor.w > 1) tempColor.w = 1;
        effect.color = tempColor;

        if(fadeRoutine == null)
        {
            fadeRoutine = Fade();
            StartCoroutine(fadeRoutine);
        }
    }

}
