using TMPro;
using UnityEngine;
using UnityEditor;

public class Test : MonoBehaviour
{
    public TMP_FontAsset newFont; // ���������� ���� ����� ����� � ����������

    [ContextMenu("Replace Fonts")]
    public void ReplaceFonts()
    {
        if (newFont == null)
        {
            Debug.LogError("New font is not assigned!");
            return;
        }

        // ������� ��� ���������� TMP_Text � TMP_InputField �� �����
        TMP_Text[] allTexts = FindObjectsOfType<TMP_Text>(true);
        TMP_InputField[] allInputFields = FindObjectsOfType<TMP_InputField>(true);

        int replacedCount = 0;

        // �������� ����� � TMP_Text
        foreach (TMP_Text text in allTexts)
        {
            text.font = newFont;
            replacedCount++;
        }

        // �������� ����� � TMP_InputField (��� ���� ���� textComponent)
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
