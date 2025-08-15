using System;
using UnityEngine;
using YG;

public class YandexLB : MonoBehaviour
{
    [SerializeField] private GameObject MinesApplier, MovingBombsHandler;
    [SerializeField] private string _leaderboardName;

    private int HighScore;
    
    private void OnEnable()
    {
        VictoryHandler.onGameWon += AddScoreToLeaderboard;
    }
    private void OnDisable()
    {
        VictoryHandler.onGameWon -= AddScoreToLeaderboard;
    }
    private void Start()
    {
        HighScore =YandexGame.savesData.highScore;
    }
    private float CountScore()
    {
        int ChallengeCount = 0;
        HighScore = YandexGame.savesData.highScore;
        ApplyMines APComponent = MinesApplier.GetComponent<ApplyMines>();
        double BombRercentage = (Convert.ToDouble(APComponent.BlueBombCount) + Convert.ToDouble(APComponent.RedBombCount)) / Convert.ToDouble(APComponent.FieldSize);
        if (BombRercentage <= 0.09)
            return 0;
        if (BombRercentage > 0.5)
            BombRercentage = 0.5;
        float AddScore = APComponent.FieldSize;
        if (ApplyMines.ApplyBlueBombs)
        {
            ChallengeCount++;
            AddScore *= 3;
        }
        if (Cell.ShouldLie)
        {
            ChallengeCount++;
            AddScore *= 2;
        }
        if (FieldGeneration.ApplyBariers)
        {
            ChallengeCount++;
            AddScore *= 4;
        }
        if (MovingBombsHandler.activeSelf)
        {
            ChallengeCount++;
            AddScore *= 2;
        }
        if (InGameTimer.IsToDecrease)
        {
            ChallengeCount++;
            AddScore *= 4;
        }
        AddScore = (float)(AddScore * BombRercentage);
        if (ChallengeCount > 0)
            AddScore /= ChallengeCount;
        return AddScore;
    }
    private void AddScoreToLeaderboard()
    {
        HighScore += Convert.ToInt32(CountScore());
        YandexGame.savesData.highScore = HighScore;
        YandexGame.SaveProgress();
        YandexGame.NewLeaderboardScores(_leaderboardName, HighScore);
    }
}
