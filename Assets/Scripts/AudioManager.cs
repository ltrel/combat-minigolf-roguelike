using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private List<AudioClip> audioClips = new List<AudioClip>();
    [SerializeField] private AudioSource effectSource;
    [SerializeField] private AudioSource musicSource;

    public static AudioManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    
    public void PlayEffect(string clipName)
    {
        AudioClip clip = audioClips.Find(x => x.name == clipName);
        if (clip != null)
        {
            effectSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogError("Audio clip does not exist");
        }
    }
}
