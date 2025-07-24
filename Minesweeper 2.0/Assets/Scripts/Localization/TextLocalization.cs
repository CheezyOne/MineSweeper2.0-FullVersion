using TMPro;
using UnityEngine;

public class TextLocalization : MonoBehaviour
{
    [SerializeField] string[] Texts; //English text comes first
    private TMP_Text MyTMP;
    private void Awake()
    {
        if (GetComponent<TMP_Text>() != null)
            MyTMP = GetComponent<TMP_Text>();
        else
            Debug.Log("Localization error: can't get my TMP component, i'm " + gameObject);

    }
    private void OnEnable()
    {
        LanguageController.onLanguageChange += ChangeText;
        ChangeText();
    }
    private void OnDisable()
    {
        LanguageController.onLanguageChange -= ChangeText;
    }
    private void ChangeText()
    {
        switch (LanguageController.CurrentLanguage)
        {
            case "English":
                {
                    MyTMP.text = Texts[0];
                    break;
                }
            case "en":
                {
                    MyTMP.text = Texts[0];
                    break;
                }
            case "Russian":
                {
                    MyTMP.text = Texts[1];
                    break;
                }
            case "ru":
                {
                    MyTMP.text = Texts[1];
                    break;
                }
            default:
                {
                    break;
                }
        }
    }
}
