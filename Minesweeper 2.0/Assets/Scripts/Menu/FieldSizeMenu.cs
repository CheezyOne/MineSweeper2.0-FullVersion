using System;
using UnityEngine;
using TMPro;

public class FieldSizeMenu : MonoBehaviour
{
    public static Action onSizeChange;
    [SerializeField] private FieldGeneration _fGComponent;
    public static int RememberString=10, RememberColomn=10;
    [SerializeField] private int SizeIndex;//string, colomn
    [SerializeField] private TMP_InputField _inputField;

    private void OnEnable()
    {
        SetupNumbers();
        MobileCanvasSwapper.onCanvasSwap += SetupNumbers;
        PlusMinusButtons.onFieldSizeChange += SetupNumbers;
    }
    private void OnDisable()
    {
        MobileCanvasSwapper.onCanvasSwap -= SetupNumbers;
        PlusMinusButtons.onFieldSizeChange -= SetupNumbers;
    }

    public static void ChangeSize(string Dimension, bool IsPositive)
    {
        switch (Dimension)
        {
            case "string":
                {
                    if (IsPositive)
                    {
                        if (RememberString >= 50)
                            return;
                        RememberString++;
                    }
                    else
                    {
                        if (RememberString <= 7)
                            return;
                        RememberString--;
                    }
                    break;
                }
            case "colomn":
                {
                    if (IsPositive)
                    {
                        if (RememberColomn >= 50)
                            return;
                        RememberColomn++;
                    }
                    else
                    {
                        if (RememberColomn <= 7)
                            return;
                        RememberColomn--;
                    }
                    break;
                }
            case "both":
                {
                    if (IsPositive)
                    {
                        if (RememberString < 50)
                            RememberString++;
                        RememberColomn = RememberString;
                    }
                    else
                    {
                        if (RememberString>7)
                            RememberString--;
                        RememberColomn = RememberString;
                    }
                    break;
                }
        }
        onSizeChange?.Invoke();
    }
    private void SetupNumbers()
    {
        _fGComponent.ColomnsSize = RememberColomn;
        _fGComponent.StringsSize = RememberString;
        if (SizeIndex == 0)
        {
            _inputField.text = Convert.ToString(RememberString);
        }
        else if (SizeIndex == 1)
        {
            _inputField.text = Convert.ToString(RememberColomn);
        }        
    }
    public void ChangeBoth(string Helper)
    {
        string Size = _inputField.text;
        if (Size == "")
        {
            return;
        }
        int IntSize = Convert.ToInt32(Size);
        _fGComponent.ColomnsSize = IntSize;
        _fGComponent.StringsSize = IntSize;
        RememberString = IntSize;
        RememberColomn = IntSize;
        _inputField.text = Convert.ToString(IntSize);
    }
    public void EditEndBoth(string Helper)
    {
        string Size = _inputField.text;
        if (Size == "")
        {
            Size = "10";
        }
        int IntSize = Convert.ToInt32(Size);
        IntSize = ConstrainInt(IntSize);
        _fGComponent.ColomnsSize = IntSize;
        _fGComponent.StringsSize = IntSize;
        RememberString = IntSize;
        RememberColomn = IntSize;
        onSizeChange?.Invoke();
        _inputField.text = Convert.ToString(IntSize);
    }
    public void ChangeColomnsSize(string Helper)
    {
        string ColomnsSize = _inputField.text;
        if (ColomnsSize=="")
        {
            return;
        }
        int IntColomnSize = Convert.ToInt32(ColomnsSize);
        _fGComponent.ColomnsSize = IntColomnSize;
        RememberColomn = IntColomnSize;
        _inputField.text =Convert.ToString(IntColomnSize);
    }
    public void EditEndColomnsSize(string Helper)
    {
        string ColomnsSize = _inputField.text;
        if (ColomnsSize == "")
        {
            ColomnsSize = "10";
        }
        int IntColomnSize = Convert.ToInt32(ColomnsSize);
        IntColomnSize = ConstrainInt(IntColomnSize);
        _fGComponent.ColomnsSize = IntColomnSize;
        RememberColomn = IntColomnSize;
        onSizeChange?.Invoke();
        _inputField.text = Convert.ToString(IntColomnSize);
    }
    public void ChangeStringsSize(string Helper)
    {
        string StringsSize = _inputField.text;
        if (StringsSize == "")
        {
            return;
        }
        int IntStringsSize = Convert.ToInt32(StringsSize);
        _fGComponent.StringsSize = IntStringsSize;
        RememberString = IntStringsSize;
        _inputField.text = Convert.ToString(IntStringsSize);
    }
    public void EditEndStringsSize(string Helper)
    {
        string StringsSize = _inputField.text;
        if (StringsSize == "")
        {
            StringsSize = "10";
        }
        int IntStringsSize = Convert.ToInt32(StringsSize);
        IntStringsSize = ConstrainInt(IntStringsSize);
        _fGComponent.StringsSize = IntStringsSize;
        RememberString = IntStringsSize;
        onSizeChange?.Invoke();
        _inputField.text = Convert.ToString(IntStringsSize);
    }
    private int ConstrainInt(int CurrentNumber)
    {
        int ConstraintNumber = CurrentNumber;
        if(ConstraintNumber <7 )
        {
            ConstraintNumber = 7;
        }
        else if(ConstraintNumber > 50 )
        {
            ConstraintNumber = 50;
        }
        return ConstraintNumber;
    }
}