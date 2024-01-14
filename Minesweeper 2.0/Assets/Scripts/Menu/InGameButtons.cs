using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameButtons : MonoBehaviour
{
    public static Action onGameExit;
    [SerializeField] private GameObject MainMenu, SmileyController, AssurenceFrame;

    public static Action PlaySound;
    public void GameExit()
    {
        PlaySound?.Invoke();
        if (ClickRegister.isGameOn)
        {
            AssurenceFrame.SetActive(true);
            return;
        }
        onGameExit?.Invoke();
        Camera.main.transform.Rotate(-90, 0, 0);
        Camera.main.transform.position =new Vector3(0,10.5f,0);
        MainMenu.SetActive(true);
        SmileyController.SetActive(false);
        gameObject.SetActive(false);
        AssurenceFrame.SetActive(false);
    }
    public void YesButton()
    {
        onGameExit?.Invoke();
        Camera.main.transform.Rotate(-90, 0, 0);
        Camera.main.transform.position = new Vector3(0, 10.5f, 0);
        MainMenu.SetActive(true);
        SmileyController.SetActive(false);
        gameObject.SetActive(false);
        AssurenceFrame.SetActive(false);
        PlaySound?.Invoke();
    }
    public void NoButton() 
    {
        AssurenceFrame.SetActive(false);
        PlaySound?.Invoke();
    }
    public void RestartButton()
    {
        PlaySound?.Invoke();
        onGameExit?.Invoke();
        Camera.main.transform.position = new Vector3(FieldGeneration.StaticStringsSize * 1.1f - (FieldGeneration.StaticStringsSize * 1.1f / 2) - 5.6f, 10.5f, FieldGeneration.StaticColomsSize * 1.1f - 2);
        ButtonsInMenu.onPlayButtonPress?.Invoke();
        MovingCamera.isAbleToMove = true;
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            GameExit();
        }
    }
}
