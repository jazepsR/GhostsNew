using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Leaderboards;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class LeaderboardScreen : MonoBehaviour
{
    const string LeaderboardId = "Jaunpils_Times";
    [SerializeField] private LeaderboardEntry entry;
    [SerializeField] private Transform leaderboardEntryParent;
    // Start is called before the first frame update
    void Start()
    {
        GetScores();
    }

    // Update is called once per frame
    public async void GetScores()
    {
        Unity.Services.Leaderboards.Models.LeaderboardScoresPage scoresResponse =
            await LeaderboardsService.Instance.GetScoresAsync(LeaderboardId, new GetScoresOptions { Offset = 0, Limit = 20 });
        Debug.Log(JsonConvert.SerializeObject(scoresResponse));
        SetupLeaderboard(scoresResponse);
        
    }

    private void SetupLeaderboard(Unity.Services.Leaderboards.Models.LeaderboardScoresPage scoresResponse)
    {
        foreach(Transform ts in leaderboardEntryParent.transform)
        {
            Destroy(ts.gameObject);
        }
        for(int i =0; i<scoresResponse.Results.Count;i++)
        {
            LeaderboardEntry obj = Instantiate(entry, leaderboardEntryParent);
            obj.SetupLeaderboardEntry(scoresResponse.Results[i].Rank, scoresResponse.Results[i].PlayerName, scoresResponse.Results[i].Score);
        }

        /*for (int i = 0; i < 19; i++)
        {
            LeaderboardEntry obj = Instantiate(entry, leaderboardEntryParent);
            obj.SetupLeaderboardEntry(i, "buttlicker", 66);
        }*/
        if (WinScreen.instance.scoreResponse != null && WinScreen.instance.scoreResponse.Rank > scoresResponse.Results.Count)
        {
            LeaderboardEntry player = Instantiate(entry, leaderboardEntryParent);
            player.SetupLeaderboardEntry(WinScreen.instance.scoreResponse.Rank, WinScreen.instance.scoreResponse.PlayerName, WinScreen.instance.scoreResponse.Score);
        }
    }
}
