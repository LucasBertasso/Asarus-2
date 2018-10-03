using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectsController : MonoBehaviour {
    public AudioSource audioSource;
    void PlaySoundEffect(AudioClip audioc)
    {
        audioSource.PlayOneShot(audioc);
    }
}
