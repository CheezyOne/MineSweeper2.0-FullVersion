using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyMines : MonoBehaviour
{
    [SerializeField] private GameObject MineField;
    public static GameObject CantBeBomb;
    public int FieldSize = 0;
    private int RedBombCount, BlueBombCount, CantBeBombNumber;
    private List<int> RedBombCells = new List<int>(), BlueBombCells = new List<int>();
    public static Action onAllBombsApply;
    public static bool ApplyBlueBombs = false, ApplyConnectedBombs = false;
    private void Awake()
    {
        ClickRegister.onFirstCubeTouch += ApplyBombCells;
    }
    private void NullAllVariables()
    {
        RedBombCells = new List<int>();
        BlueBombCells = new List<int>();
    }
    private void DecideBombCount()
    {

        RedBombCount = FieldSize / 10 + FieldSize / 20;
        if(ApplyBlueBombs)
        {
            if(RedBombCount>1)
                RedBombCount /= 2;
            BlueBombCount = RedBombCount;
        }

        //RedBombCount = FieldSize-2;
    }
    private int GetRandomNumber(List<int> BombCells)
    {
        int RandomNumber;
        do
        {
            RandomNumber = UnityEngine.Random.Range(0, FieldSize);
        }
        while (RandomNumber== CantBeBombNumber);
        foreach(int Number in BombCells)
        {
            if(RandomNumber==Number)
            {
                RandomNumber = GetRandomNumber(BombCells);
            }
        }
        return RandomNumber;
    }
    private int GetNoBombNumber() 
    {
        for (int i=0;i< MineField.transform.childCount;i++)
        {
            if (CantBeBomb == MineField.transform.GetChild(i).gameObject)
            {
                return i;
            }
        }
        return -1;
    }
    private int GetCellNumber(GameObject Cell)
    {
        for (int i = 0; i < MineField.transform.childCount; i++)
        {
            if (Cell == MineField.transform.GetChild(i).gameObject)
            {
                return i;
            }
        }
        return -1;
    }
    private GameObject FindASpotForANewConnectedRedBomb(int BombNumberToApply)
    {
        GameObject NeighbourBomb = MineField.transform.GetChild(RedBombCells[BombNumberToApply]).GetComponent<Cell>().GetRandomNeighbour();
        for (int i = 0; i < RedBombCells.Count; i++)
        {
            if (GetCellNumber(NeighbourBomb) == RedBombCells[i] || CantBeBomb == NeighbourBomb)
            {
                return null;
            }
        }
        return NeighbourBomb;
    }
    private GameObject FindASpotForANewConnectedBlueBomb(int BombNumberToApply)
    {
        GameObject NeighbourBomb = MineField.transform.GetChild(BlueBombCells[BombNumberToApply]).GetComponent<Cell>().GetRandomNeighbour();
        for (int i = 0; i < BlueBombCells.Count; i++)
        {
            if (GetCellNumber(NeighbourBomb) == BlueBombCells[i] || CantBeBomb == NeighbourBomb)
            {
                return null;
            }
        }
        return NeighbourBomb;
    }
    private void ApplyConnectedBombsFun()
    {
        int BombNumberToApply = 0, CounterForTries;
        for (int j = 0; j < RedBombCount; j++)
        {
            GameObject NeighbourBomb = null;
            CounterForTries = 0;
            while (NeighbourBomb==null)
            {
                NeighbourBomb = FindASpotForANewConnectedRedBomb(BombNumberToApply);
                CounterForTries++;
                if (CounterForTries > 3)
                {
                    CounterForTries = 0;
                    if (BombNumberToApply > 0)
                        BombNumberToApply--;
                    else
                        BombNumberToApply = RedBombCells.Count;
                }
            }
            BombNumberToApply = RedBombCells.Count;
            RedBombCells.Add(GetCellNumber(NeighbourBomb));
        }
        BombNumberToApply = 0;
        for (int j = 0; j < BlueBombCount; j++)
        {
            GameObject NeighbourBomb = null;
            CounterForTries = 0;
            while (NeighbourBomb == null)
            {
                NeighbourBomb = FindASpotForANewConnectedBlueBomb(BombNumberToApply);
                CounterForTries++;
                if (CounterForTries > 3)
                {
                    CounterForTries = 0;
                    if (BombNumberToApply > 0)
                        BombNumberToApply--;
                    else
                        BombNumberToApply = BlueBombCells.Count;
                }
            }
            BombNumberToApply = BlueBombCells.Count;
            BlueBombCells.Add(GetCellNumber(NeighbourBomb));
        }
    }    
    private void ApplyNormalBombs()
    {
        for (int i = 0; i < RedBombCount; i++)
        {
            RedBombCells.Add(GetRandomNumber(RedBombCells));
        }
        for (int i = 0; i < BlueBombCount; i++)
        {
            BlueBombCells.Add(GetRandomNumber(BlueBombCells));
        }
    }
    private int BothBlueAndRed()
    {
        int BlueRedBombs = 0;
        for(int i=0;i< MineField.transform.childCount;i++)
        {
            if (!MineField.transform.GetChild(i).TryGetComponent<Cell>(out Cell cellComponent))
                continue;
            if (cellComponent.HasBlueBomb && cellComponent.HasRedBomb)
                BlueRedBombs++;
        }
        return BlueRedBombs;
    }
    public void ApplyBombCells()
    {
        NullAllVariables();
        CantBeBombNumber = GetNoBombNumber();
        if (CantBeBombNumber == -1)
            Debug.Log("It's impossible");
        DecideBombCount();




        RedBombCells.Add(GetRandomNumber(RedBombCells));
        if (ApplyBlueBombs)
            BlueBombCells.Add(GetRandomNumber(BlueBombCells));
        if(ApplyConnectedBombs)
             ApplyConnectedBombsFun();
        else
            ApplyNormalBombs();
        if (ApplyBlueBombs)
            VictoryHandler.EmptyCellsCount = FieldSize - RedBombCount - BlueBombCount + BothBlueAndRed();//Присваивается дважды, но это фиксит баг
        else
            VictoryHandler.EmptyCellsCount = FieldSize - RedBombCount;
        InGameBombsCounter.BombsCount = FieldSize - VictoryHandler.EmptyCellsCount;
        for (int i=0;i< RedBombCount; i++)
        {
            MineField.transform.GetChild(RedBombCells[i]).GetComponent<Cell>().HasRedBomb = true;
        }
        for (int i = 0; i < BlueBombCount; i++)
        {
            MineField.transform.GetChild(BlueBombCells[i]).GetComponent<Cell>().HasBlueBomb = true;
        }
        if (ApplyBlueBombs)
            VictoryHandler.EmptyCellsCount = FieldSize - RedBombCount - BlueBombCount + BothBlueAndRed();//Присваивается дважды, но это фиксит баг
        else
            VictoryHandler.EmptyCellsCount = FieldSize - RedBombCount;

        onAllBombsApply?.Invoke();
    }
}
