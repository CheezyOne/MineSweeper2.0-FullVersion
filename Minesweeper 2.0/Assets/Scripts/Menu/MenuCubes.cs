using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MenuCubes : MonoBehaviour
{
    private float RandomX, RandomY, RandomZ;
    private Vector3 EndPosition, Rotation;
    private void Awake()
    {
        Destroy(gameObject, 25f);
        EndPosition = transform.position + new Vector3(Random.Range(30, 40), Random.Range(20,30), 0);
        EndPosition.Normalize();
        EndPosition *= Random.Range(2.5f, 4f);
        RandomX = Random.Range(-10, 10) ;
        RandomY = Random.Range(-10, 10) ;
        RandomZ = Random.Range(-10, 10) ;
        GetComponent<Rigidbody>().AddForce(EndPosition, ForceMode.Impulse);
        Rotation=new Vector3 (RandomX, RandomY, RandomZ);   
        GetComponent<Rigidbody>().AddTorque(Rotation);
    }
    void Update()
    {
      
    }
}
