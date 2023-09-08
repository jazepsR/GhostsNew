using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioSource mapAmbience;
    public AudioSource arAmbience;
    public AudioSource soundFX;
    private float lerpTime = 1f;
    [HideInInspector] public bool isMap = true;
    public static MusicController instance;
    public List<AudioClip> enemyHitSounds;
    void Awake()
    {        
        GameObject[] objs = GameObject.FindGameObjectsWithTag("music");
        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void ToggleAmbience(bool isMap)
    {
        this.isMap = isMap;
    }

    public void PlaySound(AudioClip clip)
    {
        soundFX.PlayOneShot(clip);
    }
    public void PlayHitSound()
    {
        soundFX.PlayOneShot(enemyHitSounds[Random.Range(0,enemyHitSounds.Count)]);
    }
    void Update()
    {
        mapAmbience.volume = Mathf.Lerp(mapAmbience.volume, isMap ? 0.3f : 0, Time.deltaTime * lerpTime);
        arAmbience.volume = Mathf.Lerp(mapAmbience.volume, isMap ? 0 : 0.3f, Time.deltaTime * lerpTime);
    }
}
