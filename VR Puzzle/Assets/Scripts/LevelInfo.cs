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
    }
}