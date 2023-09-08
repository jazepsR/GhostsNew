using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int score = 0;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this; 
    }

    public void IncreaseScore()
    {
        score = score + 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
