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
    public void TwoBombTypes()
    {
        PlaySound?.Invoke();
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
    
    public void Bariers()
    {
        PlaySound?.Invoke();
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
    public void ConnectedBombs()
    {
        PlaySound?.Invoke();
        if (Check.activeSelf)
        {
            Check.SetActive(false);
            ApplyMines.ApplyConnectedBombs = false;
        }
        else
        {
            Check.SetActive(true);
            ApplyMines.ApplyConnectedBombs = true;
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
        PlaySound?.Invoke();
        if (TwoBombsCheck.activeSelf)
            TwoBombTypesFun();
        MovingBombsFun();
    }
}
