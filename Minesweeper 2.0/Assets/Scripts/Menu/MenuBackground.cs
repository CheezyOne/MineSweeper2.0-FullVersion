using System.Collections;
using UnityEngine;

public class MenuBackground : MonoBehaviour
{
    [SerializeField] GameObject[] Cubes;
    public static bool ShouldSpawnCubes = true;
    private Vector3 MainCameraPosition;
    private WaitForSeconds _waitForNextCubeSpawn = new(0.7f);

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
        while (true)
        {
            yield return _waitForNextCubeSpawn;
            SpawnACube();
        }
    }

    private Vector3 GetNewCubePosition()
    {
        Vector3 RandomPosition = MainCameraPosition + new Vector3(-18,-9f,8);
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
        int RandomNumber = UnityEngine.Random.Range(0, 4);
        Vector3 RandomPosition = GetNewCubePosition();
        MenuCubes NewCube = Instantiate(Cubes[RandomNumber], RandomPosition, Quaternion.identity, transform).GetComponent<MenuCubes>();
        switch (RandomNumber)
        {
            case 0:
                {
                    break;
                }
            case 1:
                {
                    int RandomNumberOnCube = UnityEngine.Random.Range(0, 9);
                    NewCube.SetNumbers(RandomNumberOnCube.ToString());
                    break;
                }
            case 2:
                {
                    int RandomColorNumber = UnityEngine.Random.Range(0, 10);
                    if(RandomColorNumber<3)
                    {
                        foreach(MeshRenderer meshRenderer in NewCube.MeshRenderers)
                        {
                            meshRenderer.material.color = Color.red;
                        }
                    }
                    else if (RandomColorNumber > 7)
                    {
                        foreach (MeshRenderer meshRenderer in NewCube.MeshRenderers)
                        {
                            meshRenderer.material.color = Color.green;
                        }
                    }
                    break;
                }
            case 3:
                {
                    break;
                }
        }

        NewCube.transform.Rotate(UnityEngine.Random.Range(-180, 180), UnityEngine.Random.Range(-180, 180), UnityEngine.Random.Range(-180, 180));
    }
}
