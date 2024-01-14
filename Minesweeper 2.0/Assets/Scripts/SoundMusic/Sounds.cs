using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sounds : MonoBehaviour
{
    public AudioClip[] AllSounds;
    private AudioSource _audioSource => GetComponent<AudioSource>();

    private void Awake()
    {
        //Подписаться на всё
        StartingSequence.onAllCubesFall += PlaySmiley;
        VictoryHandler.onGameWon += PlayVictory;
        Cell.PlaySound += PlayExplosion;
        GameModesSystem.PlaySound += PlayOnMouseOverButton;
        ClickRegister.PlaySound += PlayOnCellClick;
        InGameButtons.PlaySound += PlayOnButtonClick;
        ButtonsInMenu.PlaySound += PlayOnButtonClick;
        LanguageController.PlaySound += PlayOnButtonClick;
        InfoAboutModes.PlaySound += PlayOnButtonClick;
        StartCoroutine(FuckMeSideways());
    }
    private IEnumerator FuckMeSideways()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("Fuck me sideways");
        _audioSource.Play();
        _audioSource.Play();
        yield return FuckMeSideways();
    }
    private void PlayOnMouseOverButton()
    {
        PlaySound(AllSounds[1]);
    }
    private void PlayOnButtonClick()
    {
        PlaySound(AllSounds[3]);
    }
    private void PlayOnCellClick()
    {
        PlaySound(AllSounds[2]);
    }
    private void PlayExplosion()
    {
        PlaySound(AllSounds[0]);
    }
    private void PlayVictory()
    {
        PlaySound(AllSounds[4]);
    }
    private void PlaySmiley()
    {
        PlaySound(AllSounds[5], Delay:0.5f);
    }
    private void PlaySound(AudioClip Clip, float Volume = 1f, float Pitch1 = 0.9f, float Pitch2 = 1.1f, float Delay = 0)
    {
        _audioSource.clip = Clip;
        _audioSource.volume = Volume;
        _audioSource.pitch = Random.Range(Pitch1, Pitch2);
        if (Delay == 0)
        {
            _audioSource.PlayOneShot(Clip, Volume);
        }
        else
        {
            _audioSource.PlayDelayed(Delay);
        }
    }
}