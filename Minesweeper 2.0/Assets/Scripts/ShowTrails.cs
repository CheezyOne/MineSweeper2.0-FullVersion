using UnityEngine;

public class ShowTrails : MonoBehaviour
{
    private GameObject PointedCube=null;
    private void Update()
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

        if (Physics.Raycast(ray, out RaycastHit hit))
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
