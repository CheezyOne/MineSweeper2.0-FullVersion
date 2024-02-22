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
    private void Update()
    {
        if (!isGameOn)
        {
            return;
        }
        if(isMobile)
        {
            if (Input.touchCount == 0)
            {
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
                        touchedCube = hit.transform.gameObject;
                        if(!setBombs)
                            onCubeTouch?.Invoke(3);
                    }
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
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
                                hit.transform.GetComponent<Cell>().WasClicked();
                                hit.transform.GetComponent<Cell>().RevealAllSurroundingNonBombs();
                            }
                        }
                    }
                }
                onCubeRelease?.Invoke(0);

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
                            hit.transform.GetComponent<Cell>().WasClicked();
                            hit.transform.GetComponent<Cell>().RevealAllSurroundingNonBombs();
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
