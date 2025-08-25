using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class StartingSequence : MonoBehaviour
{
    [SerializeField] private GameObject Field;
    private List<GameObject> AllCubes = new List<GameObject>(), FallingCubes = new List<GameObject>();
    private Dictionary<GameObject, Vector3> CubeTargetPositions = new Dictionary<GameObject, Vector3>();
    private bool CubesShouldFall = false;
    private float FallSpeed = 15f;
    private int HowManyCubesFallSimultaneously = 1;
    private Coroutine _waitForBariersToDetect;

    public static Action getBariersToWork;
    public static Action<int> onCubesFallSmiley;

    private void Awake()
    {
        FieldGeneration.onFieldGenerated += NullAllVariables;
        FieldGeneration.onFieldGenerated += ApplyCubes;
        FieldGeneration.onFieldGenerated += CubesShouldFallTrue;
        FieldGeneration.onFieldGenerated += StartFallingCoroutine;
        InGameButtons.onGameExit += CubesShouldFallFalse;
        InGameButtons.onGameExit += StopFallingCoroutine;
        InGameButtons.onGameExit += NullAllVariables;
    }

    private void OnDestroy()
    {
        FieldGeneration.onFieldGenerated -= NullAllVariables;
        FieldGeneration.onFieldGenerated -= ApplyCubes;
        FieldGeneration.onFieldGenerated -= CubesShouldFallTrue;
        FieldGeneration.onFieldGenerated -= StartFallingCoroutine;
        InGameButtons.onGameExit -= CubesShouldFallFalse;
        InGameButtons.onGameExit -= StopFallingCoroutine;
        InGameButtons.onGameExit -= NullAllVariables;
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
        HowManyCubesFallSimultaneously = AllCubes.Count / 10;
        if (HowManyCubesFallSimultaneously < 1)
            HowManyCubesFallSimultaneously = 1;
    }

    private Vector3 CalculateEndPosition(GameObject cube)
    {
        if (cube == null) return Vector3.zero;

        return new Vector3(
            cube.transform.position.x,
            cube.transform.position.y - 11f,
            cube.transform.position.z
        );
    }

    private GameObject PickRandomCube()
    {
        if (AllCubes.Count == 0)
            return null;

        int randomNumber = UnityEngine.Random.Range(0, AllCubes.Count);
        GameObject randomCube = AllCubes[randomNumber];
        AllCubes.RemoveAt(randomNumber);

        return randomCube;
    }

    private IEnumerator AddNewCubesFalling()
    {
        while (AllCubes.Count > 0)
        {
            yield return new WaitForSeconds(0.075f);

            for (int i = 0; i < HowManyCubesFallSimultaneously; i++)
            {
                if (AllCubes.Count == 0) break;

                GameObject randomCube = PickRandomCube();
                if (randomCube == null) continue;

                Vector3 endPosition = CalculateEndPosition(randomCube);
                CubeTargetPositions[randomCube] = endPosition;

                if (!FallingCubes.Contains(randomCube))
                {
                    FallingCubes.Add(randomCube);
                }
            }
        }

        yield break;
    }

    private void FallCubes()
    {
        for (int i = FallingCubes.Count - 1; i >= 0; i--)
        {
            GameObject cube = FallingCubes[i];
            if (cube == null || !CubeTargetPositions.ContainsKey(cube))
            {
                FallingCubes.RemoveAt(i);
                continue;
            }

            Vector3 targetPosition = CubeTargetPositions[cube];
            cube.transform.position = Vector3.MoveTowards(cube.transform.position, targetPosition, Time.deltaTime * FallSpeed);

            // ѕровер€ем, достиг ли куб целевой позиции
            if (cube.transform.position == targetPosition)
            {
                CubeTargetPositions.Remove(cube);
                FallingCubes.RemoveAt(i);
            }
        }
    }

    private void NullAllVariables()
    {
        AllCubes = new List<GameObject>();
        FallingCubes = new List<GameObject>();
        CubeTargetPositions = new Dictionary<GameObject, Vector3>();
    }

    private void Update()
    {
        if (CubesShouldFall)
        {
            FallCubes();

            if (FallingCubes.Count == 0 && AllCubes.Count == 0)
            {
                if (_waitForBariersToDetect == null)
                    _waitForBariersToDetect = StartCoroutine(WaitForBariersToDetect());
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
        EventBus.OnAllCubesFall?.Invoke();
        _waitForBariersToDetect = null;
        StartCoroutine(SmileyCor());
        NullAllVariables();
        yield break;
    }

    private IEnumerator SmileyCor()
    {
        yield return new WaitForSeconds(1f);
        onCubesFallSmiley?.Invoke(0);
        yield break;
    }
}