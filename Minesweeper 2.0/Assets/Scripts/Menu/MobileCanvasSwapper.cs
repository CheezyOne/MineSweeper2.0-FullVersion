using System;
using UnityEngine;
using UnityEngine.UI;

public class MobileCanvasSwapper : MonoBehaviour
{
    [SerializeField] private GameObject[] MainDesktopCanvas, MainVerticalCanvas, InGameDesktopCanvas, InGameVerticalCanvas;
    [SerializeField] private GameObject[] DesktopChecks, MobileChecks;
    [SerializeField] private Transform SmileyController;
    [SerializeField] private GameObject SoundManager;
    private readonly Vector3 NormalSmileyScale = new Vector3(0.07f, 0.07f, 0.07f), MobileSmileyScale = new Vector3(0.05f, 0.05f, 0.05f);
    public static Action onCanvasSwap;
    public static bool IsNowDesktop = true;
    private void Start()
    {
        if (Application.isMobilePlatform && Screen.orientation == ScreenOrientation.Portrait)
            MainVerticalCanvas[0].SetActive(true);
        else
            MainDesktopCanvas[0].SetActive(true);
    }
    private void Update() //This script is pure shit
    {
        if (!Application.isMobilePlatform)
            return;
        if (!IsNowDesktop && (Screen.orientation == ScreenOrientation.LandscapeLeft || Screen.orientation == ScreenOrientation.LandscapeRight))
        {
            SmileyController.localScale = NormalSmileyScale;
            SoundManager.SetActive(false);
            IsNowDesktop = true;
            if (MainVerticalCanvas[0].activeSelf)
                ToMainDesktopCanvasSwap();
            else if (InGameVerticalCanvas[0].activeSelf)
                ToInGameDesktopCanvasSwap();
            SoundManager.SetActive(true);
            for (int i=0;i<5;i++)
            {
                if (MainVerticalCanvas[i+1].transform.GetChild(0).gameObject.activeSelf)
                    DesktopChecks[i].SetActive(true);//And setup checks in case of swapping in options
            }
        }
        else if (IsNowDesktop && Screen.orientation == ScreenOrientation.Portrait)
        {
            SmileyController.localScale = MobileSmileyScale;
            SoundManager.SetActive(false);
            IsNowDesktop = false;
            if (MainDesktopCanvas[0].activeSelf)
                ToMainMobileCanvasSwap();
            else if (InGameDesktopCanvas[0].activeSelf)
                ToInGameMobileCanvasSwap();
            SoundManager.SetActive(true);
            for (int i = 0; i < 5; i++)
            {
                if (MainDesktopCanvas[i + 1].transform.GetChild(0).gameObject.activeSelf)
                    MobileChecks[i].SetActive(true);//And setup checks in case of swapping in options
            }
        }
    }
    private void ToInGameMobileCanvasSwap()
    {
        InGameDesktopCanvas[0].SetActive(false);
        InGameVerticalCanvas[0].SetActive(true);
        if (InGameDesktopCanvas[1].activeSelf)//Exit assurence 
        {
            InGameDesktopCanvas[1].SetActive(false);
            InGameVerticalCanvas[1].SetActive(true);
        }
        onCanvasSwap?.Invoke();
    }
    private void ToInGameDesktopCanvasSwap()
    {
        InGameDesktopCanvas[0].SetActive(true);
        InGameVerticalCanvas[0].SetActive(false);
        if (InGameVerticalCanvas[1].activeSelf)//Exit assurence 
        {
            InGameDesktopCanvas[1].SetActive(true);
            InGameVerticalCanvas[1].SetActive(false);
        }
        onCanvasSwap?.Invoke();
    }
    private void ToMainDesktopCanvasSwap()
    {
        MainVerticalCanvas[0].SetActive(false);
        MainDesktopCanvas[0].SetActive(true);//Canvas first
        for(int i=1;i<6;i++)
        {
            if (MainVerticalCanvas[i].transform.GetChild(0).gameObject.activeSelf)//Then its checkboxes
            {
                MainVerticalCanvas[i].GetComponent<Button>().onClick.Invoke();
                MainDesktopCanvas[i].GetComponent<Button>().onClick.Invoke();
            }
        }
        onCanvasSwap?.Invoke();
    }
    private void ToMainMobileCanvasSwap()
    {
        MainVerticalCanvas[0].SetActive(true);
        MainDesktopCanvas[0].SetActive(false);
        for (int i = 1; i < 6; i++)
        {
            if (MainDesktopCanvas[i].transform.GetChild(0).gameObject.activeSelf)//Then its checkboxes
            {
                MainVerticalCanvas[i].GetComponent<Button>().onClick.Invoke();
                MainDesktopCanvas[i].GetComponent<Button>().onClick.Invoke();
                MobileChecks[i - 1].SetActive(true);//And setup checks in case of swapping in options
            }
        }
        onCanvasSwap?.Invoke();
    }
}
