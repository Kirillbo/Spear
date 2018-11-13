using UnityEngine;
 using System.Collections;
 using Random = UnityEngine.Random;
 
 [CreateAssetMenu(menuName="Audio Events/Simple")]
 public class SimpleAudioEvent : AudioEvent
 {
 	public AudioClip[] clips;
 
 
 	[MinMaxRange(0, 2)]
 	public RangedFloat pitch;
    public float VolumeLevel;

 	public override void Play(AudioSource source)
 	{
 		if (clips.Length == 0) return;
	    source.volume = Data.Instance.Sound ? VolumeLevel : 0;
 		source.clip = clips[Random.Range(0, clips.Length)];
 		source.pitch = Random.Range(pitch.minValue, pitch.maxValue);
 		source.Play();
 	}
 }