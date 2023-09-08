using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;
using System;
using UnityEngine.Localization;
using UnityEngine.SocialPlatforms.Impl;

public enum UIMode { map, AR, win, leaderboard}
public class UIManager : MonoBehaviour
{
    [Header("map menu")]
    public GameObject mapMenu;
    [SerializeField] private TMP_Text mapHeading;
    [SerializeField] private CastleController castleController;
    [SerializeField] private LocalizedString[] castleFloorNames;
    [SerializeField] private GameObject scoreText;
    [SerializeField] private LocalizedString scoreTextLocalized;

    [Header("ar menu")]
    public GameObject arMenu; 
    public TMP_Text score;
    public TMP_Text time;

    [Header("leaderboard menu")]
    public GameObject winMenu;

    [Header("leaderboard menu")]
    public GameObject leaderboardMenu;

    [Header("Other")]
    public static UIManager instance;
    [HideInInspector] public UIMode viewerMode = UIMode.map;
    public GameObject ghostParent;
    public GameObject castleParent;
    public GameObject castle;
    public GameObject ghostMovedText;
    [HideInInspector] public float startTime = 0;
    [HideInInspector] public float finishTime = 0;

    private void Awake()
    {
        instance = this;
        viewerMode = UIMode.map;
        ToggleViewMode();
       // scoreTextLocalized= scoreText.GetComponent<LocalizedString>();
    }
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        viewerMode = UIMode.map;
        ToggleViewMode();
    }

    public void SetARMode()
    {
        viewerMode = UIMode.AR;
        ToggleViewMode();
    }

    public void SetMapMode()
    {
        viewerMode = UIMode.map;
        ToggleViewMode();
    }

    public void SetWinMode()
    {
        viewerMode = UIMode.win;
        ToggleViewMode();
    }

    public void SetLeaderboardMode()
    {
        viewerMode = UIMode.leaderboard;
        if(leaderboardMenu.TryGetComponent(out LeaderboardScreen leaderboardScreen))
        {
            leaderboardScreen.GetScores();
        }
        ToggleViewMode();
    }

    public float GetFinalTime()
    {
        if (finishTime == 0)
            return -1;
        else
            return finishTime - startTime;
    }

    public void ToggleViewMode()
    {
        arMenu.SetActive(viewerMode == UIMode.AR);
        mapMenu.SetActive(viewerMode == UIMode.map);
        winMenu.SetActive(viewerMode == UIMode.win);
        leaderboardMenu.SetActive(viewerMode == UIMode.leaderboard);
        castleParent.SetActive(viewerMode == UIMode.map);
        ghostMovedText.SetActive(false);
    }
    public static string GetTimeString(float timeInSeconds)
    {
        return ((timeInSeconds) / 60).ToString("00") + ":" + ((timeInSeconds) % 60).ToString("00");
    }
    // Update is called once per frame
    void Update()
    {
        score.text = scoreTextLocalized.GetLocalizedString()+ GameManager.instance.score;
        //score.text = "Score: " + GameManager.instance.score;
        //time.text = "Time: "+ string.Format("{0:00}", (Time.time - startTime));
        time.text =  GetTimeString(Time.time - startTime);
        mapHeading.text = castleFloorNames[castleController.currentFloor].GetLocalizedString();
       // scoreTextLocalized.RefreshString();
    }
}
