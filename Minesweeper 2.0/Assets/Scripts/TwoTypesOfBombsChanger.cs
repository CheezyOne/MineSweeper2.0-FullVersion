using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoTypesOfBombsChanger : MonoBehaviour
{
    public static Action onBombsChange;
    public static bool IsToChange = false;
    private float _timerForChange = 1f;
    private void ChangeBombs()
    {

        onBombsChange?.Invoke();
    }
    private void Update()
    {
        if (IsToChange)
        {
            if (_timerForChange > 0)
            {
                _timerForChange -= Time.deltaTime;
            }
            else
            {
                ChangeBombs();
                _timerForChange = 1.5f;
            }
        }
    }
}
