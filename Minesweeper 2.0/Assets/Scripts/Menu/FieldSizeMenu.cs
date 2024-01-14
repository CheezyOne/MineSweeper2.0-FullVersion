using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FieldSizeMenu : MonoBehaviour
{
    [SerializeField] private GameObject _fieldGenerator;
    private FieldGeneration _fGComponent;
    private void Awake()
    {
        _fGComponent = _fieldGenerator.GetComponent<FieldGeneration>();
    }
    private void OnEnable()
    {
        _fGComponent.ColomnsSize = 5;
        _fGComponent.StringsSize = 5;
        GetComponent<TMP_InputField>().text = "5";
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
        GetComponent<TMP_InputField>().text = Convert.ToString(IntSize);
    }
    public void EditEndBoth(string Size)
    {
        if (Size == "")
        {
            Size = "5";
        }
        int IntSize = Convert.ToInt32(Size);
        IntSize = ConstrainInt(IntSize);
        _fGComponent.ColomnsSize = IntSize;
        _fGComponent.StringsSize = IntSize;
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
        GetComponent<TMP_InputField>().text =Convert.ToString(IntColomnSize);
    }
    public void EditEndColomnsSize(string ColomnsSize)
    {
        if (ColomnsSize == "")
        {
            ColomnsSize = "5";
        }
        int IntColomnSize = Convert.ToInt32(ColomnsSize);
        IntColomnSize = ConstrainInt(IntColomnSize);
        _fGComponent.ColomnsSize = IntColomnSize;
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
        GetComponent<TMP_InputField>().text = Convert.ToString(IntStringsSize);
    }
    public void EditEndStringsSize(string StringsSize)
    {
        if (StringsSize == "")
        {
            StringsSize = "5";
        }
        int IntStringsSize = Convert.ToInt32(StringsSize);
        IntStringsSize = ConstrainInt(IntStringsSize);
        _fGComponent.StringsSize = IntStringsSize;
        GetComponent<TMP_InputField>().text = Convert.ToString(IntStringsSize);
    }
    private int ConstrainInt(int CurrentNumber)
    {
        int ConstraintNumber = CurrentNumber;
        if(ConstraintNumber <5 )
        {
            ConstraintNumber = 5;
        }
        else if(ConstraintNumber > 50 )
        {
            ConstraintNumber = 50;
        }
        return ConstraintNumber;
    }
}
