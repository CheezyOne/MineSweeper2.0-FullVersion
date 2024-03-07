using UnityEngine;
using System;
using UnityEngine.UI;
public class SoundButtons : MonoBehaviour
{
    public static Action PlaySound;
    [SerializeField] private GameObject MusicHandler, SoundsHandler;
    [SerializeField] private GameObject[] MusicButtonsArr, SoundButtonsArr;
    [SerializeField] private Sprite MutedMusic, UnMutedMusic, MutedSound, UnMutedSound;
    private const float MusicVolume = 0.1f;
    public void MusicButtonFun()
    {
        AudioSource AS = MusicHandler.GetComponent<AudioSource>();
        PlaySound?.Invoke();
        if (AS.volume != 0)
        {
            for (int i = 0; i< MusicButtonsArr.Length;i++)
            {
                MusicButtonsArr[i].GetComponent<Image>().sprite = MutedMusic;
            }
            AS.volume = 0;
        }
        else
        {
            for (int i = 0; i < MusicButtonsArr.Length; i++)
            {
                MusicButtonsArr[i].GetComponent<Image>().sprite = UnMutedMusic;
            }
            AS.volume = MusicVolume;
        }
    }
    public void SoundsButtonFun()
    {
        PlaySound?.Invoke();
        if (SoundsHandler.activeSelf)
        {
            for (int i = 0; i < SoundButtonsArr.Length; i++)
            {
                SoundButtonsArr[i].GetComponent<Image>().sprite = MutedSound;
            }
            SoundsHandler.SetActive(false);
        }
        else
        {
            for (int i = 0; i < SoundButtonsArr.Length; i++)
            {
                SoundButtonsArr[i].GetComponent<Image>().sprite = UnMutedSound;
            }
            SoundsHandler.SetActive(true);
        }
    }
}
