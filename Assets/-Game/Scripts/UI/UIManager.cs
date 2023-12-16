using System;
using System.Collections;
using UnityEngine;
using Microlight.MicroBar;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject inGameUIParent;
    [SerializeField] private GameObject menuUIParent;
    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private GameObject levelEndUI;
    [SerializeField] private GameObject levelCompletedUI;
    [SerializeField] private GameObject levelFailedUI;
    [SerializeField] private GameObject[] civilianHitWarning;
    [SerializeField] private GameObject laserButton;
    [SerializeField] private GameObject inGameLevelText;
    [SerializeField] private GameObject shootTutorial;
    [SerializeField] private GameObject civilianTutorial;
    [SerializeField] private GameObject healthBarParent;
    [SerializeField] private GameObject tutorialContinueButton;
    [SerializeField] private MicroBar healthBarController;

    [SerializeField] private Text moneyText;
    [SerializeField] private Text[] levelTexts;

    public static event Action StartButtonPressed;
    public static event Action LaserButtonPressed;
    public static event Action PlayAgainButtonPressed;
    public static event Action ContinueButtonPressed;
    public static event Action TutorialContinueBtnPressed;

    private void Awake()
    {
        StartState();
    }

    private void OnEnable()
    {
        GameManager.OnLevelStart += OnLevelStart;
        GameManager.OnLevelEnd += OnLevelEnd;
        PlayerHealth.LevelStarted += InitializeHealthBar;
        PlayerHealth.HealthDecreased += UpdateHealthBar;
        TargetableAgent.TargetableAgentGetHit += OnTargetableAgentGetHit;
        CurrencyManager.MoneyAmountChanged += UpdateMoneyText;
        Tutorial.TutorialStarted += OnTutorialStart;
    }

    private void Start()
    {
        UpdateLevelTexts();
    }

    private void OnDisable()
    {
        GameManager.OnLevelStart -= OnLevelStart;
        GameManager.OnLevelEnd -= OnLevelEnd;
        PlayerHealth.LevelStarted -= InitializeHealthBar;
        PlayerHealth.HealthDecreased -= UpdateHealthBar;
        TargetableAgent.TargetableAgentGetHit -= OnTargetableAgentGetHit;
        CurrencyManager.MoneyAmountChanged -= UpdateMoneyText;
        Tutorial.TutorialStarted -= OnTutorialStart;
    }

    private void StartState()
    {
        inGameUIParent.SetActive(true); // never disable it.
        menuUIParent.SetActive(true); // never disable it.
        healthBarParent.SetActive(false);
        mainMenuUI.SetActive(true);
        levelEndUI.SetActive(true);
        levelCompletedUI.SetActive(false);
        levelFailedUI.SetActive(false);
        levelEndUI.SetActive(false);
        inGameLevelText.SetActive(false);
        shootTutorial.SetActive(false);
        civilianTutorial.SetActive(false);
        tutorialContinueButton.SetActive(false);
    }

    public void OnStartButtonPressed()
    {
        mainMenuUI.SetActive(false);
        StartButtonPressed?.Invoke();
    }

    private void OnLevelStart()
    {
        mainMenuUI.SetActive(false);
        levelFailedUI.SetActive(false);
        levelCompletedUI.SetActive(false);
        levelEndUI.SetActive(false);
        UpdateLevelTexts();
        inGameLevelText.SetActive(false);
        inGameLevelText.SetActive(true);
        shootTutorial.SetActive(true);
    }

    private void InitializeHealthBar(float maxHealth)
    {
        healthBarParent.SetActive(true);
        healthBarController.Initialize(maxHealth);
    }

    private void UpdateHealthBar(float value)
    {
        healthBarController.UpdateHealthBar(value);
    }

    private void OnLevelEnd(bool isLevelPassed)
    {
        StartCoroutine(LevelEndRoutine(isLevelPassed));
    }

    private IEnumerator LevelEndRoutine(bool isLevelPassed)
    {
        yield return new WaitForSeconds(0.5f);

        healthBarParent.SetActive(false);
        levelEndUI.SetActive(true);
        levelCompletedUI.SetActive(isLevelPassed);
        levelFailedUI.SetActive(!isLevelPassed);

    }

    public void OnLaserButtonPressed()
    {
        LaserButtonPressed?.Invoke();
        laserButton.SetActive(false);
        StartCoroutine(EnableGameObjectWithDelay(laserButton, SpecialPowersManager.instance.laserCooldown));
    }

    public void OnTargetableAgentGetHit(TargetableAgentHitInfo hitInfo)
    {
        if(!hitInfo.isEnemy) PlayCivilianShootWarning();
    }

    private void PlayCivilianShootWarning()
    {
        foreach (GameObject warning in civilianHitWarning)
        {
            if (!warning.activeInHierarchy)
            {
                warning.SetActive(true);
                break;
            }
        }
    }

    public void OnPlayAgainButtonPressed()
    {
        PlayAgainButtonPressed?.Invoke();
    }

    public void OnContinueButtonPressed()
    {
        ContinueButtonPressed?.Invoke();
    }

    public void OnTutorialContinueBtnPressed()
    {
        tutorialContinueButton.SetActive(false);
        civilianTutorial.SetActive(false);
        TutorialContinueBtnPressed?.Invoke();
    }

    private void OnTutorialStart()
    {
        mainMenuUI.SetActive(false);
        levelFailedUI.SetActive(false);
        levelCompletedUI.SetActive(false);
        levelEndUI.SetActive(false);
        civilianTutorial.SetActive(true);
        StartCoroutine(EnableGameObjectWithDelay(tutorialContinueButton, 1.5f));
    }

    private IEnumerator EnableGameObjectWithDelay(GameObject gameobject, float delay)
    {
        yield return new WaitForSeconds(delay);
        gameobject.SetActive(true);
    }

    private void UpdateMoneyText(int currentMoney)
    {
        moneyText.text = "<color=lime>$</color>" + currentMoney.ToString();
    }

    private void UpdateLevelTexts()
    {
        foreach (Text levelText in levelTexts)
        {
            levelText.text = "LEVEL " + LevelManager.currentLevelData.levelNumber.ToString(); 
        }
    }
    
}
