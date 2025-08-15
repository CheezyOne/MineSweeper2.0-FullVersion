using UnityEngine;
using YG;

public class TutorialWindow : BaseWindow
{
    [SerializeField] private GameObject[] _tutorialTexts;
    [SerializeField] private Transform[] _handPositions;
    [SerializeField] private Transform _hand;

    private int _tutorialIndex;

    public override void Init()
    {
        base.Init();
        NextTutorial();
    }

    public void NextTutorial()
    {
        EventBus.OnButtonClick?.Invoke();

        if (_tutorialIndex == _handPositions.Length)
        {
            YandexGame.savesData.hasSeenTutorial = true;
            WindowsManager.Instance.CloseCurrentWindow();
            return;
        }

        if(_tutorialIndex>0)
            _tutorialTexts[_tutorialIndex-1].SetActive(false);

        _tutorialTexts[_tutorialIndex].SetActive(true);
        _hand.position = _handPositions[_tutorialIndex].position;
        _tutorialIndex++;
    }
}