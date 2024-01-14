using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombsInGameChanger : MonoBehaviour
{
    [SerializeField] private GameObject MineField;
    private List<GameObject> Bombs = new List<GameObject>(), Neighbours = new List<GameObject>(), BombsToClear = new List<GameObject>();
    private float Timer=2f,TimeHolder=2f;
    public static Action onBombsMove;
    private void Awake()
    {
        ApplyMines.onAllBombsApply += ApplyBombs;
        ApplyMines.onAllBombsApply += GetANeighbour;
    }

    private void ApplyBombs()
    {
        Bombs=new List<GameObject>();
        for (int i=0;i<MineField.transform.childCount;i++)
        {
            if(MineField.transform.GetChild(i).GetComponent<Cell>()==null)
            {
                continue;
            }
            if (MineField.transform.GetChild(i).GetComponent<Cell>().HasRedBomb)
            {
                Bombs.Add(MineField.transform.GetChild(i).gameObject);
            }
        }
    }
    private void GetANeighbour()
    {
        bool isNull = false;
        Neighbours= new List<GameObject>();
        BombsToClear = new List<GameObject>();
        List<GameObject> AllNeighbours = new List<GameObject>();
        for(int i=0;i<Bombs.Count;i++)
        {
            AllNeighbours= Bombs[i].GetComponent<Cell>().GetAllNeighbours();
            for(int j=0;j< AllNeighbours.Count;j++)
            {
                if (AllNeighbours[j].GetComponent<Cell>() == null)
                    continue;
                if (!AllNeighbours[j].GetComponent<Cell>().HasRedBomb && AllNeighbours[j].GetComponent<MeshRenderer>().material.name!= "Touched cube (Instance)")
                {
                    for(int o=0;o<Neighbours.Count;o++)
                    {
                        if(Neighbours[o] == AllNeighbours[j])
                        {
                            Neighbours.Add(null);
                            BombsToClear.Add(null);
                            isNull = true;
                            break;
                            //Non-bomb is occupied
                        }
                    }
                    if (!isNull)
                    {
                        BombsToClear.Add(Bombs[i]);
                        Neighbours.Add(AllNeighbours[j]);
                    }
                    else
                    {
                        isNull = false;
                        continue;
                    }
                    break;
                }
            }
        }
    }
    private void Update()
    {
        if (!ClickRegister.isGameOn)
            return;
        Timer -= Time.deltaTime;
        if (Timer < 0)
        {
            Timer = TimeHolder;
            MoveAllBombs();
        }
    }
    private void MoveAllBombs()
    {
        
        for (int i = 0; i < Neighbours.Count; i++)
        {
            if (Neighbours[i] != null)
            {
                if (Neighbours[i].GetComponent<MeshRenderer>().material.name != "Touched cube (Instance)")
                {
                    Neighbours[i].GetComponent<Cell>().HasRedBomb = true;
                    BombsToClear[i].GetComponent<Cell>().HasRedBomb = false;
                }
            }
        }
        List<GameObject> Helper = new List<GameObject>();
        Helper = Neighbours;
        Neighbours = BombsToClear;
        BombsToClear = Helper;
        onBombsMove?.Invoke();
    }
}
