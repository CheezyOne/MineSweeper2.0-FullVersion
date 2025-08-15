using UnityEngine;
using YG;

public class YGRewardedAd : MonoBehaviour
{
    private void OnEnable()
    {
        YandexGame.RewardVideoEvent += GetReward;
    }

    private void OnDisable()
    {
        YandexGame.RewardVideoEvent -= GetReward;
    }

    private void GetReward(int rewardIndex)
    {
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