using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlusMinusButtons : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    private const float _constTime = 0.3f;
    [SerializeField] private bool[] _purpose;//0-strings, 1- colomns, 2-both
    [SerializeField] private bool _isPositive = false;
    public static Action onFieldSizeChange;
    public static Action<bool> onBombSizeChange;
    private bool _isToStopCoroutine;
    private float _time = 0.3f, _decreaseTime = 0.05f;
    public void Plus()
    {
        if (_purpose[0])
            FieldSizeMenu.ChangeSize("string", true);
        else if (_purpose[1])
            FieldSizeMenu.ChangeSize("colomn", true);
        else if (_purpose[2])
            FieldSizeMenu.ChangeSize("both", true);
        else if (_purpose[3])
            onBombSizeChange?.Invoke(true);
    }
    private IEnumerator PlusTimer()
    {
        if (_isToStopCoroutine)
            yield break;
        Plus();
        ShoutOutAction();
        yield return new WaitForSeconds(_time);
        if (_time <= 0.2f)
            _time = 0.1f;
        else
            _time -= _decreaseTime;
        yield return PlusTimer();
    }
    private IEnumerator MinusTimer()
    {
        if (_isToStopCoroutine)
            yield break;
        Minus();
        ShoutOutAction();
        yield return new WaitForSeconds(_time);
        if (_time <= 0.2f)
            _time = 0.1f;
        else
            _time -= _decreaseTime;
        yield return MinusTimer();
    }
    public void Minus()
    {
        if (_purpose[0])
            FieldSizeMenu.ChangeSize("string", false);
        else if (_purpose[1])
            FieldSizeMenu.ChangeSize("colomn", false);
        else if (_purpose[2])
            FieldSizeMenu.ChangeSize("both", false);
        else if (_purpose[3])
            onBombSizeChange?.Invoke(false);
    }
    private void ShoutOutAction()
    {
        onFieldSizeChange?.Invoke();
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        _time = _constTime;
        _isToStopCoroutine = true;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        StopAllCoroutines();
        _isToStopCoroutine = false;
        if(_isPositive)
            StartCoroutine(PlusTimer());
        else
            StartCoroutine(MinusTimer());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _time = _constTime;
        _isToStopCoroutine = true; 
    }
}