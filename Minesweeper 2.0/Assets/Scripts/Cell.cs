using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class Cell : MonoBehaviour
{
    public static Action onEmptyCellClicked;
    public static Action<int> onGameLoseSmiley;
    public static bool ShouldLie = false;
    public bool HasBlueBomb = false, HasRedBomb = false, HasBeenTouched=false;
    private bool HasBarierUp = false, HasBarierDown = false, HasBarierRight = false, HasBarierLeft = false;
    public GameObject Flag, DoubleNumberAboutBombs, Bomb;
    [SerializeField] private TMP_Text NumberAboutBombs;
    [SerializeField] private TMP_Text _firstDoubleNumber;
    [SerializeField] private TMP_Text _secondDoubleNumber;
    [SerializeField] private ContinueWindow _continueWindow;
    public int NumberOfRedBombsAround = 0, NumberOfBlueBombsAround = 0;
    private Collider[] hitColliders;
    [SerializeField] private Material[] AllMaterials;
    [SerializeField] private GameObject ExplosionEffect, TrailingSphere;
    private List<GameObject> ExcludeCubes = new ();
    private bool HasLiedOnceRed = false, IsToLieHighRed = false, HasLiedOnceBlue = false, IsToLieHighBlue = false;
    private GameObject OneColoredString, TwoColoredString;
    public bool Test;
    public static GameObject MainOpeningCube;
    private bool OnceWasMainOpeningCube=false;
    [SerializeField] private MeshRenderer _meshRenderer; 
    public static bool CubesAreOpened;
    private Transform _mainCamera;
    [SerializeField] private float _surroundingCellsOpenTime = 0.1f;
    private WaitForSeconds _surroundingCellsWait = new(0.1f);
    private Coroutine _trailRoutine;
    private bool CanHaveTwoBombs => ApplyMines.ApplyBlueBombs;

    private static bool _hasLostOnce;

    public MeshRenderer MeshRenderer => _meshRenderer;


    private void Awake()
    {
        _mainCamera = Camera.main.transform;
        _hasLostOnce = false;
    }

    private void OnEnable()
    {
        BombsInGameChanger.onBombsMove += GetSurroundingCubesInfo;
        StartingSequence.getBariersToWork += FindBariers;//This event should be called ASAP and then we give the player ability to click
        ApplyMines.onAllBombsApply += GetSurroundingCubesInfo;
        EventBus.OnGameLose += ShowIfTheBombTextureIsRight;
        EventBus.OnGameLose += SpawnExplosion;
        OneColoredString = transform.GetChild(2).GetChild(0).gameObject;
        TwoColoredString = transform.GetChild(2).GetChild(1).gameObject;
    }
    private void OnDisable()
    {
        BombsInGameChanger.onBombsMove -= GetSurroundingCubesInfo;
        StartingSequence.getBariersToWork -= FindBariers;
        ApplyMines.onAllBombsApply -= GetSurroundingCubesInfo;
        EventBus.OnGameLose -= ShowIfTheBombTextureIsRight;
        EventBus.OnGameLose -= SpawnExplosion;
    }

    public void ApplyMaterial(Material material)
    {
        _meshRenderer.material = material;
    }

    private void SpawnExplosion()
    {
        if (HasRedBomb||HasBlueBomb)
        {
            Bomb.SetActive(true);
            _meshRenderer.enabled = false;
            NumberAboutBombs.gameObject.SetActive(false);
            GameObject Explosion = Instantiate(ExplosionEffect, new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), Quaternion.identity);
            Destroy(Explosion, 2f);
        }
    }
    private void ShowIfTheBombTextureIsRight()
    {
        if(Flag.activeSelf)
        {
            Bomb.SetActive(true);
            _meshRenderer.enabled = false;
            NumberAboutBombs.gameObject.SetActive(false);
            if (!HasRedBomb && !HasBlueBomb)
            {
                Bomb.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = new Color(0.47f, 0, 0);
                Bomb.transform.GetChild(1).GetComponent<MeshRenderer>().material.color = new Color(0.47f, 0, 0);
            }
            else
            {
                Bomb.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = new Color(0, 0.47f, 0);
                Bomb.transform.GetChild(1).GetComponent<MeshRenderer>().material.color = new Color(0, 0.47f, 0);
            }
        }
        Flag.SetActive(false);
    }
    public List<GameObject> GetAllNeighbours()
    {
        List<GameObject> Neighbours = new();
        Physics.OverlapSphereNonAlloc(transform.position, 1f, hitColliders);
        //hitColliders = Physics.OverlapSphere(transform.position, 1f);
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
        Cell ObjectCell = Object.GetComponent<Cell>();
        if (ObjectX - TransformX > 0.1f && (HasBarierRight || ObjectCell.HasBarierLeft))
        {
            return true;
        }
        if (TransformX - ObjectX > 0.1f && (HasBarierLeft || ObjectCell.HasBarierRight))
        {
            return true;
        }
        if (ObjectZ - TransformZ >0.1f && (HasBarierUp || ObjectCell.HasBarierDown))
        {
            return true;
        }
        if (TransformZ - ObjectZ >0.1f && (HasBarierDown || ObjectCell.HasBarierUp))
        {
            return true;
        }
        return false;
    }
    private bool IsBehindABarier(GameObject Object)
    {
        float ObjectX = Object.transform.position.x, ObjectZ = Object.transform.position.z;
        float TransformX = transform.position.x, TransformZ = transform.position.z;
        Cell ObjectCell = Object.GetComponent<Cell>();
        if (ObjectX - TransformX > 0.1f && (HasBarierRight || ObjectCell.HasBarierLeft))
        {
            return true;
        }
        if (TransformX - ObjectX > 0.1f && (HasBarierLeft || ObjectCell.HasBarierRight))
        {
            return true;
        }
        if (ObjectZ - TransformZ > 0.1f && (HasBarierUp || ObjectCell.HasBarierDown))
        {
            return true;
        }
        if (TransformZ - ObjectZ > 0.1f && (HasBarierDown || ObjectCell.HasBarierUp))
        {
            return true;
        }
        return false;
    }
    private void FindBariers()
    {
        hitColliders = Physics.OverlapSphere(transform.position, 1f);
        
        foreach (Collider collider in hitColliders)
        {
            if (collider.gameObject == gameObject)
                continue;
            if (collider.transform.name == "Barier(Clone)")
            {
                DecideBarierPosition(collider);
            }
        }
    }
    private void FunExcludeCubes()
    {
        foreach (Collider collider in hitColliders)
        {
            if (collider.gameObject == gameObject)
                continue;
            Cell CellComponent = collider.GetComponent<Cell>();
            if (CellComponent != null)
            {
                if (IsBehindABarier(collider))
                {
                    ExcludeCube(collider.gameObject);
                    CellComponent.ExcludeCube(gameObject);
                }
            }
        }
    }
    public void ExcludeCube(GameObject Cube)
    {
        if (!ExcludeCubes.Contains(Cube))
            ExcludeCubes.Add(Cube);
    }
    public void GetSurroundingCubesInfo()
    {
        Physics.OverlapSphereNonAlloc(transform.position, 1f, hitColliders);
        //FindBariers();
        ExcludeCubes.Clear();
        FunExcludeCubes();
        NumberOfRedBombsAround = 0;
        NumberOfBlueBombsAround = 0;
        foreach (Collider collider in hitColliders)
        {
            if (collider.gameObject == gameObject)
                continue;
            Cell CellComponent = collider.GetComponent<Cell>();
            if (CellComponent != null)
            {
                if (CellComponent.HasRedBomb)
                {
                    if (ExcludeCubes.Contains(collider.gameObject))
                        continue;
                    NumberOfRedBombsAround++;
                }
                if(CellComponent.HasBlueBomb)
                {
                    if (ExcludeCubes.Contains(collider.gameObject))
                        continue;
                    NumberOfBlueBombsAround++;
                }
            }
        }
        if(HasBeenTouched)
            SetupNumbers();
    }
    private int LyingNumber(int RealNumber, bool RedNumber)
    {
        if (!ShouldLie)
            return RealNumber;
        if (RealNumber == 0)
            return 1;
        if (RedNumber)
        {
            if (HasLiedOnceRed)
            {
                if (IsToLieHighRed)
                    return RealNumber + 1;
                return RealNumber - 1;
            }
            if (UnityEngine.Random.Range(0, 100) > 50)
            {
                IsToLieHighRed = true;
                HasLiedOnceRed = true;
                return RealNumber + 1;
            }
            HasLiedOnceRed = true;
            return RealNumber - 1;
        }
        else
        {
            if (HasLiedOnceBlue)
            {
                if (IsToLieHighBlue)
                    return RealNumber + 1;
                return RealNumber - 1;
            }
            if (UnityEngine.Random.Range(0, 100) > 50)
            {
                IsToLieHighBlue = true;
                HasLiedOnceBlue = true;
                return RealNumber + 1;
            }
            HasLiedOnceBlue = true;
            return RealNumber - 1;
        }
    }
    private void SetupNumbers()
    {
        if (NumberOfRedBombsAround <= 0 && NumberOfBlueBombsAround <= 0)
        {
            if (NumberAboutBombs.gameObject.activeSelf || DoubleNumberAboutBombs.activeSelf)
            {
                NumberAboutBombs.color = Color.blue;//I dont like red numbers, so red bombs are blue
                NumberAboutBombs.text = Convert.ToString(LyingNumber(NumberOfRedBombsAround, true));
                NumberAboutBombs.gameObject.SetActive(true);
                DoubleNumberAboutBombs.SetActive(false);
            }
            return;
        }
        if (NumberOfRedBombsAround > 0 && NumberOfBlueBombsAround > 0)
        {
            NumberAboutBombs.gameObject.gameObject.SetActive(false);
            DoubleNumberAboutBombs.SetActive(true);
            _secondDoubleNumber.text = Convert.ToString(LyingNumber(NumberOfBlueBombsAround, false));
            _firstDoubleNumber.text = Convert.ToString(LyingNumber(NumberOfRedBombsAround, true));
        }
        else
        {
            NumberAboutBombs.gameObject.SetActive(true);
            DoubleNumberAboutBombs.SetActive(false);
            if (NumberOfRedBombsAround > 0)
            {
                NumberAboutBombs.color = Color.blue;//I dont like red numbers, so red bombs are blue
                NumberAboutBombs.text = Convert.ToString(LyingNumber(NumberOfRedBombsAround, true));
            }
            else if (NumberOfBlueBombsAround > 0)
            {
                NumberAboutBombs.color = Color.red;
                NumberAboutBombs.text = Convert.ToString(LyingNumber(NumberOfBlueBombsAround, false));
            }
        }
    }
    public void RevealAllSurroundingNonBombs()
    {
        if (HasBlueBomb || HasRedBomb)
            return;

        int BombsCounter=0;
        List<Collider> NonBombs = new();
        Collider[] SurroundingColliders = Physics.OverlapSphere(transform.position, 1f);

        foreach (Collider collider in SurroundingColliders)
        {
            if (collider.gameObject == gameObject || collider.transform.name != "Cell(Clone)"|| IsBehindABarier(collider))
                continue;

            GameObject Flag = collider.transform.GetChild(2).gameObject;
            if (Flag.activeSelf)
            {
                if (CanHaveTwoBombs)
                {
                    if (Flag.transform.GetChild(0).gameObject.activeSelf)
                        BombsCounter++;
                    else if (Flag.transform.GetChild(1).gameObject.activeSelf)
                        BombsCounter += 2;
                }
                else
                {
                    BombsCounter++;
                }
            }
            else
            {
                NonBombs.Add(collider);
            }
        }

        if (BombsCounter == NumberOfBlueBombsAround + NumberOfRedBombsAround)
        {
            if(ShouldLie)
            {
                return;
            }

            foreach (Collider collider in NonBombs)
            {
                if (!IsBehindABarier(collider))
                    collider.GetComponent<Cell>().WasClicked(0f);
            }
        }

    }
    private IEnumerator ClickWait(float ClickDelay)
    {
        if (ClickDelay > 0f)
            yield return new WaitForSeconds(ClickDelay);
        if (MainOpeningCube == gameObject)
            CubesAreOpened = true;
        Click();
        yield return _surroundingCellsWait;
        RevealAllSurroundingNonBombs();
    }
    private void Click()
    {
        if (HasBeenTouched)
            return;

        if (Flag.activeSelf)
        {
            InGameBombsCounter.PlayersBombsCount--;
            Flag.SetActive(false);
        }
        
        if (HasRedBomb || HasBlueBomb)
        {
            if (!_hasLostOnce && !ContinueWindow.DontShowAgain)
            {
                WindowsManager.Instance.OpenWindow(_continueWindow);
                _hasLostOnce = true;
            }
            else
            {
                EventBus.OnGameLose?.Invoke();
                onGameLoseSmiley?.Invoke(7);
            }

            return;
        }

        HasBeenTouched = true;
        Sequence MoveToOpen = DOTween.Sequence();
        MoveToOpen.Append(transform.DOMoveY(transform.position.y + 0.3f, 0.2f));
        MoveToOpen.Append(transform.DOMoveY(transform.position.y, 0.2f));

        if (NumberOfRedBombsAround > 0 || NumberOfBlueBombsAround > 0)
        {
            SetupNumbers();
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
                    if (IsBehindABarier(collider))
                        continue;
                    CellComponent.WasClicked(_surroundingCellsOpenTime); 
                }
            }
        }
        if (_meshRenderer.material.name == "Normal cube (Instance)" || _meshRenderer.material.name == "Pointed cube (Instance)")
        {
            onEmptyCellClicked?.Invoke();
        }
        _meshRenderer.material = AllMaterials[1];
    }    
    public void WasClicked(float Delay)
    {
        if (HasBeenTouched|| !ClickRegister.isGameOn)
            return;
        
        if (!OnceWasMainOpeningCube)
        {
            OnceWasMainOpeningCube = true;
            CubesAreOpened = false;
            MainOpeningCube = gameObject;
        }

        StartCoroutine(ClickWait(Delay));
    }
    public void WasRightClicked()
    {
        if (_meshRenderer.material.name == "Touched cube (Instance)")
            return;
        if (CanHaveTwoBombs)
        {
            OneColoredString.SetActive(true);
            SpriteRenderer StringMesh = OneColoredString.GetComponent<SpriteRenderer>();
            if (!Flag.activeSelf)
            {
                InGameBombsCounter.PlayersBombsCount++;
                Flag.SetActive(true);
                StringMesh.material.color = Color.blue;
            }
            else if (StringMesh.material.color == Color.blue)
                StringMesh.material.color = Color.red;
            else if (!TwoColoredString.activeSelf)
            {
                OneColoredString.SetActive(false);
                TwoColoredString.SetActive(true);
            }
            else
            {
                TwoColoredString.SetActive(false);
                Flag.SetActive(false);
                InGameBombsCounter.PlayersBombsCount--;
            }
        }
        else
        {
            if (Flag.activeSelf)
            {
                Flag.SetActive(false);
                InGameBombsCounter.PlayersBombsCount--;
            }
            else
            {
                Flag.SetActive(true);
                InGameBombsCounter.PlayersBombsCount++;
            }
        }
    }
    public void ShowTrails()
    {
        _trailRoutine = StartCoroutine(SpawnTrailsToNearbyCell());
    }
    public void StopTheTrails()
    {
        StopCoroutine(_trailRoutine);
    }
    private IEnumerator SpawnTrailsToNearbyCell()
    {
        Vector3 CameraAdjast = (_mainCamera.position- transform.position);
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
