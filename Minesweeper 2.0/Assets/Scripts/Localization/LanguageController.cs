using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageController : MonoBehaviour
{
    public static string CurrentLanguage;
    public static Action onLanguageChange, PlaySound;
    private void Awake()
    {
        if (!PlayerPrefs.HasKey("Language"))
        {
            if (Application.systemLanguage == SystemLanguage.Russian || Application.systemLanguage == SystemLanguage.Ukrainian|| Application.systemLanguage == SystemLanguage.Belarusian)
            {
                PlayerPrefs.SetString("Language", "Russian");
            }
            else
            {
                PlayerPrefs.SetString("Language", "English");
            }
        }
        LanguageController.CurrentLanguage = PlayerPrefs.GetString("Language");
        onLanguageChange?.Invoke();
    }
    public void ChangeLanguageButton(string Language)
    {
        PlaySound?.Invoke();
        CurrentLanguage = Language;
        PlayerPrefs.SetString("Language", Language);
        onLanguageChange?.Invoke();
    }
}
