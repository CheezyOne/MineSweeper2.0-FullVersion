using System;
using System.Collections.Generic;
using UnityEngine;

public class FieldGeneration : MonoBehaviour
{
    public static string FieldType = "Normal";
    [SerializeField] private GameObject Cell, Field, Barier;
    private Vector3 PositionFromField = Vector3.zero;
    public int ColomnsSize = 10, StringsSize = 10;
    public static int StaticColomsSize, StaticStringsSize;
    private int Counter = 0, SpecialFormsCounter=0;
    [SerializeField] private ApplyMines ApplyComponent;
    public static List <GameObject> AllCubes = new List<GameObject>(), AllBariers=new List<GameObject>();
    public static Action onFieldGenerated;
    public static bool ApplyBariers = false;
    public static int BariersCount=0;

    private void OnEnable()
    {
        InGameButtons.onGameExit += DestroyField;
        ButtonsInMenu.onPlayButtonPress += GenerateAField;
    }

    private void OnDisable()
    {
        InGameButtons.onGameExit -= DestroyField;
        ButtonsInMenu.onPlayButtonPress -= GenerateAField;
    }
    private void DestroyField()
    {
        for(int i=0;i< Field.transform.childCount;i++)
        {
            Destroy(Field.transform.GetChild(i).gameObject);
        }
    }
    private void Update()
    {
        StaticColomsSize = ColomnsSize;
        StaticStringsSize = StringsSize;
    }
    private void GenerateAField()
    {
        AllCubes = new List<GameObject>();
        PositionFromField = Vector3.zero;
        Counter = 0;
        SpecialFormsCounter = 0;
        switch (FieldType)
        {
            case "Normal":
                {
                    ApplyComponent.FieldSize = ColomnsSize * StringsSize;
                    for (float i=0;i< ColomnsSize; i++)
                    {
                        for(float j=0;j< StringsSize; j++)
                        {
                            GameObject NewCell = Instantiate(Cell, Field.transform.position + PositionFromField, Quaternion.identity);
                            NewCell.transform.SetParent(Field.transform);
                            AllCubes.Add(NewCell);
                            PositionFromField += new Vector3(1.1f, 0, 0);
                            Counter++;
                        }
                        PositionFromField -= new Vector3(1.1f* Counter, 0, 0);
                        Counter = 0;
                        PositionFromField += new Vector3(0, 0, 1.1f);
                    }

                    break;
                }
            case "Cross":
                {
                    int ColomnsCutter = ColomnsSize / 5;
                    int StringsCutter = StringsSize / 5;
                    ApplyComponent.FieldSize = ColomnsSize* StringsSize -(ColomnsCutter * StringsCutter*4);
                    SpecialFormsCounter = 0;
                    for (float i = 0; i < ColomnsSize; i++)
                    {
                        for (float j = 0; j < StringsSize; j++)
                        {
                            if (Counter > StringsCutter-1 && Counter < StringsSize-StringsCutter || (SpecialFormsCounter> ColomnsCutter -1 && SpecialFormsCounter< ColomnsSize - ColomnsCutter))
                            {
                                GameObject NewCell = Instantiate(Cell, Field.transform.position + PositionFromField, Quaternion.identity);
                                NewCell.transform.SetParent(Field.transform);
                                AllCubes.Add(NewCell);
                            }
                            PositionFromField += new Vector3(1.1f, 0, 0);
                            Counter++;
                        }
                        PositionFromField -= new Vector3(1.1f * Counter, 0, 0);
                        SpecialFormsCounter++;
                        Counter = 0;
                        PositionFromField += new Vector3(0, 0, 1.1f);
                    }
                    break;
                }
            case "Diamond":
                {//must have ColomnsSize==StringsSize

                    int AnglesCutter = ColomnsSize / 2 - 1;
                    int AnglesCutterSquared = AnglesCutter * AnglesCutter;
                    int NotEmptySpaces = 0;
                    for(int i= AnglesCutter-1;i>0;i--)
                    {
                        NotEmptySpaces += i;
                    }

                    int EmptySpaces = AnglesCutterSquared - NotEmptySpaces;
                    EmptySpaces *= 4;
                    ApplyComponent.FieldSize = ColomnsSize* ColomnsSize- EmptySpaces;
                    SpecialFormsCounter = 0;
                    for (float i = 0; i < ColomnsSize / 2; i ++)//Lower part of the diamond
                    {
                        for (float j = 0; j < StringsSize; j ++)
                        {
                            if (!(AnglesCutter - SpecialFormsCounter > Counter || (ColomnsSize-AnglesCutter) + SpecialFormsCounter <= Counter))
                            {
                                GameObject NewCell = Instantiate(Cell, Field.transform.position + PositionFromField, Quaternion.identity);
                                NewCell.transform.SetParent(Field.transform);
                                AllCubes.Add(NewCell);
                                Counter++;
                                PositionFromField += new Vector3(1.1f, 0, 0);
                                continue;
                            }
                           
                            PositionFromField += new Vector3(1.1f, 0, 0);
                            Counter++;
                        }
                        PositionFromField -= new Vector3(1.1f * Counter, 0, 0);
                        SpecialFormsCounter++;
                        Counter = 0;
                        PositionFromField += new Vector3(0, 0, 1.1f);
                    }
                    if(ColomnsSize%2==0)
                        SpecialFormsCounter--;
                    for (float i = ColomnsSize / 2; i < ColomnsSize; i ++)
                    {
                        for (float j = 0; j < StringsSize; j ++)
                        {
                            if (!(AnglesCutter - SpecialFormsCounter > Counter || (ColomnsSize - AnglesCutter) + SpecialFormsCounter <= Counter))
                            {
                                GameObject NewCell = Instantiate(Cell, Field.transform.position + PositionFromField, Quaternion.identity);
                                NewCell.transform.SetParent(Field.transform);
                                AllCubes.Add(NewCell);
                                Counter++;
                                PositionFromField += new Vector3(1.1f, 0, 0);
                                continue;
                            }

                            PositionFromField += new Vector3(1.1f, 0, 0);
                            Counter++;
                        }
                        PositionFromField -= new Vector3(1.1f * Counter, 0, 0);
                        SpecialFormsCounter--;
                        Counter = 0;
                        PositionFromField += new Vector3(0, 0, 1.1f);
                    }
                    break;
                }
            case "Hole":
                {
                    int ColomnsCutter = 0;
                    int StringsCutter = 0;
                    if (ColomnsSize<=10)
                    {
                         ColomnsCutter = 3;
                    }
                    else
                    {
                        int i = (ColomnsSize-10)/4;
                        ColomnsCutter = 3 + i;
                    }
                    if (StringsSize <= 10)
                    {
                        StringsCutter = 3;
                    }
                    else
                    {
                        int i = (StringsSize - 10) / 4;
                        StringsCutter = 3 + i;
                    }
                    ApplyComponent.FieldSize = StringsSize * ColomnsSize - ((ColomnsSize - ColomnsCutter * 2) * (StringsSize - StringsCutter * 2));

                    if (ApplyComponent.FieldSize == 24)
                        ApplyComponent.FieldSize++;
                    SpecialFormsCounter = 0;
                    for (float i = 0; i < ColomnsSize; i ++)
                    {
                        for (float j = 0; j < StringsSize; j ++)
                        {
                            if (SpecialFormsCounter > ColomnsCutter-1 && SpecialFormsCounter < ColomnsSize-ColomnsCutter && Counter > StringsCutter -1 && Counter < StringsSize- StringsCutter)
                            {
                                PositionFromField += new Vector3(1.1f, 0, 0);
                                Counter++;
                                continue;
                            }
                            GameObject NewCell = Instantiate(Cell, Field.transform.position + PositionFromField, Quaternion.identity);
                            NewCell.transform.SetParent(Field.transform);
                            AllCubes.Add(NewCell);
                            PositionFromField += new Vector3(1.1f, 0, 0);
                            Counter++;
                        }
                        SpecialFormsCounter++;
                        PositionFromField -= new Vector3(1.1f * Counter, 0, 0);
                        Counter = 0;
                        PositionFromField += new Vector3(0, 0, 1.1f);
                    }
                    break;
                }
            case "4 Sides":
                { //must have ColomnsSize==StringsSize
                    SpecialFormsCounter = 0;
                    if(ColomnsSize%2==0)
                    {
                        ApplyComponent.FieldSize = ColomnsSize * StringsSize - (ColomnsSize * 2);
                    }
                    else
                    {
                        ApplyComponent.FieldSize = ColomnsSize * StringsSize-(ColomnsSize *2 -1 );
                    }
                    for (float i = 0; i < ColomnsSize; i++)
                    {
                        for (float j = 0; j < StringsSize; j++)
                        {
                            if (Counter == SpecialFormsCounter || (ColomnsSize-1) - SpecialFormsCounter==Counter)
                            {
                                PositionFromField += new Vector3(1.1f, 0, 0);
                                Counter++;
                                continue;
                            }
                            GameObject NewCell = Instantiate(Cell, Field.transform.position + PositionFromField, Quaternion.identity);
                            NewCell.transform.SetParent(Field.transform);
                            AllCubes.Add(NewCell);
                            PositionFromField += new Vector3(1.1f, 0, 0);
                            Counter++;
                        }
                        SpecialFormsCounter++;
                        PositionFromField -= new Vector3(1.1f * Counter, 0, 0);
                        Counter = 0;
                        PositionFromField += new Vector3(0, 0, 1.1f);
                    }
                    break;
                }
            default:
                {
                    Debug.Log("Can't identify the field type");
                    break;
                }
        }
        AllBariers.Clear();
        if (ApplyBariers)
            CreateBariers();
        onFieldGenerated?.Invoke();
    }
    private void CreateBariers()
    {
        Vector3 BarierPosition =Vector3.zero;
        List<Vector3> AllBariersPositions = new List<Vector3>(); 
        bool ShouldRotate, ShallNotProceed = false;
        int TriesCounter = 0;
        BariersCount = ApplyComponent.FieldSize / 10;
        // BariersCount = 1;
        List<GameObject> AllCubes = new List<GameObject>();
        for(int i=0;i<FieldGeneration.AllCubes.Count;i++)
        {
            AllCubes.Add(FieldGeneration.AllCubes[i]);
        }
        float YPosition = 11f;
        for(int i=0;i<BariersCount;i++)
        {
            ShouldRotate = false;
            int RandomInt=UnityEngine.Random.Range(0, AllCubes.Count - 1);
            GameObject Neighbour = null;
            while (Neighbour == null)
            {
                Neighbour = AllCubes[RandomInt].GetComponent<Cell>().GetRandomNeighbour();
                TriesCounter++;
                if (TriesCounter > 80)
                    return;
                continue;
            }
            if (Neighbour.transform.position.x - AllCubes[RandomInt].transform.position.x>=0.1f)
            {
                if (Neighbour.transform.position.z - AllCubes[RandomInt].transform.position.z>=0.1f)
                {
                    BarierPosition = new Vector3(AllCubes[RandomInt].transform.position.x, YPosition, AllCubes[RandomInt].transform.position.z  + 0.55f);
                }
                else
                {
                    ShouldRotate = true;
                    BarierPosition = new Vector3(AllCubes[RandomInt].transform.position.x  + 0.55f, YPosition, AllCubes[RandomInt].transform.position.z);
                }
            }
            else if (Neighbour.transform.position.x - AllCubes[RandomInt].transform.position.x<=-0.1f)
            {
                if (Neighbour.transform.position.z - AllCubes[RandomInt].transform.position.z >= 0.1f)
                {
                    BarierPosition = new Vector3(AllCubes[RandomInt].transform.position.x, YPosition, AllCubes[RandomInt].transform.position.z  + 0.55f);
                }
                else//HERE
                {
                    ShouldRotate = true;
                    BarierPosition = new Vector3(AllCubes[RandomInt].transform.position.x  - 0.55f, YPosition, AllCubes[RandomInt].transform.position.z);
                }
            }
            else if (Neighbour.transform.position.z - AllCubes[RandomInt].transform.position.z >= 0.1f)
                BarierPosition = new Vector3(AllCubes[RandomInt].transform.position.x, YPosition, AllCubes[RandomInt].transform.position.z  + 0.55f);
            else if (Neighbour.transform.position.z - AllCubes[RandomInt].transform.position.z<=0.1f)
                BarierPosition = new Vector3(AllCubes[RandomInt].transform.position.x, YPosition, AllCubes[RandomInt].transform.position.z - 0.55f);

            foreach(Vector3 BP in AllBariersPositions)
            {
                if (BP == BarierPosition)
                {
                    ShallNotProceed = true;
                    break;
                }

            }
            if (ShallNotProceed)
            {
                ShallNotProceed = false;
                continue;
            }
            GameObject NewBarier = Instantiate(Barier, BarierPosition,Quaternion.identity);//Rotate if on the left and right
            NewBarier.transform.SetParent(Field.transform);
            AllBariers.Add(NewBarier);
            AllBariersPositions.Add(BarierPosition);
            if (ShouldRotate)
                NewBarier.transform.Rotate(0, 90f, 0);
            AllCubes.RemoveAt(RandomInt);
        }
    }
}
