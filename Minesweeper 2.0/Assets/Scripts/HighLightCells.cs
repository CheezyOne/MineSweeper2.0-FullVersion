using UnityEngine;

public class HighLightCells : MonoBehaviour
{
    [SerializeField] private Material PointedMaterial, NormalMaterial;
    private Cell PointedCube;
    public static bool IsMobile;
    private Camera _mainCamera;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        if (!ClickRegister.isGameOn)
        {
            if(PointedCube!=null)
            {
                if (PointedCube.MeshRenderer.material.name == "Pointed cube (Instance)")
                    PointedCube.ApplyMaterial(NormalMaterial);
                PointedCube = null;
            }
            return;
        }
        Ray ray;
        if (IsMobile && Input.touchCount>0)
            ray = _mainCamera.ScreenPointToRay(Input.touches[0].position);
        else
            ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
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
                    if (PointedCube.MeshRenderer.material.name == "Pointed cube (Instance)")
                        PointedCube.ApplyMaterial(NormalMaterial);
                }
            }
            PointedCube = hit.transform.GetComponent<Cell>();
            PointedCube.ApplyMaterial(PointedMaterial);
        }
    }
}
