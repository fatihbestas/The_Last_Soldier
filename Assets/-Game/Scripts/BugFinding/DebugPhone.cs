using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using System.Linq;

public class DebugPhone : MonoBehaviour
{
    public TextMeshProUGUI fpsAverage;
    public TextMeshProUGUI fpsMin;
    public TextMeshProUGUI fpsMax;

    private float[] deltaTimesForAverage = new float[15];
    private int counterAverage = 0;
    private int deltaTimesForAverageArraySize;

    private float[] deltaTimesForMinMax = new float[120];
    private int counterMinMax = 0;
    private int deltaTimesForMinMaxArraySize;

    [NonSerialized] public float fpsAverageValue;
    [NonSerialized] public float fpsMinValue;
    [NonSerialized] public float fpsMaxValue;

    private void Start()
    {
        for (int i = 0; i < deltaTimesForAverage.Length; i++)
        {
            deltaTimesForAverage[i] = 1f;
        }

        for (int i = 0;i < deltaTimesForMinMax.Length; i++) 
        {
            deltaTimesForMinMax[i] = 1f; 
        }

        deltaTimesForAverageArraySize = deltaTimesForAverage.Length;
        deltaTimesForMinMaxArraySize = deltaTimesForMinMax.Length;
    }

    private void Update()
    {
        
        FpsCounter();

        fpsAverage.text = ((int)fpsAverageValue).ToString();
        fpsMin.text = ((int)fpsMinValue).ToString();
        fpsMax.text = ((int)fpsMaxValue).ToString();

    }
   
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void FpsCounter()
    {
        // oyun basladiktan yaklasik 0.5 saniye sonra dogru sonuc vermeye baslar.
        deltaTimesForAverage[counterAverage] = Time.deltaTime;
        counterAverage = (counterAverage + 1) % deltaTimesForAverageArraySize;

        fpsAverageValue = 0;
        for (int i = 0; i < deltaTimesForAverage.Length; i++)
        {
            fpsAverageValue += deltaTimesForAverage[i];
        }
        fpsAverageValue = fpsAverageValue / deltaTimesForAverage.Length;
        fpsAverageValue = 1f / fpsAverageValue;

        deltaTimesForMinMax[counterMinMax] = Time.deltaTime;
        counterMinMax = (counterMinMax + 1) % deltaTimesForMinMaxArraySize; 
        
        fpsMinValue = 1f / deltaTimesForAverage.Max();

        fpsMaxValue = 1f / deltaTimesForAverage.Min();

    }

}
