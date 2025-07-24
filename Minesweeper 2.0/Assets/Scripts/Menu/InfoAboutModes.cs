using System;
using UnityEngine;

public class InfoAboutModes : MonoBehaviour
{
    private bool isToBeExpanded = false, isToBeShrinked = false, isToRevealText = false;
    [SerializeField] private GameObject Tip;
    [SerializeField] private CanvasGroup TipCG;
    [SerializeField] private float XScale, YScale;
    [SerializeField] private Vector3 TargetPosition;
    [SerializeField] private float ScaleExpandSpeed, ScaleShinkSpeed, PositionSpeed, TextRevealSpeed;

    public static Action PlaySound;

    public void OnQuestionOver()
    {
        PlaySound?.Invoke();
        isToBeExpanded = true;
        isToBeShrinked = false;
        Tip.SetActive(true);
    }
    public void OnQuestionExit()
    {
        isToRevealText = false;
        isToBeShrinked = true;
        isToBeExpanded = false;
    }
    private void FixedUpdate()
    {
        if (isToBeExpanded)
        {
            MoveToOpen();
            Expand();
        }
        else if (isToBeShrinked)
        {
            MoveToClose();
            Shrink();
        }
        if (isToRevealText)
            TipCG.alpha += TextRevealSpeed;
        else
            TipCG.alpha -= TextRevealSpeed;

    }
    private void NullPosition()
    {
        Tip.transform.localPosition = Vector3.zero;
    }
    private void MoveToOpen()
    {
        Tip.transform.localPosition = Vector3.MoveTowards(Tip.transform.localPosition, TargetPosition, PositionSpeed);
    }
    private void MoveToClose()
    {
        Tip.transform.localPosition = Vector3.MoveTowards(Tip.transform.localPosition, Vector3.zero, PositionSpeed);
    }
    private void Shrink()
    {
        Vector3 NewScale = new Vector3(Tip.transform.localScale.x, Tip.transform.localScale.y, Tip.transform.localScale.z);
        if (Tip.transform.localScale.x > 0)
        {
            NewScale.x -= ScaleShinkSpeed;
        }
        if (Tip.transform.localScale.y > 0)
        {
            NewScale.y -= ScaleShinkSpeed;
        }
        if (Tip.transform.localScale.x <= 0 && Tip.transform.localScale.y <= 0)
        {
            NullPosition();
            Tip.SetActive(false);
            isToBeShrinked = false;
        }
        Tip.transform.localScale = NewScale;
    }
    private void Expand()
    {
        Vector3 NewScale=new Vector3 (Tip.transform.localScale.x, Tip.transform.localScale.y, Tip.transform.localScale.z);
        if (Tip.transform.localScale.x < XScale)
        {
            NewScale.x += ScaleExpandSpeed;
        }
        if(Tip.transform.localScale.y < YScale)
        {
            NewScale.y += ScaleExpandSpeed;
        }
        Tip.transform.localScale = NewScale;
        if(Tip.transform.localScale.x>=XScale && Tip.transform.localScale.y>=YScale)
        {
            isToBeExpanded = false;
            isToRevealText = true;
        }
    }
}
