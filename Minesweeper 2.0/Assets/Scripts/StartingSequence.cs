using System.Collections;
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;

public class StartingSequence : MonoBehaviour
{
    [SerializeField] private GameObject Field;
    private List<GameObject> AllCubes = new List<GameObject>(), FallingCubes = new List<GameObject>();
    private List<Vector3> AllStartingPositions = new List<Vector3>(), AllEndPositions = new List<Vector3>();
    private bool CubesShouldFall = false;
    private float FallSpeed=15f;
    private int HowManyCubesFallSimultaneously = 1;

    public static Action onAllCubesFall, getBariersToWork;
    public static Action<int> onCubesFallSmiley;
    private void Awake()
    {
        FieldGeneration.onFieldGenerated += NullAllVariables;
        FieldGeneration.onFieldGenerated += ApplyCubes;
        FieldGeneration.onFieldGenerated += CubesShouldFallTrue;
        FieldGeneration.onFieldGenerated += StartFallingCoroutine;
        InGameButtons.onGameExit += CubesShouldFallFalse;
        InGameButtons.onGameExit += StopFallingCoroutine;
        InGameButtons.onGameExit+=NullAllVariables;

    }
    private void CubesShouldFallTrue()
    {
        CubesShouldFall = true;
    }
    private void CubesShouldFallFalse()
    {
        CubesShouldFall = false;
    }
    private void StopFallingCoroutine()
    {
        StopAllCoroutines();
    }
    private void StartFallingCoroutine()
    {
        StartCoroutine(AddNewCubesFalling());
    }
    private void ApplyCubes()
    {
        AllCubes = FieldGeneration.AllCubes.Concat(FieldGeneration.AllBariers).ToList();
        HowManyCubesFallSimultaneously = AllCubes.Count / 75;
        if (HowManyCubesFallSimultaneously < 1)
            HowManyCubesFallSimultaneously = 1;
    }
    private void DecideStartAndEndPositions(GameObject Cube)
    {
        if(Cube == null)
        {
            return;
        }
            AllStartingPositions.Add(Cube.transform.position);
            AllEndPositions.Add
                (new Vector3
                (Cube.transform.position.x,
                Cube.transform.position.y-11f,
                Cube.transform.position.z)
                );
    }
    private GameObject PickRandomCube()
    {
        int Randomnumber = UnityEngine.Random.Range(0, AllCubes.Count-1);
        GameObject RandomCube = AllCubes[Randomnumber];
        AllCubes.RemoveAt(Randomnumber);
        return RandomCube;
    }
    private IEnumerator AddNewCubesFalling()
    {
        yield return new WaitForEndOfFrame();
        for (int i = 0; i < HowManyCubesFallSimultaneously; i++)
        {
            GameObject RandomCube = PickRandomCube();
            DecideStartAndEndPositions(RandomCube);
            FallingCubes.Add(RandomCube);
            if (AllCubes.Count == 0)
            {
                StopAllCoroutines();
                yield return null;
            }
        }
        yield return AddNewCubesFalling();
    }
    private void FallCubes()
    {
        for (int i = 0; i < FallingCubes.Count; i++) 
        {
            if (FallingCubes[i] == null)
                continue;
            FallingCubes[i].transform.position = Vector3.MoveTowards(FallingCubes[i].transform.position, AllEndPositions[i], Time.deltaTime* FallSpeed);
        }
    }
    private void NullAllVariables()
    {
        AllCubes = new List<GameObject>();
        FallingCubes = new List<GameObject>();
        AllStartingPositions = new List<Vector3>();
        AllEndPositions = new List<Vector3>();
}    
    void Update()
    {
        if (CubesShouldFall)
        {
            FallCubes();
            if (FallingCubes.Count > 0)
            {
                if (FallingCubes[FallingCubes.Count - 1].transform.position == AllEndPositions[FallingCubes.Count - 1])
                {
                    StartCoroutine(WaitForBariersToDetect());
                }
            } 
        }
    }
    private IEnumerator WaitForBariersToDetect()
    {
        getBariersToWork?.Invoke();
        yield return new WaitForSeconds(0.1f);
        ClickRegister.isGameOn = true;
        CubesShouldFall = false;
        onCubesFallSmiley?.Invoke(2);
        StartCoroutine(SmileyCor());
        NullAllVariables();
        yield break;
    }
    private IEnumerator SmileyCor()
    {
        yield return new WaitForSeconds(1f);
        onCubesFallSmiley?.Invoke(0);
        onAllCubesFall?.Invoke();
        yield break;
    }
}
