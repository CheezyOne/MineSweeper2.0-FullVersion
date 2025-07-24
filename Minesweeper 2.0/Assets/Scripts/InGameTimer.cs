using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class InGameTimer : MonoBehaviour
{
    [SerializeField] private GameObject TimerObj1, TimerObj2, MinesApplier, MovingBombsHandler;
    [SerializeField] private TMP_Text _firstTimer;
    [SerializeField] private TMP_Text _secondTimer;
    public static Action onTimeDeplete;
    public static bool IsToDecrease = false;
    private static int DecreaseSeconds, DecreaseMinutes;
    private static int Seconds = 0, Minutes=0;
    private string SecondsText;
    private bool TimerIsActive = false, NullTimerOnce=true;

    private void OnEnable()
    {
        ApplyMines.onAllBombsApply += CalculateTimer;
        ApplyMines.onAllBombsApply += StartTimer;
        Cell.onGameLose += StopTheTimer;
        VictoryHandler.onGameWon += StopTheTimer;
        InGameButtons.onGameExit += StopTheTimer;
        InGameButtons.onGameExit += SetUpTime;
        InGameButtons.onGameExit += CalculateTimer;
    }
    private void OnDisable()
    {
        ApplyMines.onAllBombsApply -= CalculateTimer;
        ApplyMines.onAllBombsApply -= StartTimer;
        Cell.onGameLose -= StopTheTimer;
        VictoryHandler.onGameWon -= StopTheTimer;
        InGameButtons.onGameExit -= StopTheTimer;
        InGameButtons.onGameExit -= SetUpTime;
        InGameButtons.onGameExit -= CalculateTimer;
    }
    private void SetUpTime()
    {
        if (TimerIsActive)
        {
            if (IsToDecrease)
            {
                if (DecreaseSeconds <= 9)
                    SecondsText = "0" + Convert.ToString(DecreaseSeconds);
                else
                    SecondsText = Convert.ToString(DecreaseSeconds);
                Debug.Log(SecondsText);
                _firstTimer.text = (DecreaseMinutes + ":" + SecondsText);
                _secondTimer.text = (DecreaseMinutes + ":" + SecondsText);
            }
            else
            {
                if (Seconds <= 9)
                    SecondsText = "0" + Convert.ToString(Seconds);
                else
                    SecondsText = Convert.ToString(Seconds);

                _firstTimer.text = (Minutes + ":" + SecondsText);
                _secondTimer.text = (Minutes + ":" + SecondsText);
            }
        }
        else
        {
            _firstTimer.text = "0:00";
            _secondTimer.text = "0:00";
        }
    }
    private void CalculateTimer()
    {
        DecreaseMinutes = 0;
        DecreaseSeconds = 0;
        int MultiplierCounter = 0;
        ApplyMines APComponent = MinesApplier.GetComponent<ApplyMines>();
        double BombRercentage = (Convert.ToDouble(APComponent.BlueBombCount) + Convert.ToDouble(APComponent.RedBombCount)) / Convert.ToDouble(APComponent.FieldSize);
        if (BombRercentage > 0.5)
            BombRercentage = 0.5;
        BombRercentage *= 4;
        double Multiplier= APComponent.FieldSize / 1.5f ;
        if (ApplyMines.ApplyBlueBombs)
        {
            MultiplierCounter++;
            Multiplier *= 2;
        }
        if (Cell.ShouldLie)
        {
            MultiplierCounter++;
            Multiplier *= 1.4;
        }
        if (FieldGeneration.ApplyBariers)
        {
            MultiplierCounter++;
            Multiplier *= 2;
        }
        if (MovingBombsHandler.activeSelf)
        {
            MultiplierCounter++;
            Multiplier *= 1.4;
        }
        if (MultiplierCounter > 0)
            Multiplier /= (MultiplierCounter);
        Multiplier *= BombRercentage;
        while(Multiplier >= 60)
        {
            DecreaseMinutes++;
            Multiplier -= 60;
        }
        DecreaseSeconds = (int)Multiplier;
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
        if (!IsToDecrease)
        {
            _secondTimer.text = "0:00";
            _firstTimer.text = "0:00";
        }
        else
        {
            if (DecreaseSeconds <= 9)
                SecondsText = "0" + Convert.ToString(DecreaseSeconds);
            else
                SecondsText = Convert.ToString(DecreaseSeconds);
            _firstTimer.text = (DecreaseMinutes + ":" + SecondsText);
            _secondTimer.text = (DecreaseMinutes + ":" + SecondsText);
        }
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
            _firstTimer.text = (Minutes + ":" + SecondsText);
            _secondTimer.text = (Minutes + ":" + SecondsText);
        }
    }
    private void TimeDecrement()
    {
        DecreaseSeconds--;
        if (DecreaseSeconds < 0)
        {
            DecreaseSeconds += 60;
            DecreaseMinutes--;
        }
        if (DecreaseSeconds <= 9)
            SecondsText = "0" + Convert.ToString(DecreaseSeconds);
        else
            SecondsText = Convert.ToString(DecreaseSeconds);
        if (TimerIsActive)
        {
            _firstTimer.text = (DecreaseMinutes + ":" + SecondsText);
            _secondTimer.text = (DecreaseMinutes + ":" + SecondsText);
        }
        if (DecreaseSeconds <= 0 && DecreaseMinutes<=0)
        {
            Cell.PlaySound?.Invoke();
            Cell.onGameLose?.Invoke();
            Cell.onGameLoseSmiley?.Invoke(7);
        }
    }
    private IEnumerator TimerCourotine()
    {
        yield return new WaitForSeconds(1f);
        if (!TimerIsActive)
            yield break;
        if (!IsToDecrease)
            TimeIncrement();
        else
            TimeDecrement();
        yield return TimerCourotine();
    }
    private void Update()
    {
        if (!NullTimerOnce)
        {
            return;
        }
        if (IsToDecrease)
        {
            if (!TimerObj1.activeSelf)
            {
                if (DecreaseSeconds <= 9)
                    SecondsText = "0" + Convert.ToString(DecreaseSeconds);
                else
                    SecondsText = Convert.ToString(DecreaseSeconds);
                _firstTimer.text = (DecreaseMinutes + ":" + SecondsText);
                NullTimerOnce = false;
            }
            if (!TimerObj2.activeSelf)
            {
                if (DecreaseSeconds <= 9)
                    SecondsText = "0" + Convert.ToString(DecreaseSeconds);
                else
                    SecondsText = Convert.ToString(DecreaseSeconds);
                _secondTimer.text = (DecreaseMinutes + ":" + SecondsText);
                NullTimerOnce = false;
            }
        }
        else
        {
            if (!TimerObj1.activeSelf)
            {
                _firstTimer.text = "0:00";
                NullTimerOnce = false;
            }
            if (!TimerObj2.activeSelf)
            {
                _secondTimer.text = "0:00";
                NullTimerOnce = false;
            }
        }
    }
}
