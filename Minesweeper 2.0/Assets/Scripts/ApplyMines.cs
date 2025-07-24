using System;
using System.Collections.Generic;
using UnityEngine;

public class ApplyMines : MonoBehaviour//Red bombs and blue bombs are vise-versa in the code
{
    [SerializeField] private Transform MineField;
    public static GameObject CantBeBomb;
    public int FieldSize = 0;
    public int RedBombCount, BlueBombCount;
    private List<int> RedBombCells = new(), BlueBombCells = new();
    private List<int> AllCellsForRed = new(), AllCellsForBlue = new();
    public static Action onAllBombsApply;
    public static bool ApplyBlueBombs = false, ApplyConnectedBombs = false;
    private int NonBombsCounter=0;
    private bool FirstNonBomb = true;
    private void OnEnable()
    {
        ClickRegister.onFirstCubeTouch += ApplyBombCells;
        BombsInGameChanger.onBombsMove += ApplyBombsCount;
        BombsInGameChanger.onBombsMove += ApplyBombsCount;
    }
    private void OnDisable()
    {
        ClickRegister.onFirstCubeTouch -= ApplyBombCells;
        BombsInGameChanger.onBombsMove -= ApplyBombsCount;
        BombsInGameChanger.onBombsMove -= ApplyBombsCount;
    }
    private void NullAllVariables()
    {
        RedBombCells = new List<int>();
        BlueBombCells = new List<int>();
        AllCellsForRed = new List<int>();
        AllCellsForBlue = new List<int>();
        BlueBombCount = 0;
        RedBombCount = 0;
        NonBombsCounter = 0;
        FirstNonBomb = true;
    }

    private void GetAllCells()
    {
        for (int i = 0; i < MineField.childCount; i++)
        {
            if (MineField.GetChild(i).name == "Cell(Clone)")
            {
                AllCellsForRed.Add(i);
                AllCellsForBlue.Add(i);
            }
        }
    }
    private void DecideBombCount()
    {
        RedBombCount = BombsAmountMenu.RememberBombs;
        if(ApplyBlueBombs)
        {
            if(RedBombCount>1)
                RedBombCount /= 2;
            BlueBombCount = RedBombCount;
        }
    }
    private int GetRandomNumber(List<int> AllCells)
    {
        int RandomNumber = UnityEngine.Random.Range(0, AllCells.Count);
        int ReturnNumber = AllCells[RandomNumber];
        AllCells.RemoveAt(RandomNumber);
        return ReturnNumber;
    }
    private void GetNoBombNumber(GameObject NotBomb)
    {
        if (!FirstNonBomb)
        {
            if (RedBombCount + BlueBombCount + NonBombsCounter >= FieldSize - 3)//-3 для того, чтобы не абузить первый клик
            {
                return;
            }
        }
        FirstNonBomb = false;
        for (int i=0;i<MineField.childCount;i++)
        {
            if (MineField.GetChild(i).gameObject == NotBomb)
            {
                AllCellsForRed.Remove(i);
                AllCellsForBlue.Remove(i);
                NonBombsCounter++;
                break;
            }
        }
    }
    private void ApplyNormalBombs()
    {
        for (int i = 0; i < RedBombCount; i++)
        {
            RedBombCells.Add(GetRandomNumber(AllCellsForRed));
        }
        if (ApplyBlueBombs)
        {
            for (int i = 0; i < BlueBombCount; i++)
            {
                BlueBombCells.Add(GetRandomNumber(AllCellsForBlue));
            }
        }
    }
    private void ApplyBombsCount()
    {
        if (ApplyBlueBombs)
            VictoryHandler.EmptyCellsCount = FieldSize - RedBombCount - BlueBombCount + BothBlueAndRed();//Присваивается дважды, но это фиксит баг
        else
            VictoryHandler.EmptyCellsCount = FieldSize - RedBombCount;
        InGameBombsCounter.BombsCount = FieldSize - VictoryHandler.EmptyCellsCount;
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
        GetAllCells();
        GetNoBombNumber(CantBeBomb);
        DecideBombCount();
        foreach (GameObject Cell in CantBeBomb.GetComponent<Cell>().GetAllNeighbours())
        {
            GetNoBombNumber(Cell);
        }

        RedBombCells.Add(GetRandomNumber(AllCellsForRed));
        if (ApplyBlueBombs)
            BlueBombCells.Add(GetRandomNumber(AllCellsForBlue));

        ApplyNormalBombs();
        ApplyBombsCount();
        for (int i=0;i< RedBombCount; i++)
        {
            MineField.transform.GetChild(RedBombCells[i]).GetComponent<Cell>().HasRedBomb = true;
        }
        for (int i = 0; i < BlueBombCount; i++)
        {
            MineField.transform.GetChild(BlueBombCells[i]).GetComponent<Cell>().HasBlueBomb = true;
        }
        ApplyBombsCount();
        onAllBombsApply?.Invoke();
    }
}
