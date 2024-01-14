using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmileyFlip : MonoBehaviour
{
    [SerializeField] private GameObject[] Smileys = new GameObject[10];
    private bool ShouldRotate = false, RotateRight = true;
    private int CurrentSmileyIndex = -1, NextSmileyIndex = -1;
    private float  RotationSpeed=1000f, TimerOfSmiley=10f;
    private void Awake()
    {
        ClickRegister.onCubeTouch += GetNextSmileyIndex;
        ClickRegister.onCubeRelease += GetNextSmileyIndex;
        Cell.onGameLoseSmiley += GetNextSmileyIndex;
        VictoryHandler.onVictorySmiley += GetNextSmileyIndex;
        StartingSequence.onCubesFallSmiley+= GetNextSmileyIndex;
    }
    
    private void GetNextSmileyIndex(int SmileyIndex)
    {
        if (!ClickRegister.isGameOn && SmileyIndex!=7 && SmileyIndex!=5)
            return;
        if ((CurrentSmileyIndex == 1 || CurrentSmileyIndex == 0) && SmileyIndex == 0)
            return;
        TimerOfSmiley = 10f;
        if (Random.Range(0,100)>50)
        {
            if (SmileyIndex == 0 || SmileyIndex == 3 || SmileyIndex==5|| SmileyIndex==7)
                SmileyIndex++;
        }
        NextSmileyIndex = SmileyIndex;
    }
    private void OnEnable()
    {
        CreateNewSmiley(0);
    }
    private void CreateNewSmiley(int SmileyIndex)
    {
        if (SmileyIndex == CurrentSmileyIndex)
            return;
        GameObject Smiley;
        if (RotateRight)
            Smiley = Instantiate(Smileys[SmileyIndex], transform.position, Quaternion.LookRotation(transform.forward, -transform.right), transform);
        else
            Smiley=Instantiate(Smileys[SmileyIndex], transform.position , Quaternion.LookRotation(transform.forward, transform.right), transform);
        DeleteSmileyShadows(Smiley);
        CurrentSmileyIndex = SmileyIndex;
        ShouldRotate = true;
        DestroyExcessiveSmiley();
    }
    private void DeleteSmileyShadows(GameObject Smiley)
    {
        Smiley.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        Smiley.GetComponent<MeshRenderer>().receiveShadows=false;
        for(int i=0;i<Smiley.transform.childCount;i++)
        {
            MeshRenderer SmileyMR = Smiley.transform.GetChild(i).GetComponent<MeshRenderer>();
            SmileyMR.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            SmileyMR.receiveShadows = false;
        }
    }
    private void Rotate180()
    {
        if(transform.rotation.eulerAngles.z < 180)
        {
            transform.Rotate(new Vector3(0, 0, Time.deltaTime* RotationSpeed));
        }
        else
        {
            transform.rotation = Quaternion.Euler(0,0,-180);
            if (transform.childCount > 1)   
                Destroy(transform.GetChild(0).gameObject);
            ShouldRotate = false;
            RotateRight = false;
        }
    }
    private void Rotate0()
    {
        if (transform.rotation.eulerAngles.z <= 360 && transform.rotation.eulerAngles.z >= 180)
        {
            transform.Rotate(new Vector3(0, 0, Time.deltaTime * RotationSpeed));
        }
        else
        {
            RotateRight = true;
            transform.rotation = Quaternion.Euler(0, 0, 0);
            if(transform.childCount>1)
                Destroy(transform.GetChild(0).gameObject);
            ShouldRotate = false;
        }
    }
    private void DestroyExcessiveSmiley()
    {
        if (transform.childCount > 2)
            Destroy(transform.GetChild(0).gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        if (ClickRegister.isGameOn)
        {
            if (TimerOfSmiley <= 0)
            {
                TimerOfSmiley = 10f;
                GetNextSmileyIndex(9);
            }
            TimerOfSmiley -= Time.deltaTime;
        }
        if (NextSmileyIndex != -1 && !ShouldRotate && NextSmileyIndex != CurrentSmileyIndex)
        {
            CreateNewSmiley(NextSmileyIndex);
            NextSmileyIndex = -1;
        }
        if (ShouldRotate)
        {
            if (RotateRight)
                Rotate180();
            else
                Rotate0();
        }
    }
}
