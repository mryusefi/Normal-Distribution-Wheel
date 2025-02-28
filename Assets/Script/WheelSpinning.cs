using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
public class WheelSpinning : MonoBehaviour
{

    [System.Serializable]
    public struct wheelSlice
    {
        public enum SliceType { Nothing, ExtraMove, Win };
        public SliceType Type;
        public double Probability;
    }

    public List<wheelSlice> wheelSlices = new List<wheelSlice>();

    [SerializeField] private int TotalSpins;
    [SerializeField] private Button spinBtn;
    [SerializeField] private Image wheelImage;
    [SerializeField] private GameObject winPopup;
    [SerializeField] private GameObject losePopup;
    [SerializeField] private float spinDuration;

    [HideInInspector]
    public int SpinsRemain;
    public event Action OnSpinsRemainChange;

    private bool hasWon;
    private int totalPlayers;
    private int winnersCount;
    private string logFilePath;

    private System.Random random;
    
    void Start()
    {
        logFilePath = Application.dataPath + "/ResultData/wheel_results.txt";
        totalPlayers = 0;
        winnersCount = 0;

        SpinsRemain = TotalSpins;
        if (OnSpinsRemainChange != null)
            OnSpinsRemainChange();

        random = new System.Random();

        spinBtn.onClick.AddListener(() => Spinning());

        InitializeWheelProbabilities();
    }

    /// <summary>
    /// Initializes probability values for each wheel slice type
    /// - Nothing: 68% (distributed across 8 slices = 8.5% each)
    /// - ExtraMove: 27% (distributed across 4 slices = 6.75% each)
    /// - Win: 5% (on 1 slice)
    /// </summary>
    private void InitializeWheelProbabilities()
    {
        for (int i = 0; i < wheelSlices.Count; i++)
        {
            var slice = wheelSlices[i];
            switch (slice.Type)
            {
                case wheelSlice.SliceType.Nothing:
                    slice.Probability = 0.68f / 8;
                    break;
                case wheelSlice.SliceType.ExtraMove:
                    slice.Probability = 0.27f / 4;
                    break;
                case wheelSlice.SliceType.Win:
                    slice.Probability = 0.05f / 1;
                    break;

            }
            wheelSlices[i] = slice;
        }
    }

    public void Spinning()
    {
        SpinsRemain--;
        if (OnSpinsRemainChange != null)
            OnSpinsRemainChange();

        int selectSliceIndex = GetWheelSliceIndex();

        StartCoroutine(SpinWheelAnimation(selectSliceIndex));
    }

    IEnumerator SpinWheelAnimation(int selectSliceIndex)
    {
        spinBtn.interactable = false;

        float sliceAngle = 360f / wheelSlices.Count;
        float targetAngle = 360f * 4 + (selectSliceIndex * sliceAngle);
        float currentTime = 0f;

        while (currentTime < spinDuration)
        {
            currentTime += Time.deltaTime;
            float t = currentTime / spinDuration;

            float easedT = 1f - Mathf.Pow(1f - t, 3f);

            float angle = Mathf.Lerp(0, targetAngle, easedT);
            wheelImage.transform.eulerAngles = new Vector3(0, 0, angle);

            yield return null;
        }

        wheelImage.transform.eulerAngles = new Vector3(0, 0, targetAngle);

        ProcessResult(selectSliceIndex);

        spinBtn.interactable = true;

    }

    private void ProcessResult(int selectSliceIndex)
    {
        wheelSlice selectSlice = wheelSlices[selectSliceIndex];

        switch (selectSlice.Type)
        {
            case wheelSlice.SliceType.Nothing:
                if (SpinsRemain <= 0) endGame();
                break;
            case wheelSlice.SliceType.ExtraMove:
                SpinsRemain++;
                if (OnSpinsRemainChange != null)
                    OnSpinsRemainChange();
                break;
            case wheelSlice.SliceType.Win:
                endGame();
                break;
        }
    }

    /// <summary>
    /// Selects a wheel slice index using Box-Muller transform for normally distributed random values
    /// mapped to the probability distribution of wheel slices
    /// </summary>
    public int GetWheelSliceIndex()
    {
        double u1 = 1.0 - random.NextDouble();
        double u2 = 1.0 - random.NextDouble();
        double z = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);

        double normalizedValue = Math.Abs(z) / 3.0;
        normalizedValue = Math.Min(normalizedValue, 0.9999);

        // Select slice based on cumulative probability
        double cumulativeProbability = 0;
        for (int i = 0; i < wheelSlices.Count; i++)
        {
            cumulativeProbability += wheelSlices[i].Probability;

            if (normalizedValue < cumulativeProbability)
                return i;
        }

        return wheelSlices.Count - 1;
    }

    private void endGame()
    {
        if (hasWon)
        {
            winnersCount++;
            winPopup.SetActive(true);
        }
        else
            losePopup.SetActive(true);

        totalPlayers++;
        SaveResults();
    }
    void SaveResults()
    {
        try
        {
            string directory = Path.GetDirectoryName(logFilePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            string result = $" - Player {totalPlayers} : {(hasWon ? "Win" : "Lose")}";

            float winPercentage = (float)winnersCount / totalPlayers * 100;

            string stats = $" - Total Players : {totalPlayers}, winPercentage : {winPercentage:F1}%";

            using (StreamWriter writer = new StreamWriter(logFilePath, true))
            {
                writer.WriteLine(result);
                writer.WriteLine(stats);
                writer.WriteLine("-------------------");
            }

        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    //I set it in inspector of buttons
    public void RestGame()
    {
        losePopup.SetActive(false);
        winPopup.SetActive(false);
        SpinsRemain = TotalSpins;

        if (OnSpinsRemainChange != null)
            OnSpinsRemainChange();
    }

}
