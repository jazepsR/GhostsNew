using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    public AudioClip clip;
    // Start is called before the first frame update
    public void Click()
    {
        if(MusicController.instance != null)
            MusicController.instance.PlaySound(clip);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
