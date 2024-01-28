using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip bgm;
    [SerializeField] private AudioSource bgmSource;

    // Start is called before the first frame update
    void Start()
    {
        bgmSource = GetComponent<AudioSource>();
        UpdateVolume();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateVolume()
    {
        bgmSource.volume = GameManager.musicVolume;
    }

    public void Swap_bgm(AudioClip new_bgm)
    {
        bgm = new_bgm;
    }

    public void PlayMusic()
    {
        bgmSource.clip = bgm;
        bgmSource.Play();
    }

    public void PlayNewClip(AudioClip newClip)
    {
        Swap_bgm(newClip);
        PlayMusic();
    }
}
