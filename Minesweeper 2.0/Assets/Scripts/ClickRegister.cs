using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickRegister : MonoBehaviour
{
    public static bool isGameOn = false, isMobile = false, setBombs=false; 
    private bool theFirstCube = true;
    public static Action onFirstCubeTouch, PlaySound;
    private GameObject touchedCube;

    private const float constTouchedTime=0.7f;//Specifically for mobile gameplay;
    private float touchedTime = 0f;
    private bool isTouchingCube = false, shouldNotClick = false;
    //These Actions are for smileys
    public static Action<int> onCubeTouch, onCubeRelease;

    private void Start()
    {
        InGameButtons.onGameExit += NullAllVariables;
    }
    private void NullAllVariables()
    {
        isGameOn = false;
        theFirstCube = true;
    }
    private void NullTouchingVariables()
    {
        touchedTime = 0;
        isTouchingCube = false;
        onCubeRelease?.Invoke(0);
    }
    private void Update()
    {
        if (!isGameOn)
        {
            return;
        }
        if(isMobile)
        {
            if (Input.touchCount != 1)//Should be tested next time i play
            {
                touchedTime = 0f;
                return;
            }

            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.TryGetComponent<Cell>(out Cell CellComponent))
                    {
                        isTouchingCube = true;
                        touchedCube = hit.transform.gameObject;
                        if(!setBombs && !CellComponent.HasBeenTouched)
                            onCubeTouch?.Invoke(3);
                    }
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                if(shouldNotClick)
                {
                    shouldNotClick = false;
                    return;
                }
                NullTouchingVariables();
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.TryGetComponent<Cell>(out Cell CellComponent))
                    {
                        if (hit.transform.gameObject == touchedCube)
                        {
                            if (setBombs)
                            {
                                if (CellComponent.HasBeenTouched)
                                    CellComponent.RevealAllSurroundingNonBombs();
                                else
                                    CellComponent.WasRightClicked();
                            }
                            else
                            {
                                if (theFirstCube)
                                {
                                    theFirstCube = false;
                                    ApplyMines.CantBeBomb = hit.transform.gameObject;
                                    onFirstCubeTouch?.Invoke();
                                }
                                PlaySound?.Invoke();
                                if (CellComponent.HasBeenTouched)
                                    CellComponent.RevealAllSurroundingNonBombs();
                                if (!CellComponent.Flag.activeSelf)
                                    CellComponent.WasClicked();
                            }
                        }
                    }
                }
            }
            if(isTouchingCube)
            {
                touchedTime += Time.deltaTime;
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform == null)
                    {
                        NullTouchingVariables();
                    }
                    if(touchedCube != hit.transform.gameObject)
                    {
                        NullTouchingVariables();
                    }
                    if(touchedTime>=constTouchedTime)
                    {
                        shouldNotClick = true;
                        hit.transform.GetComponent<Cell>().WasRightClicked();
                        NullTouchingVariables();
                    }
                }
            }
        }
        else
        {
            if (Input.GetMouseButtonUp(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.TryGetComponent<Cell>(out Cell CellComponent))
                    {
                        if (hit.transform.gameObject == touchedCube)
                        {
                            if (theFirstCube)
                            {
                                theFirstCube = false;
                                ApplyMines.CantBeBomb = hit.transform.gameObject;
                                onFirstCubeTouch?.Invoke();
                            }
                            PlaySound?.Invoke();
                            if (CellComponent.HasBeenTouched)
                                CellComponent.RevealAllSurroundingNonBombs();
                            if(!CellComponent.Flag.activeSelf)
                                CellComponent.WasClicked();
                        }
                    }
                }
                onCubeRelease?.Invoke(0);
            }
            if (Input.GetMouseButtonDown(0)) // 0 is the left mouse button
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.TryGetComponent<Cell>(out Cell CellComponent))
                    {
                        touchedCube = hit.transform.gameObject;
                        if(!CellComponent.HasBeenTouched)
                            onCubeTouch?.Invoke(3);
                    }
                }
            }
            else if (Input.GetMouseButtonDown(1)) // 1 is the right mouse button
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.TryGetComponent<Cell>(out Cell CellComponent))
                    {
                        CellComponent.WasRightClicked();
                    }
                }
            }
        }
    }
}
