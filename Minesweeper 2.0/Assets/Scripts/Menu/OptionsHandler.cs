using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsHandler : MonoBehaviour
{
    [SerializeField] private GameObject MainDesktopCanvas, MainMobileCanvas;
    [SerializeField] private GameObject Canvas;
    public void EscapeButton()
    {
        if (MobileCanvasSwapper.IsNowDesktop)
            MainDesktopCanvas.SetActive(true);
        else
            MainMobileCanvas.SetActive(true);
        Canvas.SetActive(false);
    }
    public void OpenOptions()
    {
        if (MobileCanvasSwapper.IsNowDesktop)
            MainDesktopCanvas.SetActive(false);
        else
            MainMobileCanvas.SetActive(false);
        Canvas.SetActive(true);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EscapeButton();
        }
    }
}
