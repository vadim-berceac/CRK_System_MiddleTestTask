using UnityEngine;

[System.Serializable]
public class SoundSettings
{
    [field: SerializeField] public AudioClip Clip { get; set; }
    [field: SerializeField] public float Volume { get; set; }

    public void Play(AudioSource audioSource)
    {
        if (Clip!= null)
        {
            audioSource.PlayOneShot(Clip, Volume);
        }
    }
}
