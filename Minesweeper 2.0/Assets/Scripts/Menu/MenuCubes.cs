using UnityEngine;
using TMPro;

public class MenuCubes : MonoBehaviour
{
    private float RandomX, RandomY, RandomZ;
    private Vector3 EndPosition, Rotation;

    [SerializeField] private MeshRenderer[] _meshRenderers;
    [SerializeField] private TMP_Text[] _numbers;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private float _torqueForce = 10f;
    [SerializeField] private float _minPushForce = 2.5f;
    [SerializeField] private float _maxPushForce = 4.5f;
    [SerializeField] private float _lifeTime = 25f;
    [SerializeField] private float _horizontalMinPositionChange = 30;
    [SerializeField] private float _horizontalMaxPositionChange = 40;
    [SerializeField] private float _verticalMinPositionChange = 20;
    [SerializeField] private float _verticalMaxPositionChange = 30;

    public MeshRenderer[] MeshRenderers => _meshRenderers;

    public void SetNumbers(string number)
    {
        for (int i = 0; i < _numbers.Length; i++)
        {
            _numbers[i].text = number;
        }
    }

    private void Awake()
    {
        Destroy(gameObject, _lifeTime);
        EndPosition = transform.position + new Vector3(Random.Range(_horizontalMinPositionChange, _horizontalMaxPositionChange), Random.Range(_verticalMinPositionChange, _verticalMaxPositionChange), 0);
        EndPosition.Normalize();
        EndPosition *= Random.Range(_minPushForce, _maxPushForce);
        RandomX = Random.Range(-_torqueForce, _torqueForce);
        RandomY = Random.Range(-_torqueForce, _torqueForce);
        RandomZ = Random.Range(-_torqueForce, _torqueForce);
        _rigidbody.AddForce(EndPosition, ForceMode.Impulse);
        Rotation = new Vector3(RandomX, RandomY, RandomZ);
        _rigidbody.AddTorque(Rotation);
    }
}