using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryHandler : MonoBehaviour
{
    public static int EmptyCellsCount;
    private int ClickedCellsCount = 0;
    public static Action<int> onVictorySmiley;
    public static Action onGameWon;
    private void Awake()
    {
        Cell.onEmptyCellClicked += NewCellClicked;
        InGameButtons.onGameExit += NullCounter;
       
    }
    private void NullCounter()
    {
        ClickedCellsCount = 0;
    }
    private void NewCellClicked()
    {
        ClickedCellsCount++;
       //Debug.Log(ClickedCellsCount);
        if (ClickedCellsCount == EmptyCellsCount)
            Victory();
    }    
    private void Victory()
    {
        ClickRegister.isGameOn = false;
        onVictorySmiley?.Invoke(5);
        onGameWon?.Invoke();
        NullCounter();
        Debug.Log("You won");
    }
}
