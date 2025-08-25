using System;
using System.Collections.Generic;
using UnityEngine;

public class BombsInGameChanger : MonoBehaviour
{
    [SerializeField] private GameObject MineField;
    private List<Cell>  RedNeighbours = new(), BlueNeighbours = new(), RedBombsToClear = new(),BlueBombsToClear = new();
    private List<GameObject> RedBombs = new(), BlueBombs = new();
    private float Timer = 2f;
    private const float TimeHolder=2f;
    public static Action onBombsMove;
    private void OnEnable()
    {
        ApplyMines.onAllBombsApply += ApplyBombs;
        ApplyMines.onAllBombsApply += GetANeighbour;
    }
    private void OnDisable()
    {
        ApplyMines.onAllBombsApply -= ApplyBombs;
        ApplyMines.onAllBombsApply -= GetANeighbour;
    }
    private void ApplyBombs()
    {
        RedBombs = new List<GameObject>();
        BlueBombs = new List<GameObject>();
        for (int i=0;i<MineField.transform.childCount;i++)
        {
            Transform Cell = MineField.transform.GetChild(i);

            if (Cell.TryGetComponent<Cell>(out Cell CellComponent))
            {
                if (CellComponent.HasRedBomb)
                {
                    RedBombs.Add(Cell.gameObject);
                }
                else if (CellComponent.HasBlueBomb)
                {
                    BlueBombs.Add(Cell.gameObject);
                }
            }
        }
    }
    private void GetANeighbour()
    {
        RedNeighbours = new List<Cell>();
        RedBombsToClear = new List<Cell>();
        BlueNeighbours = new List<Cell>();
        BlueBombsToClear = new List<Cell>();
        for (int i=0;i<RedBombs.Count;i++)
        {
            List<GameObject> Neighbours = RedBombs[i].GetComponent<Cell>().GetAllNeighbours();
            for (int j = 0; j < Neighbours.Count;j++)
            {
                if (Neighbours[j].GetComponent<Cell>().HasBeenTouched)
                    continue;
                if (Neighbours[j].GetComponent<Cell>().HasRedBomb)
                    continue;
                if (RedNeighbours.Contains(Neighbours[j].GetComponent<Cell>()))
                    continue;
                RedNeighbours.Add(Neighbours[j].GetComponent<Cell>());
                RedBombsToClear.Add(RedBombs[i].GetComponent<Cell>());
                break;
            }
        }
        for (int i = 0; i < BlueBombs.Count; i++)
        {
            List<GameObject> Neighbours = BlueBombs[i].GetComponent<Cell>().GetAllNeighbours();
            for (int j = 0; j < Neighbours.Count; j++)
            {
                if (Neighbours[j].GetComponent<Cell>().HasBeenTouched)
                    continue;
                if (Neighbours[j].GetComponent<Cell>().HasBlueBomb)
                    continue;
                if (RedNeighbours.Contains(Neighbours[j].GetComponent<Cell>()))
                    continue;
                BlueNeighbours.Add(Neighbours[j].GetComponent<Cell>());
                BlueBombsToClear.Add(BlueBombs[i].GetComponent<Cell>());
                break;
            }
        }
    }
    private void Update()
    {
        if (!ClickRegister.isGameOn)
            return;
        Timer -= Time.deltaTime;
        if (!Cell.CubesAreOpened)
        {
            Timer = 0.5f;
            return;
        }
        if (Timer < 0)
        {
            Timer = TimeHolder;
            MoveAllBombs();
        }
    }
    private void MoveAllBombs()
    {
        
        for (int i = 0; i < RedNeighbours.Count; i++)
        {
            if (RedNeighbours[i] == null)
            {
                continue;
            }
            if (!RedNeighbours[i].HasBeenTouched)
            {
                RedNeighbours[i].HasRedBomb = true;
                RedBombsToClear[i].HasRedBomb = false;
            }
        }
        for (int i = 0; i < BlueNeighbours.Count; i++)
        {
            if (BlueNeighbours[i] == null)
            {
                continue;
            }
            if (!BlueNeighbours[i].HasBeenTouched)
            {
                BlueNeighbours[i].HasBlueBomb = true;
                BlueBombsToClear[i].HasBlueBomb = false;
            }
        }
        List<Cell> Helper = RedNeighbours;
        RedNeighbours = RedBombsToClear;
        RedBombsToClear = Helper;
        Helper = BlueNeighbours;
        BlueNeighbours = BlueBombsToClear;
        BlueBombsToClear = Helper;
        onBombsMove?.Invoke();
    }
}
