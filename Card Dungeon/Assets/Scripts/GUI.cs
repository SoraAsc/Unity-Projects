using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GUI : MonoBehaviour
{
    [SerializeField]
    AudioClip[] allAudios;
    [SerializeField]
    int i=0;
    public AudioSource audioSource;
    public TextMeshProUGUI musicText;
    public void LeftMusicSwitch()
    {
        if (i > 0) i--;
        else i = allAudios.Length - 1;

        audioSource.clip = allAudios[i];
        musicText.text = audioSource.clip.name;
        audioSource.Play();
    }

    public void RightMusicSwitch()
    {
        if (i < allAudios.Length - 1) i++;
        else i = 0;

        audioSource.clip = allAudios[i];
        musicText.text = audioSource.clip.name;
        audioSource.Play();
    }

    public void PlayMusic()
    {
        if(audioSource.isPlaying) audioSource.Pause();
        else audioSource.UnPause();
    }
}
