using UnityEngine;

public class HighLightCells : MonoBehaviour
{
    private Cell _pointedCell;
    private Transform _pointedCellTransform;
    public static bool IsMobile;
    private Camera _mainCamera;
    private Material _normalMaterial;
    private Material _pointedMaterial;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        if (!ClickRegister.isGameOn)
        {
            ClearHighlight();
            return;
        }

        HandleRaycast();
    }

    private void HandleRaycast()
    {
        Ray ray = IsMobile && Input.touchCount > 0
            ? _mainCamera.ScreenPointToRay(Input.touches[0].position)
            : _mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit) && hit.transform.CompareTag("Cell"))
        {
            ProcessCellHit(hit);
        }
        else
        {
            ClearHighlight();
        }
    }

    private void ProcessCellHit(RaycastHit hit)
    {
        if (hit.transform == _pointedCellTransform)
            return;

        Cell hitCell = hit.transform.GetComponent<Cell>();

        if(_pointedMaterial == null)
        {
            _normalMaterial = hitCell.AllMaterials[0];
            _pointedMaterial = hitCell.AllMaterials[2];
        }

        if (hitCell == null)
            return;

        if (CanHighlightCell(hitCell))
        {
            HighlightCell(hitCell);
        }
        else
        {
            ClearHighlight();
        }
    }

    private bool CanHighlightCell(Cell cell)
    {
        return cell.MeshRenderer.sharedMaterial == _normalMaterial;
    }

    private void HighlightCell(Cell cell)
    {
        ClearHighlight();
        _pointedCellTransform = cell.transform;
        _pointedCell = cell;
        _pointedCell.ApplyMaterial(_pointedMaterial);
    }

    private void ClearHighlight()
    {
        if (_pointedCell != null)
        {
            if (_pointedCell.MeshRenderer.sharedMaterial == _pointedMaterial)
            {
                _pointedCell.ApplyMaterial(_normalMaterial);
            }
            _pointedCell = null;
        }
    }

    private void OnDisable()
    {
        ClearHighlight();
    }
}