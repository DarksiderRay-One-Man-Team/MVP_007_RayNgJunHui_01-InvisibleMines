using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    [SerializeField] private AudioSource bgmAudioSource;
    //[SerializeField] private bool playOnStart = true;
    [SerializeField] private float fadeInDuration = 1f;
    [SerializeField] private float fadeOutDuration = 1f;

    private float originalVolume;
    
    void Awake()
    {
        originalVolume = bgmAudioSource.volume;
    }
    
    // void Start()
    // {
    //     if (playOnStart)
    //         bgmAudioSource.Play();
    // }
    
    public void FadeInBGM()
    {
        bgmAudioSource.Play();
        bgmAudioSource.DOFade(originalVolume, fadeInDuration);
    }

    public void FadeOutBGM()
    {
        //bgmAudioSource.DOFade(0f, fadeOutDuration).OnComplete(() => bgmAudioSource.Stop());
        bgmAudioSource.DOFade(0.2f, fadeOutDuration);
    }
}
