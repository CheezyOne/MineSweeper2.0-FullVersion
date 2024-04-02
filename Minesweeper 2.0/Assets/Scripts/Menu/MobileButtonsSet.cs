using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class MobileButtonsSet : MonoBehaviour
{
    [SerializeField] private GameObject[] MobileButtons;
    private void Start()
    {
        CheckMobile();
        StartCoroutine(SecondTimeCheck());
    }
    private void CheckMobile()
    {
        if (YandexGame.EnvironmentData.isMobile)
        {
            MovingCamera.isMobileMovement = true;
            HighLightCells.IsMobile = true;
            ClickRegister.isMobile = true;
            for (int i = 0; i < MobileButtons.Length; i++)
            {
                MobileButtons[i].SetActive(true);
            }
        }
    }
    private IEnumerator SecondTimeCheck()//This is here just in case that some browser doesn't get that we play on mobile instantly
    {
        yield return new WaitForSeconds(0.5f);
        CheckMobile();
        yield break;
    }
}
