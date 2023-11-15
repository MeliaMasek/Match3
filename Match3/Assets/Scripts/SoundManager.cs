using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource matchingSound;
    public AudioSource backgroundMusic;
    
    public void PlayMatchingSound()
    {
        matchingSound.Play();    
    }

    public void Update()
    {
        backgroundMusic.Play();
    }
}
