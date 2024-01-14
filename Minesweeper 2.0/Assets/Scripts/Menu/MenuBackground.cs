using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class MenuBackground : MonoBehaviour
{
    [SerializeField] GameObject[] Cubes;
    public static bool ShouldSpawnCubes = true;
    private Vector3 MainCameraPosition;
    private void OnEnable()
    {
        DeleteAllChildren();
        MainCameraPosition = Camera.main.transform.position;
        StartCoroutine(SpawnCubes());
    }
    private void DeleteAllChildren()
    {
        for (int i=0;i<transform.childCount;i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }    
    }
    private IEnumerator SpawnCubes()
    {
        yield return new WaitForSeconds(0.7f);
        SpawnACube();
        yield return SpawnCubes();
    }
    private Vector3 GetNewCubePosition()
    {
        Vector3 RandomPosition = MainCameraPosition + new Vector3(-18,-9f,10);
        float Randomfloat = UnityEngine.Random.Range(0, 2);
        int RandomInt=0;
        if (Randomfloat >= 0.5f)
            RandomInt = 1;
        switch (RandomInt)
        {
            case 0:
                {
                    RandomPosition += new Vector3(UnityEngine.Random.Range(0, 25f), 0, UnityEngine.Random.Range(-4f, 3f));
                    break;
                }
            case 1:
                {
                    RandomPosition += new Vector3(0, UnityEngine.Random.Range(0, 12f), UnityEngine.Random.Range(-4f, 3f));
                    break;
                }
        }

        return RandomPosition;
    }
    private void SpawnACube()
    {
        int RandomNumber = UnityEngine.Random.Range(0, 3);
        //RandomNumber = 0;
        Vector3 RandomPosition = GetNewCubePosition();
        GameObject NewCube = Instantiate(Cubes[RandomNumber], RandomPosition, Quaternion.identity);
        switch (RandomNumber)
        {
            case 0:
                {
                    break;
                }
            case 1:
                {
                    int RandomNumberOnCube = UnityEngine.Random.Range(0, 9);
                    for (int i=0;i<NewCube.transform.childCount;i++)
                    {
                        NewCube.transform.GetChild(i).GetComponent<TextMeshPro>().text = Convert.ToString(RandomNumberOnCube);
                    }
                    break;
                }
            case 2:
                {
                    int RandomColorNumber = UnityEngine.Random.Range(0, 10);
                    if(RandomColorNumber<3)
                    {
                        NewCube.GetComponent<MeshRenderer>().material.color = Color.red;
                    }
                    else if (RandomColorNumber > 7)
                    {
                        NewCube.GetComponent<MeshRenderer>().material.color = Color.green;
                    }
                    break;
                }
            case 3:
                {
                    break;
                }
        }
        NewCube.transform.Rotate(UnityEngine.Random.Range(-180, 180), UnityEngine.Random.Range(-180, 180), UnityEngine.Random.Range(-180, 180));
        NewCube.transform.SetParent(gameObject.transform);

    }
}
