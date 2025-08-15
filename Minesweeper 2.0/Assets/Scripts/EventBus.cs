using System;
using UnityEngine;

public class EventBus : MonoBehaviour
{
    public static Action OnGameLose;
    public static Action<int> OnChangeSmiley;
    public static Action OnButtonClick;
    public static Action OnAllCubesFall;
}