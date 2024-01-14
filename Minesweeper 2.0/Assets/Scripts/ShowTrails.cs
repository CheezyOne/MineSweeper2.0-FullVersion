using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowTrails : MonoBehaviour
{
    private GameObject PointedCube=null;
    void Update()
    {
        if (!ClickRegister.isGameOn)
        {
            if(PointedCube != null)
            {
                PointedCube.GetComponent<Cell>().StopTheTrails();
            }
            return;
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.name != "Cell(Clone)" && PointedCube != null)
            {
                PointedCube.GetComponent<Cell>().StopTheTrails();
                PointedCube = null;
            }
            if (hit.transform.gameObject != PointedCube && PointedCube!=null)
            {
                PointedCube.GetComponent<Cell>().StopTheTrails();
                PointedCube = null;
            }
            if (hit.transform.name == "Cell(Clone)" && hit.transform.gameObject!=PointedCube)
            {
                if(PointedCube!=null)
                    PointedCube.GetComponent<Cell>().StopTheTrails();
                PointedCube =hit.transform.gameObject;
                hit.transform.GetComponent<Cell>().ShowTrails();
            }
        }
    }
}
