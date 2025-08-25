using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static List<ObjectPool> ObjectPools = new();
    private static Transform _poolObjectsHolder;
    private void Awake()
    {
        _poolObjectsHolder = transform;
    }
    private void OnDisable()
    {
        foreach(ObjectPool ObjectPool in ObjectPools)
        {
            ObjectPool.InactiveObjects.Clear();
        }
    }
    public static GameObject SpawnObject(GameObject objectToSpawn, Vector3 spawnPosition, Quaternion spawnRotation, Transform parent = null)
    {
        ObjectPool pool = null;
        foreach (ObjectPool objectPool in ObjectPools)
        {
            if (objectPool.PoolName == objectToSpawn.name)
            {
                pool = objectPool;
                break;
            }
        }

        if (pool == null)
        {
            pool = new ObjectPool() { PoolName = objectToSpawn.name };
            ObjectPools.Add(pool);
        }


        GameObject spawnableObject = pool.InactiveObjects.FirstOrDefault();

        if (spawnableObject == null)
        {
            if(parent == null)
                spawnableObject = Instantiate(objectToSpawn, spawnPosition, spawnRotation, _poolObjectsHolder);
            else
                spawnableObject = Instantiate(objectToSpawn, spawnPosition, spawnRotation, parent);
        }
        else
        {
            spawnableObject.transform.SetPositionAndRotation(spawnPosition, spawnRotation);
            pool.InactiveObjects.Remove(spawnableObject);
            spawnableObject.SetActive(true);
        }
        return spawnableObject;
    }
    public static void ReturnObjectToPool(GameObject objectToReturn)
    {
        string realObjectName = objectToReturn.name.Substring(0, objectToReturn.name.Length - 7);
        ObjectPool pool = null;
        foreach (ObjectPool objectPool in ObjectPools)
        {
            if (objectPool.PoolName == realObjectName)
            {
                pool = objectPool;
                break;
            }
        }
        if (pool == null)
            Debug.LogWarning("There's no such pool");
        else
        {
            objectToReturn.SetActive(false);
            pool.InactiveObjects.Add(objectToReturn);
        }
    }
}
public class ObjectPool
{
    public string PoolName;
    public List<GameObject> InactiveObjects = new();
}