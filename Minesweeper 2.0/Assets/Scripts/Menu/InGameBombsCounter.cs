using TMPro;
using UnityEngine;

public class InGameBombsCounter : MonoBehaviour
{
    public static int BombsCount=0, PlayersBombsCount=0;
    [SerializeField] private TMP_Text BombsCountText;
    private void OnEnable()
    {
        InGameButtons.onGameExit += NullTheCounter;
    }
    private void OnDisable()
    {
        InGameButtons.onGameExit -= NullTheCounter;
    }
    private void NullTheCounter()
    {
        BombsCount = 0;
        PlayersBombsCount = 0;
        BombsCountText.text = PlayersBombsCount + "/" + BombsCount;
    }
    private void Update()
    {
        BombsCountText.text = PlayersBombsCount + "/" + BombsCount;
    }
}
