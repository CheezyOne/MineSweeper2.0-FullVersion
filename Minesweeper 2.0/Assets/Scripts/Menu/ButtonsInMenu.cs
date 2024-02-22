using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsInMenu : MonoBehaviour
{
    public static Action onPlayButtonPress, PlaySound;
    private readonly string[] EnglishMapNames = new string[] { "Normal", "Cross", "4 Sides", "Hole", "Diamond" };
    private readonly string[] RussianMapNames = new string[] { "Обычный", "Крест", "4 Стороны", "Дыра", "Ромб" };
    private string[] MapNames = new string[] { "Normal", "Cross", "4 Sides", "Hole", "Diamond" };
    [SerializeField] private GameObject MapNameText, MapNameChangeRight, MapNameChangeLeft, InGameMenu, SmileyPosition, TwoInput, FullInput;
    private static int MapNameNumber = 0;
    private void Awake()
    {
        LanguageController.onLanguageChange += ChangeLanguage;
        ChangeLanguage();
    }

    private void ChangeLanguage()
    {
        if(LanguageController.CurrentLanguage=="English"|| LanguageController.CurrentLanguage == "en")
        {
            MapNames = EnglishMapNames;
        }
        else if(LanguageController.CurrentLanguage == "Russian" || LanguageController.CurrentLanguage == "ru")
        { 
            MapNames = RussianMapNames;
        }
        SetMapNameText();
    }
    public void PlayButton()
    {
        PlaySound?.Invoke();
        Camera.main.transform.Rotate(90, 0, 0);
        Camera.main.transform.position = new Vector3(FieldGeneration.StaticStringsSize * 1.1f - (FieldGeneration.StaticStringsSize * 1.1f / 2) - 5.6f, 10.5f, FieldGeneration.StaticColomsSize * 1.1f - 6);
        //SmileyPosition.transform.position= new Vector3(FieldGeneration.StaticStringsSize * 1.1f - (FieldGeneration.StaticStringsSize * 1.1f / 2) - 5.6f, 0, FieldGeneration.StaticColomsSize * 1.1f);
        SmileyPosition.SetActive(true);
        onPlayButtonPress?.Invoke();
        gameObject.SetActive(false);
        InGameMenu.SetActive(true);
        MovingCamera.isAbleToMove = true;
    }
    public void ChangeMapNameLeft()
    {
        PlaySound?.Invoke();
        if (MapNameNumber != 0)
        {
            MapNameChangeRight.SetActive(true);
            MapNameNumber--;
            SetMapNameText();
            if (MapNameNumber == 0)
            {
                MapNameChangeLeft.SetActive(false);
            }
        }
    }
    private void SetInputs(bool DoubleInput)
    {
        if (DoubleInput)
        {
            FullInput.SetActive(false);
            TwoInput.SetActive(true);
        }
        else
        {
            TwoInput.SetActive(false);
            FullInput.SetActive(true);
        }
    }
    private void SetMapNameText()
    {
        MapNameText.GetComponent<TextMeshProUGUI>().text = MapNames[MapNameNumber];
        FieldGeneration.FieldType = EnglishMapNames[MapNameNumber];
        if (MapNames[MapNameNumber] == "4 Sides" || MapNames[MapNameNumber] == "Diamond"|| MapNames[MapNameNumber] == "4 Стороны" || MapNames[MapNameNumber] == "Ромб")
            SetInputs(false);
        else
            SetInputs(true);
    }    
    public void ChangeMapNameRight()
    {
        PlaySound?.Invoke();
        if (MapNameNumber != MapNames.Length-1)
        {
            MapNameChangeLeft.SetActive(true);
            MapNameNumber++;
            SetMapNameText();
            if (MapNameNumber == MapNames.Length-1)
            {
                MapNameChangeRight.SetActive(false);
            }
        }
    }
}
