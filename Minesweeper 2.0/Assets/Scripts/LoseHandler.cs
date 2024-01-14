using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LoseHandler : MonoBehaviour
{
    private int CameraShakeCounter = 0, TimesToShakeCamera=20;
    private Vector3 NormalCameraPosition;
    [SerializeField] private float ShakeForce = 0.3f, TimeBetweenShakes=0.04f;
    private void Awake()
    {
        Cell.onGameLose += GameLose;
    }
    private void GameLose()
    {
        CameraShakeCounter = 0;
        NormalCameraPosition = Camera.main.transform.position;
        StartCoroutine(CameraShake());
        ClickRegister.isGameOn = false;
    }
    private IEnumerator CameraShake()
    {
        yield return new WaitForSeconds(TimeBetweenShakes);
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
        Vector3 AddingCameraPosition = new Vector3(
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
