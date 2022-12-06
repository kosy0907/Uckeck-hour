using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMusic : MonoBehaviour
{
    GameObject BackgroundMusic;
    AudioSource backmusic;

    void Awake()
    {
        BackgroundMusic = GameObject.Find("BackGroundMusic");
        backmusic = BackgroundMusic.GetComponent<AudioSource>();
        backmusic.Play();
        DontDestroyOnLoad(BackgroundMusic);
    }
}