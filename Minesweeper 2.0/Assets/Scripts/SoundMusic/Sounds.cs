using UnityEngine;

public class Sounds : MonoBehaviour
{
    public AudioClip[] AllSounds;
    [SerializeField] private AudioSource _audioSource;

    [SerializeField] private float CellVolume = 0.7f, SmileyVolume = 0.5f, ExplosionVolume =0.55f, ButtonOverVolume = 0.6f, ButtonClick=0.6f, VictoryVolume=0.8f;
    private void Awake()
    {
        StartingSequence.onCubesFallSmiley += PlaySmiley;
        VictoryHandler.onGameWon += PlayVictory;
        EventBus.OnGameLose += PlayExplosion;
        GameModesSystem.PlaySound += PlayOnMouseOverButton;
        ClickRegister.PlaySound += PlayOnCellClick;
        InGameButtons.PlaySound += PlayOnButtonClick;
        ButtonsInMenu.PlaySound += PlayOnButtonClick;
        EventBus.OnButtonClick += PlayOnButtonClick;
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
            PlaySound(AllSounds[5], SmileyVolume, ChangePitch:false);
    }
    private void PlaySound(AudioClip Clip, float Volume = 1f, float Pitch1 = 0.9f, float Pitch2 = 1.1f, float Delay = 0, bool ChangePitch = true)
    {
        if (ChangePitch)
        {
            _audioSource.pitch = Random.Range(Pitch1, Pitch2);
        }
        else
        {
            _audioSource.pitch = 1f;
        }

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