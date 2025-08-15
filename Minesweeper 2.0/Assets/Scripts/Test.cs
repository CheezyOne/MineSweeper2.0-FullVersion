using TMPro;
using UnityEngine;
using UnityEditor;

public class Test : MonoBehaviour
{
    public TMP_FontAsset newFont; // Перетащите сюда новый шрифт в инспекторе

    [ContextMenu("Replace Fonts")]
    public void ReplaceFonts()
    {
        if (newFont == null)
        {
            Debug.LogError("New font is not assigned!");
            return;
        }

        // Находим все компоненты TMP_Text и TMP_InputField на сцене
        TMP_Text[] allTexts = FindObjectsOfType<TMP_Text>(true);
        TMP_InputField[] allInputFields = FindObjectsOfType<TMP_InputField>(true);

        int replacedCount = 0;

        // Заменяем шрифт в TMP_Text
        foreach (TMP_Text text in allTexts)
        {
            text.font = newFont;
            replacedCount++;
        }

        // Заменяем шрифт в TMP_InputField (там есть поле textComponent)
        foreach (TMP_InputField inputField in allInputFields)
        {
            if (inputField.textComponent != null)
            {
                inputField.textComponent.font = newFont;
                replacedCount++;
            }
        }

        Debug.Log($"Replaced font in {replacedCount} objects.");
    }
}
