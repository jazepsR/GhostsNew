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
        this.rank.text = (rank+1).ToString();
        this.playerName.text = playerName.Split('#')[0];
        this.playerTime.text = UIManager.GetTimeString((float)playerTime);

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
