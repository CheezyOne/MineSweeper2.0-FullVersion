using UnityEngine;
using UnityEngine.UI;
using TMPro;
using YG;

public class ContinueWindow : BaseWindow
{
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private float _constTimerTime;
    [SerializeField] private Image _timerImage;
    [SerializeField] private Toggle _dontShowAgainToggle;
    [SerializeField] private int _continueRewardID;

    private float _timerTime;

    public static bool DontShowAgain;

    public override void Init()
    {
        ClickRegister.isGameOn = false;
        _timerTime = _constTimerTime;
    }

    private void Update()
    {
        if (_timerTime <= 0)
        {
            DeclineAd();
            return;
        }

        _timerTime-=Time.deltaTime;
        _timerImage.fillAmount = _timerTime / _constTimerTime;
        _timerText.text = Mathf.Floor(_timerTime).ToString();
    }

    public void OnYesButton()
    {
        EventBus.OnButtonClick?.Invoke();
        YandexGame.RewVideoShow(_continueRewardID);
        WindowsManager.Instance.CloseCurrentWindow();
    }

    private void DeclineAd()
    {
        EventBus.OnGameLose?.Invoke();
        EventBus.OnChangeSmiley.Invoke(7);
        WindowsManager.Instance.CloseCurrentWindow();
    }

    public void OnNoButton()
    {
        EventBus.OnButtonClick?.Invoke();
        DeclineAd();
    }

    public override void OnClose()
    {
        if (_dontShowAgainToggle.isOn)
            DontShowAgain = true;
    }
}