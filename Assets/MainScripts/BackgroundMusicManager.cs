using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BackgroundMusicManager : MonoBehaviour
{
    AudioSource music;
    public AudioClip cafeClip, outsideClip;
    //AudioSource cafeMusic, outsideMusic;
    public GameObject border;

    public float taretVolume = 1;

    public AudioSource musicSourceA;
    public AudioSource musicSourceB;
    private bool activeIsA;
    
    private AudioSource ActiveAudioSource => activeIsA ? musicSourceA : musicSourceB;
    private AudioSource FadingAudioSource => activeIsA ? musicSourceB : musicSourceA;
    [NonSerialized] private AudioClip targetAudioClip;

    void Start() {
        border = GameObject.FindWithTag("cafeBorder");
        ChangeSong(cafeClip);
    }

    void Update() {
        if (GameEventsManager.instance.Tools.PlayerInCafe()) {
            ChangeSong(cafeClip);
        }
        else {
            ChangeSong(outsideClip);
        }

        Lerp(ActiveAudioSource, ActiveAudioSource.clip ? taretVolume : 0, 1);
        Lerp(FadingAudioSource, 0, 1);

        if (targetAudioClip && targetAudioClip != ActiveAudioSource.clip)
        {
            // Swap right away if target song just played and haven't faded out yet
            if (targetAudioClip == FadingAudioSource.clip)
            {
                Swap();
            }
            // Swap once fading source is quiet
            else if (FadingAudioSource.volume < 0.01f)
                    Swap();
        }
    }

    public void ChangeSong(AudioClip newClip) => targetAudioClip = newClip;
    
    public void Silence() => targetAudioClip = null;
    
    private void Swap()
    {
        activeIsA = !activeIsA;
        ActiveAudioSource.clip = targetAudioClip;
        ActiveAudioSource.Play();
    }

    private void Lerp(AudioSource src, float targetVolume, float speed)
    {
        float diff = src.volume - targetVolume;

        if (diff == 0)
            return;
        
        float portion = Mathf.Abs(speed * Time.deltaTime / diff);

        src.volume = Mathf.Lerp(src.volume, targetVolume, portion);
    }
    
    private void Awake()
    {
        musicSourceA = CreateSource("cafeMusic");
        musicSourceB = CreateSource("outsideMusic");
    }
    
    private AudioSource CreateSource(string goName)
    {
        var src = new GameObject(goName).AddComponent<AudioSource>();
        src.transform.SetParent(transform);
        src.volume = 0;
        src.loop = true;
        return src;
    }
}
