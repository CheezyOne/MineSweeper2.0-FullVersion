using System.Collections;
using UnityEngine;

public class LoseHandler : MonoBehaviour
{
    private int CameraShakeCounter = 0;
    private const int TimesToShakeCamera=20;
    private Vector3 NormalCameraPosition;
    [SerializeField] private float ShakeForce = 0.3f, TimeBetweenShakes=0.04f;
    private bool StopShakingBool;
    private Transform _mainCamera;

    private void Awake()
    {
        _mainCamera = Camera.main.transform;
    }

    private void OnEnable()
    {
        InGameButtons.onGameExit += StopShaking;
        Cell.onGameLose += GameLose;
    }
    private void OnDisable()
    {
        InGameButtons.onGameExit -= StopShaking;
        Cell.onGameLose -= GameLose;
    }
    private void StopShaking()
    {
        StopShakingBool = true;
        StopAllCoroutines();
    }
    private void GameLose()
    {
        StopShakingBool = false;
        CameraShakeCounter = 0;
        NormalCameraPosition = _mainCamera.position;
        StartCoroutine(CameraShake());
        ClickRegister.isGameOn = false;
    }
    private IEnumerator CameraShake()
    {
        yield return new WaitForSeconds(TimeBetweenShakes);
        if (StopShakingBool)
            yield break;
        ChangeCameraPosition();
        if (CameraShakeCounter <= TimesToShakeCamera)
        {
            CameraShakeCounter++;
            yield return CameraShake();
        }
        else
        {
            SetCameraPositionToNormal();
            StopCoroutine(CameraShake());
        }
    }
    private void ChangeCameraPosition()
    {
        Vector3 AddingCameraPosition = new (
            Random.Range(-ShakeForce, ShakeForce),
            Random.Range(-ShakeForce, ShakeForce),
            Random.Range(-ShakeForce, ShakeForce));
        _mainCamera.position = NormalCameraPosition + AddingCameraPosition;
    }
    private void SetCameraPositionToNormal()
    {
        _mainCamera.position = NormalCameraPosition;
    }
}
