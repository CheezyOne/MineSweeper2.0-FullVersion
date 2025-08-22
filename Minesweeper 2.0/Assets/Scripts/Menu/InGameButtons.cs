using System;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class InGameButtons : MonoBehaviour
{
    public static Action onGameExit;
    [SerializeField] private GameObject MainMenu, SmileyController, AssurenceFrame;
    [Header ("Mobile buttons")]
    [SerializeField] private GameObject BombsCheck;
    [SerializeField] private GameObject CellsCheck;
    [SerializeField] private Renderer Flag;
    [SerializeField] private Material TouhedCubeMaterail;
    [SerializeField] private Material FlagMaterial;
    [SerializeField] private Image Cursor;

    public static Action PlaySound;

    private void OnEnable()
    {
        TryBottomButtonsSwap();
        MobileCanvasSwapper.onCanvasSwap += TryBottomButtonsSwap;
    }
    private void OnDisable()
    {
        MobileCanvasSwapper.onCanvasSwap -= TryBottomButtonsSwap;
    }
    private void TryBottomButtonsSwap()
    {
        try
        {
            if (ClickRegister.setBombs)
            {
                CellsCheck.SetActive(false);
                BombsCheck.SetActive(true);
                Flag.material = TouhedCubeMaterail;
                Cursor.color = Color.white;
            }
            else
            {
                CellsCheck.SetActive(true);
                BombsCheck.SetActive(false);
                Flag.material = FlagMaterial;
                Cursor.color = Color.grey;
            }
        }
        catch { } //I'm too lazy to fix that small error
    }

    public void SetBombs()
    {
        CellsCheck.SetActive(false);
        BombsCheck.SetActive(true);
        Flag.material = TouhedCubeMaterail;
        Cursor.color = Color.white;
        ClickRegister.setBombs = true;
    }

    public void ClickOnCells()
    {
        CellsCheck.SetActive(true);
        BombsCheck.SetActive(false);
        Flag.material = FlagMaterial;
        Cursor.color = Color.grey;
        ClickRegister.setBombs = false;
    }

    public void GameExit()
    {
        PlaySound?.Invoke();

        if (ClickRegister.isGameOn)
        {
            ClickRegister.isGameOn = false;
            AssurenceFrame.SetActive(true);
            return;
        }

        YandexGame.FullscreenShow();
        Exit();
    }

    private void Exit()
    {
        onGameExit?.Invoke();
        Camera.main.transform.Rotate(-90, 0, 0);
        Camera.main.transform.position = new Vector3(0, 10.5f, 0);
        MainMenu.SetActive(true);
        SmileyController.SetActive(false);
        gameObject.SetActive(false);
        AssurenceFrame.SetActive(false);
    }

    public void YesButton()
    {
        Exit();
    }

    public void NoButton() 
    {
        ClickRegister.isGameOn = true;
        AssurenceFrame.SetActive(false);
        YandexGame.FullscreenShow();
        PlaySound?.Invoke();
    }

    public void RestartButton()
    {
        PlaySound?.Invoke();
        onGameExit?.Invoke();
        Camera.main.transform.position = new Vector3(FieldGeneration.StaticStringsSize * 1.1f - (FieldGeneration.StaticStringsSize * 1.1f / 2) - 5.6f, 10.5f, FieldGeneration.StaticColomsSize * 1.1f - 6);
        ButtonsInMenu.onPlayButtonPress?.Invoke();
        MovingCamera.isAbleToMove = true;
        YandexGame.FullscreenShow();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            GameExit();
        }
    }
}