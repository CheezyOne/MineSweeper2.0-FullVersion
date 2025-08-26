using UnityEngine;

public class MovingCamera : Singleton<MovingCamera>
{
    private float MouseSpeed=8f;
    public static bool isAbleToMove = false, isMobileMovement = false;
    private bool isAbleToBackScrollCamera = false, GetBorderPositionOnce = true;
    private float RightBorder, UpperBorder, Timer=0.5f;
    private const float LeftBorder=-5.5f, LowerBorder=-2.2f;
    private Vector3 DragStartPosition;
    public Camera MainCamera;

    public float movementSpeed = 0.1f; // Скорость движения камеры

    private Vector2 previousTouchPosition2;
    private Vector2 previousTouchPosition1;

    private Transform MainCamTransform;

    private void OnEnable()
    {
        MainCamTransform = MainCamera.transform;
        InGameButtons.onGameExit += LockTheCamera;
        EventBus.OnAllCubesFall += LetTheScroll;
    }

    private void OnDisable()
    {
        InGameButtons.onGameExit -= LockTheCamera;
        EventBus.OnAllCubesFall -= LetTheScroll;
    }

    private void MouseCamDrag()
    {
        if (Input.GetMouseButtonDown(0)) // Проверяем нажатие ЛКМ
        {
            DragStartPosition = Input.mousePosition; // Запоминаем начальную позицию мыши
        }
        else if (Input.GetMouseButton(0)) // Если ЛКМ удерживается
        {
            Vector3 delta = Input.mousePosition - DragStartPosition; // Вычисляем изменение позиции мыши
            //Stop the camera 
            if (transform.position.x < LeftBorder && delta.x > 0 || transform.position.x > FieldGeneration.StaticStringsSize * 1.1f - 5 && delta.x < 0)
                delta.x = 0;
            if (transform.position.z < LowerBorder && delta.y > 0 || transform.position.z > FieldGeneration.StaticColomsSize * 1.1f - 2 && delta.y < 0)
                delta.y = 0;
            delta /= 1.5f;
            MainCamTransform.Translate(-delta * Time.deltaTime); // Перемещаем камеру в противоположном направлении от движения мыши
            DragStartPosition = Input.mousePosition; // Обновляем позицию мыши
        }
    }

    private void TouchCamDrag()
    {
        if (Input.touchCount == 1)
        {
            // Обработка движения камеры по скрину
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                DragStartPosition = touch.position;
            }
            Vector3 delta = Input.mousePosition - DragStartPosition; // Вычисляем изменение позиции мыши
            //Stop the camera 
            if (transform.position.x < LeftBorder && delta.x > 0 || transform.position.x > FieldGeneration.StaticStringsSize * 1.1f - 5 && delta.x < 0)
                delta.x = 0;
            if (transform.position.z < LowerBorder && delta.y > 0 || transform.position.z > FieldGeneration.StaticColomsSize * 1.1f - 2 && delta.y < 0)
                delta.y = 0;

            delta /= 1.5f;
            float ClampFloat = 19f;
            delta = new (Mathf.Clamp(delta.x, -ClampFloat, ClampFloat), Mathf.Clamp(delta.y, -ClampFloat, ClampFloat), Mathf.Clamp(delta.z, -ClampFloat, ClampFloat));
            MainCamTransform.Translate(-delta * Time.deltaTime); // Перемещаем камеру в противоположном направлении от движения мыши
            DragStartPosition = touch.position; 
        }
        else if (Input.touchCount == 2 && isAbleToBackScrollCamera)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            if (touch1.phase == TouchPhase.Moved && touch2.phase == TouchPhase.Moved)
            {
                Vector2 currentTouchPosition1 = touch1.position;
                Vector2 currentTouchPosition2 = touch2.position;

                float previousTouchDelta = Vector2.Distance(previousTouchPosition1, previousTouchPosition2);
                float currentTouchDelta = Vector2.Distance(currentTouchPosition1, currentTouchPosition2);

                float deltaDifference = currentTouchDelta - previousTouchDelta;

                Vector3 cameraPosition = transform.position;
                deltaDifference = Mathf.Clamp(deltaDifference, -50, 50);
                if (!(transform.position.y<=7&&deltaDifference>0|| transform.position.y >= 50 && deltaDifference <0))
                    cameraPosition.y -= deltaDifference * Time.deltaTime;
                transform.position = cameraPosition;

                previousTouchPosition1 = currentTouchPosition1;
                previousTouchPosition2 = currentTouchPosition2;
            }
            else if (touch1.phase == TouchPhase.Began || touch2.phase == TouchPhase.Began)
            {
                previousTouchPosition1 = touch1.position;
                previousTouchPosition2 = touch2.position;
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
            Vector3 CamPos = MainCamTransform.position;
            if (!((CamPos.y <= 7 && Input.mouseScrollDelta.y > 0) || (CamPos.y > 50 && Input.mouseScrollDelta.y < 0)))
                CamPos.y -= Input.mouseScrollDelta.y;
            MainCamTransform.position = CamPos;
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

    private void Update()
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