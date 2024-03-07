using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LoseHandler : MonoBehaviour
{
    private int CameraShakeCounter = 0;
    private const int TimesToShakeCamera=20;
    private Vector3 NormalCameraPosition;
    [SerializeField] private float ShakeForce = 0.3f, TimeBetweenShakes=0.04f;
    private bool StopShakingBool;

    private void OnEnable()
    {
        InGameButtons.onGameExit += StopShaking;
    }
    private void OnDisable()
    {
        InGameButtons.onGameExit -= StopShaking;
    }
    private void StopShaking()
    {
        StopShakingBool = true;
        StopAllCoroutines();
    }
    private void Awake()
    {
        Cell.onGameLose += GameLose;
    }
    private void GameLose()
    {
        StopShakingBool = false;
        CameraShakeCounter = 0;
        NormalCameraPosition = Camera.main.transform.position;
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
        Camera.main.transform.position = NormalCameraPosition + AddingCameraPosition;
    }
    private void SetCameraPositionToNormal()
    {
        Camera.main.transform.position = NormalCameraPosition;
    }
}
