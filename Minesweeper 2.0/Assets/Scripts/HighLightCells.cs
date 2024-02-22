using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighLightCells : MonoBehaviour
{
    [SerializeField] private Material PointedMaterial, NormalMaterial;
    private GameObject PointedCube;
    public static bool IsMobile;
    private void Update()
    {
        if (!ClickRegister.isGameOn)
        {
            if(PointedCube!=null)
            {
                if (PointedCube.GetComponent<MeshRenderer>().material.name == "Pointed cube (Instance)")
                    PointedCube.GetComponent<MeshRenderer>().material = NormalMaterial;
                PointedCube = null;
            }
            return;
        }
        Ray ray;
        if (IsMobile && Input.touchCount>0)

            ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
        else
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.name=="Cell(Clone)")
            {
                HighLightCube(hit);
            }
        }
    }
    private void HighLightCube(RaycastHit hit)
    {

        if (hit.transform.GetComponent<MeshRenderer>().material.name == "Normal cube (Instance)")
        {
            if (hit.transform.gameObject != PointedCube)
            {
                if (PointedCube != null)
                {
                    if (PointedCube.GetComponent<MeshRenderer>().material.name == "Pointed cube (Instance)")
                        PointedCube.GetComponent<MeshRenderer>().material = NormalMaterial;
                    PointedCube = hit.transform.gameObject;
                }
            }
            PointedCube = hit.transform.gameObject;
            hit.transform.GetComponent<MeshRenderer>().material = PointedMaterial;
        }
    }
}
