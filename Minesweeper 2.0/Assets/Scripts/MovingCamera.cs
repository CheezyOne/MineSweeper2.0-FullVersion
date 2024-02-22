
using System.Threading;
using UnityEngine;

public class MovingCamera : MonoBehaviour
{
    private float MouseSpeed=8f;
    public static bool isAbleToMove = false, isMobileMovement = false;
    private bool isAbleToBackScrollCamera = false, GetBorderPositionOnce = true;
    private float RightBorder, UpperBorder, Timer=0.5f;
    private const float LeftBorder=-5.5f, LowerBorder=-2.2f;
    private Vector3 DragStartPosition;

    public float movementSpeed = 0.1f; // �������� �������� ������

    private Vector2 initialTouchPosition;
    private Vector2 previousTouchPosition;

    private Transform MainCamTransform;
    private void OnEnable()
    {
        MainCamTransform = Camera.main.transform;
        InGameButtons.onGameExit += LockTheCamera;
        StartingSequence.onAllCubesFall += LetTheScroll;
    }
    private void MouseCamDrag()
    {
        if (Input.GetMouseButtonDown(0)) // ��������� ������� ���
        {
            DragStartPosition = Input.mousePosition; // ���������� ��������� ������� ����
        }
        else if (Input.GetMouseButton(0)) // ���� ��� ������������
        {
            Vector3 delta = Input.mousePosition - DragStartPosition; // ��������� ��������� ������� ����
            //Stop the camera 
            if (transform.position.x < LeftBorder && delta.x > 0 || transform.position.x > FieldGeneration.StaticStringsSize * 1.1f - 5 && delta.x < 0)
                delta.x = 0;
            if (transform.position.z < LowerBorder && delta.y > 0 || transform.position.z > FieldGeneration.StaticColomsSize * 1.1f - 2 && delta.y < 0)
                delta.y = 0;
            delta /= 1.5f;
            MainCamTransform.Translate(-delta * Time.deltaTime); // ���������� ������ � ��������������� ����������� �� �������� ����
            DragStartPosition = Input.mousePosition; // ��������� ������� ����
        }
    }
    private void TouchCamDrag()
    {
        if (Input.touchCount == 1)
        {

            // ��������� �������� ������ �� ������
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                DragStartPosition = touch.position;
            }
            Vector3 delta = Input.mousePosition - DragStartPosition; // ��������� ��������� ������� ����
            //Stop the camera 
            if (transform.position.x < LeftBorder && delta.x > 0 || transform.position.x > FieldGeneration.StaticStringsSize * 1.1f - 5 && delta.x < 0)
                delta.x = 0;
            if (transform.position.z < LowerBorder && delta.y > 0 || transform.position.z > FieldGeneration.StaticColomsSize * 1.1f - 2 && delta.y < 0)
                delta.y = 0;
            delta /= 1.5f;
            MainCamTransform.Translate(-delta * Time.deltaTime); // ���������� ������ � ��������������� ����������� �� �������� ����
            DragStartPosition = touch.position; 
        }
        else if (Input.touchCount == 2)
        {
            DragStartPosition = Vector3.zero;
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            if (touch0.phase == TouchPhase.Began || touch1.phase == TouchPhase.Began)
            {
                initialTouchPosition = (touch0.position + touch1.position) / 2f;
                previousTouchPosition = initialTouchPosition;
            }
            else if (touch0.phase == TouchPhase.Moved && touch1.phase == TouchPhase.Moved)
            {
                Vector2 currentTouchPosition = (touch0.position + touch1.position) / 2f;
                float deltaY = currentTouchPosition.y - previousTouchPosition.y;

                Vector3 cameraNewPosition = transform.position + new Vector3(0f, deltaY * movementSpeed, 0f);
                transform.position = cameraNewPosition;

                previousTouchPosition = currentTouchPosition;
            }
        }
    }
    private void MouseWASD()
    {
        //Take the axises
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        if (Input.GetKeyDown(KeyCode.LeftShift))
            MouseSpeed *= 2;
        if (Input.GetKeyUp(KeyCode.LeftShift))
            MouseSpeed /= 2;
        //mouse scroll
        if (isAbleToBackScrollCamera)
        {
            Vector3 CamPos = Camera.main.transform.position;
            if (!((CamPos.y <= 3 && Input.mouseScrollDelta.y > 0) || (CamPos.y > 50 && Input.mouseScrollDelta.y < 0)))
                CamPos.y -= Input.mouseScrollDelta.y;
            Camera.main.transform.position = CamPos;
        }

        //Stop the camera 
        if (transform.position.x < LeftBorder && horizontal < 0 || transform.position.x > FieldGeneration.StaticStringsSize * 1.1f - 5 && horizontal > 0)
            horizontal = 0;
        if (transform.position.z < LowerBorder  && vertical < 0 || transform.position.z > FieldGeneration.StaticColomsSize * 1.1f - 2 && vertical > 0)
            vertical = 0;
        transform.position += new Vector3(horizontal, 0, vertical) * MouseSpeed * Time.deltaTime;
    }
    private void LetTheScroll()
    {
        isAbleToBackScrollCamera = true;
    }
    private void LockTheCamera()
    {
        isAbleToMove = false;
        isAbleToBackScrollCamera = false;
        GetBorderPositionOnce = true;
        Timer = 0.5f;
        MouseSpeed = 8;
    }
    private void BringCameraBack()
    {
        if (MainCamTransform.position.x < LeftBorder)
            MainCamTransform.position = Vector3.Lerp(MainCamTransform.position, new Vector3(LeftBorder, MainCamTransform.position.y, MainCamTransform.position.z),Time.deltaTime*3);
        if (MainCamTransform.position.z < LowerBorder)
            MainCamTransform.position = Vector3.Lerp(MainCamTransform.position, new Vector3(MainCamTransform.position.x, MainCamTransform.position.y, LowerBorder), Time.deltaTime * 3);
        if (MainCamTransform.position.x > RightBorder)
            MainCamTransform.position = Vector3.Lerp(MainCamTransform.position, new Vector3(RightBorder, MainCamTransform.position.y, MainCamTransform.position.z), Time.deltaTime * 3);
        if (MainCamTransform.position.z > UpperBorder)
            MainCamTransform.position = Vector3.Lerp(MainCamTransform.position, new Vector3(MainCamTransform.position.x, MainCamTransform.position.y, UpperBorder), Time.deltaTime * 3);

    }
    void Update()
    {
        if(!isAbleToMove)
        {
            return;
        }
        else
        {
            Timer -= Time.deltaTime;
        }
        if (Timer > 0)
            return;
        if(GetBorderPositionOnce)
        {
            RightBorder = FieldGeneration.StaticStringsSize * 1.1f - 5;
            UpperBorder = FieldGeneration.StaticColomsSize * 1.1f - 2;
            GetBorderPositionOnce = false;
        }
        if(isMobileMovement)
        {
            TouchCamDrag();
        }
        else
        {
            MouseCamDrag();
            MouseWASD();
        }
        BringCameraBack();
    }

}
