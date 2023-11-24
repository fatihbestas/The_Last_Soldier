using Cinemachine;
using NaughtyAttributes;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public CinemachineVirtualCamera[] virtualCameras;
    public MenuCameraMovements menuCameraMovements;

    private CinemachineVirtualCamera _targetCamera;

    private void OnEnable()
    {
        //GameManager.OnLevelStart += SwitchToPlayerShootCamera;
        GameManager.GamePlayStarted += SwitchToPlayerShootCamera;
        Tutorial.TutorialStarted += SwitchToTutorialCamera;
        Tutorial.TutorialEnded += SwitchToPlayerShootCamera;
    }

    private void OnDisable()
    {
        //GameManager.OnLevelStart -= SwitchToPlayerShootCamera;
        GameManager.GamePlayStarted -= SwitchToPlayerShootCamera;
        Tutorial.TutorialStarted -= SwitchToTutorialCamera;
        Tutorial.TutorialEnded -= SwitchToPlayerShootCamera;
    }

    private void Start()
    {
        menuCameraMovements.StartCameraMovement();
        _targetCamera = virtualCameras[0];
        _targetCamera.enabled = false;
        _targetCamera.enabled = true;
    }

    private void SwitchCamera(CinemachineVirtualCamera targetCamera)
    {
        foreach (CinemachineVirtualCamera camera in virtualCameras)
        {
            camera.enabled = camera == targetCamera;
        }
    }

    private void SwitchToStartCamera()
    {
        if (_targetCamera != virtualCameras[0])
        {
            menuCameraMovements.StartCameraMovement();
        }

        _targetCamera = virtualCameras[0];

        SwitchCamera(_targetCamera);
    }

    private void SwitchToPlayerWalkCamera()
    {
        if (_targetCamera == virtualCameras[0])
        {
            menuCameraMovements.StopCameraMovement();
        }
        _targetCamera = virtualCameras[1];

        SwitchCamera(_targetCamera);
    }

    private void SwitchToPlayerShootCamera()
    {
        if (_targetCamera == virtualCameras[2]) return;

        if (_targetCamera == virtualCameras[0])
        {
            menuCameraMovements.StopCameraMovement();
        }
        _targetCamera = virtualCameras[2];

        SwitchCamera(_targetCamera);
    }

    private void SwitchToLaserCamera()
    {
        _targetCamera = virtualCameras[3];

        SwitchCamera(_targetCamera);
    }

    private void SwitchToTutorialCamera()
    {
        _targetCamera = virtualCameras[4];

        _targetCamera.enabled = false;
        _targetCamera.enabled = true;
    }


#if UNITY_EDITOR
    public enum CamerasTest
    {
        Start,
        PlayerWalk,
        PlayerShoot
    }

    public CamerasTest camerasTest;

    [Button]
    public void ChangeCameraTest()
    {
        if (camerasTest == CamerasTest.Start)
        {
            SwitchToStartCamera();
        }
        else if (camerasTest == CamerasTest.PlayerWalk)
        {
            SwitchToPlayerWalkCamera();
        }
        else if (camerasTest == CamerasTest.PlayerShoot)
        {
            SwitchToPlayerShootCamera();
        }
    }

#endif
}

