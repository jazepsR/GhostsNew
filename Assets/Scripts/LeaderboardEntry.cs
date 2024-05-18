using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LeaderboardEntry : MonoBehaviour
{
    [SerializeField] private TMP_Text rank;
    [SerializeField] private TMP_Text playerName;
    [SerializeField] private TMP_Text playerTime;


    public void SetupLeaderboardEntry(int rank,  string playerName, double playerTime)
    {
        string playerNameString = playerName.Split('#')[0];
        if (playerNameString == WinScreen.instance.username)
        {
            this.playerName.text = "<color=#ffffff>"+playerNameString+ "</color>";
            this.playerTime.text = "<color=#ffffff>"+UIManager.GetTimeString((float)playerTime)+ "</color>";
            this.rank.text = "<color=#ffffff>" + (rank + 1).ToString()+ "</color>";
        }
        else
        {
            this.playerName.text = playerNameString;
            this.playerTime.text = UIManager.GetTimeString((float)playerTime);
            this.rank.text = (rank + 1).ToString();
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
