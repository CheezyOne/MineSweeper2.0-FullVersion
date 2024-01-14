using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InGameBombsCounter : MonoBehaviour
{
    public static int BombsCount=0, PlayersBombsCount=0;
    [SerializeField] private TMP_Text BombsCountText;
    // Start is called before the first frame update
    private void Awake()
    {
        InGameButtons.onGameExit += NullTheCounter;
    }
    private void NullTheCounter()
    {
        InGameBombsCounter.BombsCount = 0;
        InGameBombsCounter.PlayersBombsCount = 0;
    }
    private void Update()
    {
        BombsCountText.text = PlayersBombsCount + "/" + BombsCount;
    }
}
