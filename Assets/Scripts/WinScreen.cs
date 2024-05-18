using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Leaderboards;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class WinScreen : MonoBehaviour
{
    const string LeaderboardId = "Jaunpils_Times";
    private string username = "";
    private string usernameKey = "usernameSaveKey";
    public TMP_InputField nameEntryField;
    public Unity.Services.Leaderboards.Models.LeaderboardEntry scoreResponse = null;
    public static WinScreen instance;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        username =PlayerPrefs.GetString(usernameKey, "");
        nameEntryField.text = username;
        //nameEntryField.SetActive(username != "");
    }

    public async void SetUsername(string nameToSet)
    {
        username = nameToSet;
        PlayerPrefs.SetString(usernameKey, nameToSet);
        await UpdateUsername();
    }

    public async void AddToLeaderboardButton()
    {
        await AddScore();
        UIManager.instance.SetLeaderboardMode();
    }
    public async Task UpdateUsername()
    {
       // if (username != "")
        {
            var scoreResponse = await AuthenticationService.Instance.UpdatePlayerNameAsync(username);
            Debug.Log(JsonConvert.SerializeObject(scoreResponse));
        }

    }
    public async Task AddScore()
    {
        float score = UIManager.instance.GetFinalTime();
        if (score != -1)
        {
            scoreResponse = await LeaderboardsService.Instance.AddPlayerScoreAsync(LeaderboardId, score);
            Debug.Log(JsonConvert.SerializeObject(scoreResponse));
        }
    }
}
