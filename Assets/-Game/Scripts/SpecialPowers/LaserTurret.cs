using UnityEngine;
using DG.Tweening;

public class LaserTurret : MonoBehaviour
{
    [SerializeField] private GameObject turret;
    [SerializeField] private Transform body;
    [SerializeField] private GameObject laser;
    [SerializeField] private float turnDuration;
    [SerializeField] private Vector3 startRotation;
    [SerializeField] private Vector3 endRotation;
    [SerializeField] private float cameraTransitionDuration;
    [SerializeField] private float laserStartDelay;

    private void OnEnable()
    {
        UIManager.LaserButtonPressed += EnableLaser;
    }

    private void Start()
    {
        body.rotation = Quaternion.Euler(startRotation);
        laser.SetActive(false);
    }

    private void OnDisable()
    {
        UIManager.LaserButtonPressed -= EnableLaser;
    }

    private void EnableLaser()
    {
        laser.SetActive(true);
        body.DORotate(endRotation, turnDuration, RotateMode.Fast).SetEase(Ease.Linear).OnComplete(() => DisableLaser());
    }

    private void DisableLaser()
    {
        laser.SetActive(false);
        body.DORotate(startRotation, turnDuration, RotateMode.Fast).SetEase(Ease.InOutExpo);
    }
}
