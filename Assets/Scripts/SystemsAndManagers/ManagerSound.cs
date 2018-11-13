using UnityEngine;

public class ManagerSound : SingltoonBehavior<ManagerSound>
{
    
    
	public SimpleAudioEvent[] AudionEvent;
		
	[Space(10)]
	public AudioSource ChanelMusic;
    public AudioSource[] ChanelFX;

    public AudioClip MainTrack;

    void Start()
    {
        EventManager.StartListening("ChangeSound", ChangeVolumeSound);
        EventManager.StartListening("ChangeMusic", ChangeVolumeMusic);
        ChangeVolumeMusic();
        ChangeVolumeSound();
    }


    private void ChangeVolumeMusic()
    {
        if (Data.Instance.Music) PlayMusic();

    }

    private void ChangeVolumeSound()
    {
        if (Data.Instance.Sound)
        {
            foreach (var a in ChanelFX)
            {
                a.volume = 0.5f;
            }
        }

        else
            foreach (var a in ChanelFX)
            {
                a.volume = 0f;
            }
    }

    private void ChangeVolumeMusic(GameObject arg0, string arg1)
    {
        if (Data.Instance.Music) PlayMusic();
        else StopMusic();
    }

    private void ChangeVolumeSound(GameObject arg0, string arg1)
    {
        if (Data.Instance.Sound)
        {
            foreach (var a in ChanelFX)
            {
                a.volume = 0.5f;
            }
        }

        else
            foreach (var a in ChanelFX)
            {
                a.volume = 0f;
            }
    }

    void StopMusic()
    {
        ChanelMusic.Stop();
    }

    public void PlayMusic()
    {
        ChanelMusic.clip = MainTrack;
        ChanelMusic.PlayDelayed(1f);
    }

    public void PlayEffect(AudioClip clip)
	{
	    ChanelFX[0].PlayOneShot(clip);
	}


	public void PlayEffect(int indexElement, int val = 0)
	{
        AudionEvent[indexElement].Play(ChanelFX[val]);
	}
	

}

public static class Channel{

    public const int One = 0;
    public const int Two = 1;

}

