using UnityEngine;
using YG;
using System.Collections;

public class YGRewardedAd : MonoBehaviour
{
    private bool _isRewardGranted;
    private WaitForSeconds _closedAdRoutineWait = new(0.1f);

    private void OnEnable()
    {
        YandexGame.RewardVideoEvent += GetReward;
    }

    private void OnDisable()
    {
        YandexGame.RewardVideoEvent -= GetReward;
    }

    public void DenyReward()
    {
        EventBus.OnGameLose?.Invoke();
    }

    public void OnOpenVideo()
    {
        _isRewardGranted = false; 
    }

    public void OnCloseVideo()
    {
        StartCoroutine(CloseAdRoutine());
    }

    private IEnumerator CloseAdRoutine()
    {
        yield return _closedAdRoutineWait;

        if (!_isRewardGranted)
            EventBus.OnGameLose?.Invoke();
    }

    private void GetReward(int rewardIndex)
    {
        _isRewardGranted = true;

        switch (rewardIndex)
        {
            case 0:
                {
                    YGMetrics.ReportToYandexMetrika("Watched_ad");
                    ClickRegister.isGameOn = true;
                    return;
                }
        }
    }
}