using System;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    [SerializeField] private int startMoney;
    private int currentMoney;
    public int CurrentMoney
    {
        get { return currentMoney; }
    }

    public static event Action<int> MoneyAmountChanged;

    private void Awake()
    {
        currentMoney = PlayerPrefs.GetInt("currentMoney", startMoney);
#if UNITY_EDITOR
        Debug.Log("Current Money: " + currentMoney);
#endif
    }

    private void OnEnable()
    {
        LevelManager.LevelPassed += AddMoney;
    }

    private void Start()
    {
        MoneyAmountChanged?.Invoke(currentMoney);
    }

    private void OnDisable()
    {
        LevelManager.LevelPassed -= AddMoney;
    }

    private void AddMoney(int value)
    {
        currentMoney += value;
        MoneyAmountChanged?.Invoke(currentMoney);
        PlayerPrefs.SetInt("currentMoney", currentMoney);
    }

    private void WithdrawMoney(int value)
    {
        currentMoney -= value;
        MoneyAmountChanged?.Invoke(currentMoney);
        PlayerPrefs.SetInt("currentMoney", currentMoney);
    }

}
