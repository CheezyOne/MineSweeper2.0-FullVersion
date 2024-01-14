using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InGameTimer : MonoBehaviour
{
    [SerializeField] private GameObject TimerObj;
    [SerializeField] private int Seconds = 0, Minutes=0;
    private string SecondsText;
    private bool TimerIsActive = false, NullTimerOnce=true;

    private void Awake()
    {
        StartingSequence.onAllCubesFall += StartTimer;
        Cell.onGameLose += StopTheTimer;
        VictoryHandler.onGameWon += StopTheTimer;
        InGameButtons.onGameExit += StopTheTimer;
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
        NullTimerOnce = true;
        StartCoroutine(TimerCourotine());
        TimerObj.GetComponent<TMP_Text>().text = "0:00";
        TimerIsActive = true;
    }
    private IEnumerator TimerCourotine()
    {
        yield return new WaitForSeconds(1f);
        Seconds++;
        if(Seconds>=60)
        {
            Seconds -= 60;
            Minutes++;
        }
        if(Seconds<=9)
            SecondsText="0"+ Convert.ToString(Seconds);
        else
            SecondsText = Convert.ToString(Seconds);
        if (TimerIsActive && TimerObj != null)
        {
            TimerObj.GetComponent<TMP_Text>().text = (Minutes+":"+ SecondsText);
        }
        yield return TimerCourotine();
    }
    private void Update()
    {
        if (NullTimerOnce)
        {
            if (!TimerObj.activeInHierarchy)
            {
                TimerObj.GetComponent<TMP_Text>().text = "0:00";
                NullTimerOnce = false;
            }
        }
    }
}
