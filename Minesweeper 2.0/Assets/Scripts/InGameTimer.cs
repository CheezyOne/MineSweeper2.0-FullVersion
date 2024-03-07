using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InGameTimer : MonoBehaviour
{
    [SerializeField] private GameObject TimerObj1, TimerObj2;
    private static int Seconds = 0, Minutes=0;
    private string SecondsText;
    private bool TimerIsActive = false, NullTimerOnce=true;

    private void OnEnable()
    {
        StartingSequence.onAllCubesFall += StartTimer;
        Cell.onGameLose += StopTheTimer;
        VictoryHandler.onGameWon += StopTheTimer;
        InGameButtons.onGameExit += StopTheTimer;
    }
    private void OnDisable()
    {
        StartingSequence.onAllCubesFall -= StartTimer;
        Cell.onGameLose -= StopTheTimer;
        VictoryHandler.onGameWon -= StopTheTimer;
        InGameButtons.onGameExit -= StopTheTimer;
    }
    private void StopTheTimer()
    {
        TimerIsActive = false;
        StopAllCoroutines();
        Seconds = 0;
        Minutes = 0;
    }
    private void StartTimer()
    {
        if (TimerIsActive)
            return;
        NullTimerOnce = true;
        StartCoroutine(TimerCourotine());
        TimerObj2.GetComponent<TMP_Text>().text = "0:00";
        TimerObj1.GetComponent<TMP_Text>().text = "0:00";
        TimerIsActive = true;
    }
    private void TimeIncrement()
    {
        Seconds++;
        if (Seconds >= 60)
        {
            Seconds -= 60;
            Minutes++;
        }
        if (Seconds <= 9)
            SecondsText = "0" + Convert.ToString(Seconds);
        else
            SecondsText = Convert.ToString(Seconds);
        if (TimerIsActive)
        {
            TimerObj1.GetComponent<TMP_Text>().text = (Minutes + ":" + SecondsText);
            TimerObj2.GetComponent<TMP_Text>().text = (Minutes + ":" + SecondsText);
        }
    }
    private IEnumerator TimerCourotine()
    {
        yield return new WaitForSeconds(1f);
        TimeIncrement();
        yield return TimerCourotine();
    }
    private void Update()
    {
        if (NullTimerOnce)
        {
            if (!TimerObj1.activeSelf)
            {
                TimerObj1.GetComponent<TMP_Text>().text = "0:00";
                NullTimerOnce = false;
            }
            if (!TimerObj2.activeSelf)
            {
                TimerObj2.GetComponent<TMP_Text>().text = "0:00";
                NullTimerOnce = false;
            }
        }
    }
}
