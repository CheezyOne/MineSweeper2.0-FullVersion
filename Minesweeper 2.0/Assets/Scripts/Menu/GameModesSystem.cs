using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModesSystem : MonoBehaviour
{
    [SerializeField] private GameObject Check, MovingBombsHandler;
    [Header("Mutually exclusive options")]
    [SerializeField] private GameObject TwoBombsCheck;
    [SerializeField] private GameObject MovingBombsCheck;

    public static Action PlaySound;
    public void PlayButtonSound()
    {
        PlaySound?.Invoke();
    }
    public void TwoBombTypes()
    {
        if (MovingBombsCheck.activeSelf)
            MovingBombs();
        TwoBombTypesFun();
    }
    private void TwoBombTypesFun()
    {
        if (TwoBombsCheck.activeSelf)
        {
            TwoBombsCheck.SetActive(false);
            ApplyMines.ApplyBlueBombs = false;
            TwoTypesOfBombsChanger.IsToChange = false;
        }
        else
        {
            TwoBombsCheck.SetActive(true);
            ApplyMines.ApplyBlueBombs = true;
            TwoTypesOfBombsChanger.IsToChange = true;
        }
    }
    
    public void LyingCells()
    {
        if (Check.activeSelf)
        {
            Check.SetActive(false);
            Cell.ShouldLie = false;
        }
        else
        {
            Check.SetActive(true);
            Cell.ShouldLie = true;
        }
    }
    public void Bariers()
    {
        if (Check.activeSelf)
        {
            Check.SetActive(false);
            FieldGeneration.ApplyBariers = false;
        }
        else
        {
            Check.SetActive(true);
            FieldGeneration.ApplyBariers = true;
        }
    }
    private void MovingBombsFun()
    {
        if (MovingBombsCheck.activeSelf)
        {
            MovingBombsCheck.SetActive(false);
            MovingBombsHandler.SetActive(false);
        }
        else
        {
            MovingBombsCheck.SetActive(true);
            MovingBombsHandler.SetActive(true);
        }
    }
    public void MovingBombs()
    {
        if (TwoBombsCheck.activeSelf)
            TwoBombTypesFun();
        MovingBombsFun();
    }
}
