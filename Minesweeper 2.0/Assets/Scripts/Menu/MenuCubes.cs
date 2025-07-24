using UnityEngine;
using TMPro;

public class MenuCubes : MonoBehaviour
{
    private float RandomX, RandomY, RandomZ;
    private Vector3 EndPosition, Rotation;

    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private TMP_Text[] _numbers;
    [SerializeField] private Rigidbody _rigidbody;

    public MeshRenderer MeshRenderer => _meshRenderer;

    public void SetNumbers(string number)
    {
        for (int i =0;i<number.Length;i++)
        {
            _numbers[i].text = number;
        }
    }

    private void Awake()
    {
        Destroy(gameObject, 25f);
        EndPosition = transform.position + new Vector3(Random.Range(30, 40), Random.Range(20,30), 0);
        EndPosition.Normalize();
        EndPosition *= Random.Range(2.5f, 4f);
        RandomX = Random.Range(-10, 10) ;
        RandomY = Random.Range(-10, 10) ;
        RandomZ = Random.Range(-10, 10) ;
        _rigidbody.AddForce(EndPosition, ForceMode.Impulse);
        Rotation=new Vector3 (RandomX, RandomY, RandomZ);
        _rigidbody.AddTorque(Rotation);
    }
}