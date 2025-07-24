using System;
using UnityEngine;
using TMPro;

public class BombsAmountMenu : MonoBehaviour
{
    public static int RememberBombs;
    [SerializeField] private ApplyMines ApplyMinesComponent;
    [SerializeField] private TMP_InputField _inputField;

    private string _fieldType=> FieldGeneration.FieldType;
    private int _fieldString => FieldSizeMenu.RememberString;
    private int _fieldColomn=> FieldSizeMenu.RememberColomn;
    private int _fieldSize;

    private void Awake()
    {
        GetFieldSize();
        if (RememberBombs==0)
            SetupNumbers();
    }
    private void OnEnable()
    {
        JustShowNewNumber();
        PlusMinusButtons.onBombSizeChange += ChangeOnce;
        MobileCanvasSwapper.onCanvasSwap += JustShowNewNumber;
        ButtonsInMenu.onMapChange += SetupNumbers;
        FieldSizeMenu.onSizeChange +=SetupNumbers;
    }
    private void OnDisable()
    {
        PlusMinusButtons.onBombSizeChange -= ChangeOnce;
        MobileCanvasSwapper.onCanvasSwap -= JustShowNewNumber;
        ButtonsInMenu.onMapChange -= SetupNumbers;
        FieldSizeMenu.onSizeChange -= SetupNumbers;
    }
    private void ChangeOnce(bool Positive)
    {
        if(Positive)
        {
            RememberBombs = ConstrainInt(RememberBombs + 1);
        }
        else
        {
            RememberBombs = ConstrainInt(RememberBombs - 1);
        }
        JustShowNewNumber();
    }
    private void GetFieldSize()
    {
        switch (_fieldType)
        {
            case "Normal":
                {
                    _fieldSize = _fieldColomn * _fieldString;
                    break;
                }
            case "Cross":
                {
                    int ColomnsCutter = _fieldColomn / 5;
                    int StringsCutter = _fieldString / 5;
                    _fieldSize = _fieldColomn * _fieldString - (ColomnsCutter * StringsCutter * 4);
                    break;
                }
            case "Diamond":
                {

                    int AnglesCutter = _fieldColomn / 2 - 1;
                    int AnglesCutterSquared = AnglesCutter * AnglesCutter;
                    int NotEmptySpaces = 0;
                    for (int i = AnglesCutter - 1; i > 0; i--)
                    {
                        NotEmptySpaces += i;
                    }
                    int EmptySpaces = AnglesCutterSquared - NotEmptySpaces;
                    EmptySpaces *= 4;
                    _fieldSize = _fieldColomn * _fieldColomn - EmptySpaces;
                    break;
                }
            case "Hole":
                {
                    int ColomnsCutter = 0;
                    int StringsCutter = 0;
                    if (_fieldColomn <= 10)
                    {
                        ColomnsCutter = 3;
                    }
                    else
                    {
                        int i = (_fieldColomn - 10) / 4;
                        ColomnsCutter = 3 + i;
                    }
                    if (_fieldString <= 10)
                    {
                        StringsCutter = 3;
                    }
                    else
                    {
                        int i = (_fieldString - 10) / 4;
                        StringsCutter = 3 + i;
                    }
                    _fieldSize = _fieldString * _fieldColomn - ((_fieldColomn - ColomnsCutter * 2) * (_fieldString - StringsCutter * 2));
                    break;
                }
            case "4 Sides":
                {
                    if (_fieldColomn % 2 == 0)
                    {
                        _fieldSize = _fieldColomn * _fieldString - (_fieldColomn * 2);
                    }
                    else
                    {
                        _fieldSize = _fieldColomn * _fieldString - (_fieldColomn * 2 - 1);
                    }
                    break;
                }
            default:
                {
                    Debug.Log("Can't identify the field type");
                    break;
                }
        }
    }
    private int DecideBombsCount()
    {
        GetFieldSize();
        if (ApplyMines.ApplyBlueBombs)
        {
            if((_fieldSize / 10 + _fieldSize / 20)%2==1)
                return RememberBombs = _fieldSize / 10 + _fieldSize / 20 - 1;
        }
        return RememberBombs = _fieldSize / 10 + _fieldSize / 20;
    }
    private void JustShowNewNumber()
    {
        _inputField.text = Convert.ToString(RememberBombs);
    }
    private void SetupNumbers()
    {
         _inputField.text = Convert.ToString(DecideBombsCount());
    }
    public void ChangeBombs(string Helper)
    {
        string ColomnsSize = _inputField.text;
        if (ColomnsSize == "")
        {
            return;
        }
        int IntColomnSize = Convert.ToInt32(ColomnsSize);
        //_fGComponent.ColomnsSize = IntColomnSize;
        RememberBombs = IntColomnSize;
        _inputField.text = Convert.ToString(IntColomnSize);
    }
    public void EditEndBombs(string Helper)
    {
        string ColomnsSize = _inputField.text;
        if (ColomnsSize == "")
        {
            ColomnsSize = Convert.ToString(DecideBombsCount());
        }
        int IntColomnSize = Convert.ToInt32(ColomnsSize);
        IntColomnSize = ConstrainInt(IntColomnSize);
        //_fGComponent.ColomnsSize = IntColomnSize;
        RememberBombs = IntColomnSize;
        _inputField.text = Convert.ToString(IntColomnSize);
    }
    private int ConstrainInt(int CurrentNumber)
    {
        int ConstraintNumber = CurrentNumber;
        if (ConstraintNumber < 1)
        {
            ConstraintNumber = 1;
        }
        else
        {
            if (ConstraintNumber >= _fieldSize - 2 && _fieldSize<=100)
            {
                ConstraintNumber = _fieldSize - 3;
            }
            else if(ConstraintNumber >= Convert.ToDouble(_fieldSize) *0.99f)
            {
                ConstraintNumber =Convert.ToInt32(Convert.ToDouble(_fieldSize) * 0.99f)-2;
            }
        }
        return ConstraintNumber;
    }
}
