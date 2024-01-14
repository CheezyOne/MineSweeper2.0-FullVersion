using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class MovingCamera : MonoBehaviour
{
    private int Speed=8;//
    public static bool isAbleToMove = false;
    private bool isAbleToBackScrollCamera = false;
    private void OnEnable()
    {
        InGameButtons.onGameExit += LockTheCamera;
        StartingSequence.onAllCubesFall += LetTheScroll;
    }
    private void LetTheScroll()
    {
        isAbleToBackScrollCamera = true;
    }
    private void LockTheCamera()
    {
        isAbleToMove = false;
        isAbleToBackScrollCamera = false;
        Speed = 8;
    }
    void Update()
    {
        if(!isAbleToMove)
        {
            return;
        }
        //Take the axises
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        if (Input.GetKeyDown(KeyCode.LeftShift))
            Speed *= 2;
        if (Input.GetKeyUp(KeyCode.LeftShift))
            Speed /= 2;
        //mouse scroll
        if (isAbleToBackScrollCamera)
        {
            Vector3 CamPos = Camera.main.transform.position;
            if (!((CamPos.y <= 3 && Input.mouseScrollDelta.y > 0) || (CamPos.y > 50 && Input.mouseScrollDelta.y < 0)))
                CamPos.y -= Input.mouseScrollDelta.y;
            Camera.main.transform.position = CamPos;
        }

        //Stop the camera 
        if (transform.position.x <-5.5 && horizontal<0 || transform.position.x > FieldGeneration.StaticStringsSize*1.1f-5 && horizontal > 0)
            horizontal = 0;
        if (transform.position.z < -2.2 && vertical < 0 || transform.position.z > FieldGeneration.StaticColomsSize*1.1f-2  && vertical > 0)
            vertical = 0;
        transform.position += new Vector3(horizontal, 0, vertical) * Speed * Time.deltaTime;
    }

}
