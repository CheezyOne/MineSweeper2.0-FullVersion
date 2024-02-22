using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class MobileButtonsSet : MonoBehaviour
{
    [SerializeField] private GameObject[] MobileButtons;
    private void Awake()
    {
        if(YandexGame.EnvironmentData.isMobile)
        {
            MovingCamera.isMobileMovement = true;
            HighLightCells.IsMobile = true;
            ClickRegister.isMobile = true;
            for(int i=0; i<MobileButtons.Length; i++)
            {
                MobileButtons[i].SetActive(true);
            }
        }    

    }
}
