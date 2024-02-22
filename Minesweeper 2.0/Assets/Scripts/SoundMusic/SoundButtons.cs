using UnityEngine;
using System;
using UnityEngine.UI;
public class SoundButtons : MonoBehaviour
{
    public static Action PlaySound;
    [SerializeField] private GameObject MusicHandler, MusicButton, MusicButton1, SoundsHandler, SoundButton, SoundButton1;
    [SerializeField] private Sprite MutedMusic, UnMutedMusic, MutedSound, UnMutedSound;
    private const float MusicVolume = 0.1f;
    public void MusicButtonFun()
    {
        AudioSource AS = MusicHandler.GetComponent<AudioSource>();
        PlaySound?.Invoke();
        if (AS.volume != 0)
        {
            MusicButton.GetComponent<Image>().sprite = MutedMusic;
            MusicButton1.GetComponent<Image>().sprite = MutedMusic;
            AS.volume = 0;
        }
        else
        {
            MusicButton.GetComponent<Image>().sprite = UnMutedMusic;
            MusicButton1.GetComponent<Image>().sprite = UnMutedMusic;
            AS.volume = MusicVolume;
        }
    }
    public void SoundsButtonFun()
    {
        PlaySound?.Invoke();
        if (SoundsHandler.activeSelf)
        {
            SoundButton.GetComponent<Image>().sprite = MutedSound;
            SoundButton1.GetComponent<Image>().sprite = MutedSound;
            SoundsHandler.SetActive(false);
        }
        else
        {
            SoundButton.GetComponent<Image>().sprite = UnMutedSound;
            SoundButton1.GetComponent<Image>().sprite = UnMutedSound;
            SoundsHandler.SetActive(true);
        }
    }
}
