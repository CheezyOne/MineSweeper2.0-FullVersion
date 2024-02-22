using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FieldSizeMenu : MonoBehaviour
{
    [SerializeField] private GameObject _fieldGenerator;
    private FieldGeneration _fGComponent;
    private static int _rememberString=7, _rememberColomn=7;
    [SerializeField] private int SizeIndex;//string, colomn
    private void Awake()
    {
        _fGComponent = _fieldGenerator.GetComponent<FieldGeneration>();
    }
    private void OnEnable()
    {
        _fGComponent.ColomnsSize = _rememberColomn;
        _fGComponent.StringsSize = _rememberString;
        if (SizeIndex == 0)
            GetComponent<TMP_InputField>().text = Convert.ToString(_rememberString);
        else if(SizeIndex==1)
            GetComponent<TMP_InputField>().text = Convert.ToString(_rememberColomn);
    }
    public void ChangeBoth(string Size)
    {
        if (Size == "")
        {
            return;
        }
        int IntSize = Convert.ToInt32(Size);
        _fGComponent.ColomnsSize = IntSize;
        _fGComponent.StringsSize = IntSize;
        _rememberString = IntSize;
        _rememberColomn = IntSize;
        GetComponent<TMP_InputField>().text = Convert.ToString(IntSize);
    }
    public void EditEndBoth(string Size)
    {
        if (Size == "")
        {
            Size = "7";
        }
        int IntSize = Convert.ToInt32(Size);
        IntSize = ConstrainInt(IntSize);
        _fGComponent.ColomnsSize = IntSize;
        _fGComponent.StringsSize = IntSize;
        _rememberString = IntSize;
        _rememberColomn = IntSize;
        GetComponent<TMP_InputField>().text = Convert.ToString(IntSize);
    }
    public void ChangeColomnsSize(string ColomnsSize)
    {
        if(ColomnsSize=="")
        {
            return;
        }
        int IntColomnSize = Convert.ToInt32(ColomnsSize);
        _fGComponent.ColomnsSize = IntColomnSize;
        _rememberColomn = IntColomnSize;
        GetComponent<TMP_InputField>().text =Convert.ToString(IntColomnSize);
    }
    public void EditEndColomnsSize(string ColomnsSize)
    {
        if (ColomnsSize == "")
        {
            ColomnsSize = "7";
        }
        int IntColomnSize = Convert.ToInt32(ColomnsSize);
        IntColomnSize = ConstrainInt(IntColomnSize);
        _fGComponent.ColomnsSize = IntColomnSize;
        _rememberColomn = IntColomnSize;
        GetComponent<TMP_InputField>().text = Convert.ToString(IntColomnSize);
    }
    public void ChangeStringsSize(string StringsSize)
    {
        if (StringsSize == "")
        {
            return;
        }
        int IntStringsSize = Convert.ToInt32(StringsSize);
        _fGComponent.StringsSize = IntStringsSize;
        _rememberString = IntStringsSize;
        GetComponent<TMP_InputField>().text = Convert.ToString(IntStringsSize);
    }
    public void EditEndStringsSize(string StringsSize)
    {
        if (StringsSize == "")
        {
            StringsSize = "7";
        }
        int IntStringsSize = Convert.ToInt32(StringsSize);
        IntStringsSize = ConstrainInt(IntStringsSize);
        _fGComponent.StringsSize = IntStringsSize;
        _rememberString = IntStringsSize;
        GetComponent<TMP_InputField>().text = Convert.ToString(IntStringsSize);
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