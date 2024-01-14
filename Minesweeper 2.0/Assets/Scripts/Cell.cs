using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public static Action onEmptyCellClicked, onGameLose, PlaySound;
    public static Action<int> onGameLoseSmiley;
    public bool HasBlueBomb = false, HasRedBomb = false, HasBeenTouched=false;
    private bool HasBarierUp = false, HasBarierDown = false, HasBarierRight = false, HasBarierLeft = false;
    public GameObject NumberAboutBombs;
    public int NumberOfRedBombsAround = 0, NumberOfBlueBombsAround=0;
    private Collider[] hitColliders;
    [SerializeField] private Material[] AllMaterials;
    [SerializeField] private GameObject ExplosionEffect, TrailingSphere;
    private bool IsShowingRedBombs=true;
    private void Awake()
    {
        TwoTypesOfBombsChanger.onBombsChange += CheckIfBombsShouldChange;
        BombsInGameChanger.onBombsMove += GetSurroundingCubesInfo;
        ApplyMines.onAllBombsApply += GetSurroundingCubesInfo;
        onGameLose += ShowIfTheBombTextureIsRight;
        onGameLose += SpawnExplosion;
    }
    private void OnDisable()
    {
        BombsInGameChanger.onBombsMove -= GetSurroundingCubesInfo;
        ApplyMines.onAllBombsApply -= GetSurroundingCubesInfo;
        onGameLose -= ShowIfTheBombTextureIsRight;
        onGameLose -= SpawnExplosion;
    }
    private void SpawnExplosion()
    {
        if (HasRedBomb||HasBlueBomb)
        {
            if (GetComponent<MeshRenderer>().material.name != "Bomb (Instance)")
            {
                GetComponent<MeshRenderer>().material = AllMaterials[2];
            }
            GameObject Explosion = Instantiate(ExplosionEffect, new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), Quaternion.identity);
            Destroy(Explosion, 2f);
        }
    }
    private void ShowIfTheBombTextureIsRight()
    {
        if(GetComponent<MeshRenderer>().material.name == "Bomb (Instance)")
        {
            if (!HasRedBomb&&!HasBlueBomb)
                GetComponent<MeshRenderer>().material.color = Color.red;
            else
                GetComponent<MeshRenderer>().material.color = Color.green;
        }
    }
    public List<GameObject> GetAllNeighbours()
    {
        List<GameObject> Neighbours = new List<GameObject>();
        hitColliders = Physics.OverlapSphere(transform.position, 1f);
        for (int i = 0;i<hitColliders.Length;i++)
        {
            if (hitColliders[i] != gameObject && hitColliders[i].transform.name=="Cell(Clone)")
                Neighbours.Add(hitColliders[i].gameObject);
        }
        return Neighbours;
    }
    public GameObject GetRandomNeighbour()
    {
        hitColliders = Physics.OverlapSphere(transform.position, 1f);
        int RN = UnityEngine.Random.Range(0, hitColliders.Length);
        GameObject RandomNeighbour = hitColliders[RN].gameObject;
        if (RandomNeighbour.transform.name != "Cell(Clone)"|| RandomNeighbour == gameObject)
            return null;
        return RandomNeighbour;
    }
    private void DecideBarierPosition(Collider Barier)
    {
        float BarierX = Barier.transform.position.x, BarierZ= Barier.transform.position.z;
        float TransformX = transform.position.x, TransformZ = transform.position.z;
        if (BarierX > TransformX && Math.Abs(BarierZ - TransformZ) < 0.1f)
            HasBarierRight = true;
        else if (BarierX < TransformX && Math.Abs(BarierZ - TransformZ) < 0.1f)
            HasBarierLeft = true;
        else if (BarierZ > TransformZ && Math.Abs(BarierX - TransformX)<0.1f)
            HasBarierUp = true;
        else if (BarierZ < TransformZ && Math.Abs(BarierX - TransformX) < 0.1f)
            HasBarierDown = true;
    }
    private bool IsBehindABarier(Collider Object)
    {
        float ObjectX = Object.transform.position.x, ObjectZ = Object.transform.position.z;
        float TransformX = transform.position.x, TransformZ = transform.position.z;
        if (ObjectX - TransformX > 0.1f && HasBarierRight)
        {
            return true;
        }
        if (TransformX - ObjectX > 0.1f && HasBarierLeft)
        {
            return true;
        }
        if (ObjectZ - TransformZ >0.1f && HasBarierUp)
        {
            return true;
        }
        if (TransformZ - ObjectZ >0.1f && HasBarierDown)
        {
            return true;
        }
        return false;
    }
    private bool IsBehindABarier(GameObject Object)
    {
        float ObjectX = Object.transform.position.x, ObjectZ = Object.transform.position.z;
        float TransformX = transform.position.x, TransformZ = transform.position.z;
        if (ObjectX - TransformX > 0.1f && HasBarierRight)
        {
            return true;
        }
        if (TransformX - ObjectX > 0.1f && HasBarierLeft)
        {
            return true;
        }
        if (ObjectZ - TransformZ > 0.1f && HasBarierUp)
        {
            return true;
        }
        if (TransformZ - ObjectZ > 0.1f && HasBarierDown)
        {
            return true;
        }
        return false;
    }
    public void GetSurroundingCubesInfo()
    {
        hitColliders = Physics.OverlapSphere(transform.position, 1f);
        NumberOfRedBombsAround = 0;
        NumberOfBlueBombsAround = 0;
        foreach (Collider collider in hitColliders)
        {
            if (collider.gameObject == gameObject)
                continue;
            if(collider.transform.name=="Barier(Clone)")
            {
                DecideBarierPosition(collider);
            }
        }
        foreach (Collider collider in hitColliders)
        {
            if (collider.gameObject == gameObject)
                continue;
            Cell CellComponent = collider.GetComponent<Cell>();
            if (CellComponent != null)
            {
                if (CellComponent.HasRedBomb)
                {
                    if (IsBehindABarier(collider))
                        continue;
                    NumberOfRedBombsAround++;
                }
                if(CellComponent.HasBlueBomb)
                {
                    if (IsBehindABarier(collider))
                        continue;
                    NumberOfBlueBombsAround++;
                }
            }
        }
        NumberAboutBombs.GetComponent<TextMeshPro>().text = Convert.ToString(NumberOfRedBombsAround);
    }
    public void RevealAllSurroundingNonBombs()
    {
        if (HasBlueBomb || HasRedBomb)
            return;
        int BombsCounter=0;
        List<Collider> NonBombs = new List<Collider>();
        Collider[] SurroundingColliders = Physics.OverlapSphere(transform.position, 1f);
        foreach (Collider collider in SurroundingColliders)
        {
            if (collider.gameObject == gameObject || collider.transform.name != "Cell(Clone)")
                continue;

            if (collider.GetComponent<MeshRenderer>().material.name == "Bomb (Instance)")
                BombsCounter++;
            else
            {
                NonBombs.Add(collider);
            }

        }
        if (BombsCounter == NumberOfBlueBombsAround + NumberOfRedBombsAround)
        {
            foreach (Collider collider in NonBombs)
            {
                collider.GetComponent<Cell>().WasClicked();
            }
        }

    }
    public void WasClicked()
    {
        HasBeenTouched = true;
        if (HasRedBomb||HasBlueBomb)
        {
            PlaySound?.Invoke();
            onGameLose?.Invoke();
            onGameLoseSmiley?.Invoke(7);
            //ClickRegister.isGameOn = false;
            return;
        }
        else if (NumberOfRedBombsAround > 0|| NumberOfBlueBombsAround>0)
        {

            NumberAboutBombs.SetActive(true);
            if (NumberOfRedBombsAround > 0 && NumberOfBlueBombsAround > 0)
            {
                NumberAboutBombs.GetComponent<TextMeshPro>().color = Color.red;
            }
            else if (NumberOfRedBombsAround > 0)
                NumberAboutBombs.GetComponent<TextMeshPro>().text = Convert.ToString(NumberOfRedBombsAround);
            else if (NumberOfBlueBombsAround > 0)
            {
                NumberAboutBombs.GetComponent<TextMeshPro>().color = Color.red;
                NumberAboutBombs.GetComponent<TextMeshPro>().text = Convert.ToString(NumberOfBlueBombsAround);
            }
        }
        else
        {
            foreach (Collider collider in hitColliders)
            {
                if (collider.gameObject == gameObject)
                    continue;
                Cell CellComponent = collider.GetComponent<Cell>();
                if (CellComponent != null)
                {
                    if (CellComponent.HasBeenTouched)
                        continue;
                    if (IsBehindABarier(collider))
                        continue;
                    CellComponent.WasClicked();
                }
            }
        }
        if(GetComponent<MeshRenderer>().material.name == "Normal cube (Instance)" || GetComponent<MeshRenderer>().material.name == "Pointed cube (Instance)")
        {
            onEmptyCellClicked?.Invoke();
        }
            GetComponent<MeshRenderer>().material = AllMaterials[1];
    }
    public void WasRightClicked()
    {
        string CurrentMaterial = GetComponent<MeshRenderer>().material.name ;
        switch (CurrentMaterial)
        {
            case "Normal cube (Instance)":
                {
                    GetComponent<MeshRenderer>().material = AllMaterials[2];
                    InGameBombsCounter.PlayersBombsCount++;
                    break;
                }
            case "Pointed cube (Instance)":
                {
                    GetComponent<MeshRenderer>().material = AllMaterials[2];
                    InGameBombsCounter.PlayersBombsCount++;
                    break;
                }
            case "Bomb (Instance)":
                {
                    GetComponent<MeshRenderer>().material = AllMaterials[0];
                    InGameBombsCounter.PlayersBombsCount--;
                    break; 
                }
            default:
                {
                    break;
                }
        }
    }
    private void CheckIfBombsShouldChange()
    {
        if (NumberOfBlueBombsAround>0 && NumberOfRedBombsAround>0)
            ChangeBombNumbers();
    }    
    private void ChangeBombNumbers()
    {
        if (this == null)
            return;
        if (IsShowingRedBombs)
        {
            NumberAboutBombs.GetComponent<TextMeshPro>().text = Convert.ToString(NumberOfBlueBombsAround);
            NumberAboutBombs.GetComponent<TextMeshPro>().color = Color.red;
            IsShowingRedBombs = false;
        }
        else
        {
            IsShowingRedBombs = true;
            NumberAboutBombs.GetComponent<TextMeshPro>().text = Convert.ToString(NumberOfRedBombsAround);
            NumberAboutBombs.GetComponent<TextMeshPro>().color = Color.blue;
        }

    }

    public void ShowTrails()
    {
        StartCoroutine(SpawnTrailsToNearbyCell());
    }
    public void StopTheTrails()
    {
        StopAllCoroutines();
    }
    private IEnumerator SpawnTrailsToNearbyCell()
    {
        Vector3 CameraAdjast = (Camera.main.transform.position- transform.position);
        CameraAdjast.Normalize();
        CameraAdjast /= 1.3f;
        yield return new WaitForSeconds(0.3f);
        for(int i= 0;i < GetAllNeighbours().Count;i++)
        {
            if (IsBehindABarier(GetAllNeighbours()[i]))
            {
                GameObject Sphere = Instantiate(TrailingSphere, transform.position + CameraAdjast, Quaternion.identity);
                Sphere.GetComponent<Rigidbody>().AddForce((GetAllNeighbours()[i].transform.position - transform.position) * 1.3f, ForceMode.Impulse);
                Destroy(Sphere, 0.8f);
            }
        }
        yield return SpawnTrailsToNearbyCell();
    }
}
