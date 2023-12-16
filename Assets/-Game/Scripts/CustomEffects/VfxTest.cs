using System.Collections;
using UnityEngine;

public class VfxTest : MonoBehaviour
{
    private ParticleSystem _particleSystem;
    [SerializeField] private float interval;

    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }

    private void OnEnable()
    {
        StartCoroutine(PlayLoop());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator PlayLoop()
    {
        while (true)
        {
            _particleSystem.Play();
            yield return new WaitForSeconds(interval/2f);
            _particleSystem.Stop();
            yield return new WaitForSeconds(interval/2f);
        }
    }
}
