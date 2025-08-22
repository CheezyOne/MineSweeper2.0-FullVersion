using System.Collections.Generic;
using UnityEngine;
using YG;

public class YGMetrics : MonoBehaviour
{
    [SerializeField] private GameObject _movingBombsHandler;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    private void OnEnable()
    {
        ApplyMines.onAllBombsApply += SendLevelMetrics;
    }

    private void OnDisable()
    {
        ApplyMines.onAllBombsApply -= SendLevelMetrics;
    }

    private void SendLevelMetrics()
    {
        bool hasOptionAdded = false;
        ReportToYandexMetrika("Start_Level");

        if (ApplyMines.ApplyBlueBombs)
        {
            ReportToYandexMetrika("Play_with_double_bombs");
            hasOptionAdded = true;
        }
        if (Cell.ShouldLie)
        {
            ReportToYandexMetrika("Play_with_lying");
            hasOptionAdded = true;
        }
        if (FieldGeneration.ApplyBariers)
        {
            ReportToYandexMetrika("Play_with_bariers");
            hasOptionAdded = true;
        }
        if (InGameTimer.IsToDecrease)
        {
            ReportToYandexMetrika("Play_with_timer");
            hasOptionAdded = true;
        }
        if (_movingBombsHandler.activeSelf)
        {
            ReportToYandexMetrika("Play_with_moving");
            hasOptionAdded = true;
        }
        if (hasOptionAdded)
        {
            ReportToYandexMetrika("Start_with_options");
        }

        YandexGame.savesData.playTimes++;

        if (YandexGame.savesData.playTimes == 3)
        {
            ReportToYandexMetrika("Played_three_times");
        }

        if (!YandexGame.savesData.startedFirstGame)
        {
            YandexGame.savesData.startedFirstGame = true;
            ReportToYandexMetrika("First_level_start");
        }

        YandexGame.SaveProgress();
    }

    public static void ReportToYandexMetrika(string goalName, Dictionary<string, object> data = null)
    {
#if !UNITY_EDITOR && UNITY_WEBGL
    string jsonData = data != null ? JsonUtility.ToJson(data) : "";
    Application.ExternalCall("ym", 103478279, "reachGoal", goalName, jsonData);
#else
        Debug.Log($"[Metrika] {goalName}: {JsonUtility.ToJson(data)}");
#endif
    }   
}
