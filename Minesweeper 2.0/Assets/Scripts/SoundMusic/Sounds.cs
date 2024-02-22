using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sounds : MonoBehaviour
{
    public AudioClip[] AllSounds;
    private AudioSource _audioSource => GetComponent<AudioSource>();

    private const float CellVolume = 0.7f, SmileyVolume = 0.5f, ExplosionVolume =0.55f, ButtonOverVolume = 0.6f, ButtonClick=0.6f, VictoryVolume=0.8f;
    private void Awake()
    {
        StartingSequence.onCubesFallSmiley += PlaySmiley;
        VictoryHandler.onGameWon += PlayVictory;
        Cell.PlaySound += PlayExplosion;
        GameModesSystem.PlaySound += PlayOnMouseOverButton;
        ClickRegister.PlaySound += PlayOnCellClick;
        InGameButtons.PlaySound += PlayOnButtonClick;
        ButtonsInMenu.PlaySound += PlayOnButtonClick;
        LanguageController.PlaySound += PlayOnButtonClick;
        InfoAboutModes.PlaySound += PlayOnButtonClick;
        SoundButtons.PlaySound += PlayOnButtonClick;
    }

    private void PlayOnMouseOverButton()
    {
        PlaySound(AllSounds[1], ButtonOverVolume);
    }
    private void PlayOnButtonClick()
    {
        PlaySound(AllSounds[3], ButtonClick);
    }
    private void PlayOnCellClick()
    {
        PlaySound(AllSounds[2], CellVolume);
    }
    private void PlayExplosion()
    {
        PlaySound(AllSounds[0], ExplosionVolume);
    }
    private void PlayVictory()
    {
        PlaySound(AllSounds[4], VictoryVolume);
    }
    private void PlaySmiley(int i)
    {
        if(i==2)//i checks smiley 
            PlaySound(AllSounds[5], SmileyVolume);
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