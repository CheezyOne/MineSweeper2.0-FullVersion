using TMPro;
using UnityEngine;
using UnityEngine.Video;

public enum Language
{
    English,
    Russian,
}

public class GameplayTutorialWindow : BaseWindow
{
    [SerializeField] private string[] _englishTexts;
    [SerializeField] private string[] _russianTexts;
    [SerializeField] private TMP_Text _tutorialText;
    [SerializeField] private TMP_Text _currentTutorialNumberText;
    [SerializeField] private string[] _videoURLs;
    [SerializeField] private VideoPlayer _videoPlayer;

    private Language _language = Language.Russian;
    private int _tutorialIndex;

    private void OnEnable()
    {
        SetLanguage();
        SetText();
    }

    private void SetText()
    {
        _tutorialIndex = 0;
        ChangeTutorial();
    }

    public void ChangeTutorial()
    {
        EventBus.OnButtonClick?.Invoke();

        if (_tutorialIndex >= _englishTexts.Length)
        {
            WindowsManager.Instance.CloseCurrentWindow();
            return;
        }

        _videoPlayer.url = _videoURLs[_tutorialIndex];
        _videoPlayer.Play();
        _tutorialText.text = _language == Language.Russian? _russianTexts[_tutorialIndex] : _englishTexts[_tutorialIndex];
        _tutorialIndex++;
        _currentTutorialNumberText.text = _tutorialIndex + "/" + _englishTexts.Length;
    }

    private void SetLanguage()
    {
        switch (LanguageController.CurrentLanguage)
        {
            case "English":
                {
                    _language = Language.English;
                    break;
                }
            case "en":
                {
                    _language = Language.English;
                    break;
                }
            case "Russian":
                {
                    _language = Language.Russian;
                    break;
                }
            case "ru":
                {
                    _language = Language.Russian;
                    break;
                }
            default:
                {
                    _language = Language.English;
                    break;
                }
        }
    }
}