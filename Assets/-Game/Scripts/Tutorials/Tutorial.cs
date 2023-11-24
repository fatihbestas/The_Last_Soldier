using System;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private GameObject civilianTutorial;

    public static event Action TutorialStarted;
    public static event Action TutorialEnded;

    private void OnEnable()
    {
        GameManager.OnTutorialStart += CheckLevel;
        UIManager.TutorialContinueBtnPressed += StopTutorial;
    }

    private void OnDisable()
    {
        GameManager.OnTutorialStart -= CheckLevel;
        UIManager.TutorialContinueBtnPressed -= StopTutorial;
    }

    private void CheckLevel()
    {
        if (LevelManager.currentLevelData.levelNumber == 2)
        {
            StartTutorial();
        }
        else
        {
            StopTutorial();
        }
    }

    private void StartTutorial()
    {
        TutorialStarted?.Invoke();
        civilianTutorial.SetActive(true);
    }

    private void StopTutorial()
    {
        TutorialEnded?.Invoke();
        civilianTutorial.SetActive(false);
    }
}
