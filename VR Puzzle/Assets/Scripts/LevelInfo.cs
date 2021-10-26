using System.Collections;
using System.Collections.Generic;
using OVR;
using UnityEngine;

public class LevelInfo : MonoBehaviour
{
    [SerializeField] private SoundFXRef levelMusic;

    void Start()
    {
        MusicManager.instance.Play(levelMusic.GetClip());
        // Debug.Log(levelMusic.name);
        // Debug.Log(AudioManager.IsSoundPlaying("layton"));
        // Debug.Log(levelMusic.soundFXName);
        // if (!AudioManager.IsSoundPlaying(levelMusic.name))
        // {
        //     AudioManager.StopAllSounds(false);
        // }
        // AudioManager.StopAllSounds(false);
        // levelMusic.PlaySound();
    }
}