using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using TMPro;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject textMenu;
    public CanvasGroup textMenuCanvas;
    public AnimationCurve fadeCurve;
    public GameObject languageMenu;
    public TutorialData[] tutorialData;

    [Header("Tutorial screen")]
    public GameObject tutorialMenu;
    public TMP_Text tutorialHeading;
    public Image tutorialImage;
    public TMP_Text tutorialBody;
    private int tutorialID = 0;

    private string privacyPolicyURL = "https://marisputns.wixsite.com/studioperspective/pp";
    void Start()
    {
        textMenu.SetActive(false); 
        tutorialMenu.SetActive(false);
        textMenuCanvas.alpha = 0;
        tutorialID = 0;
    }

    public void OpenPrivacyPolicy()
    {
        Application.OpenURL(privacyPolicyURL);
        Debug.Log("Opening privacy policy");
    }

    async void Awake()
    {
        await UnityServices.InitializeAsync();

        await SignInAnonymously();
    }

    public void OpenTutorial()
    {
        textMenu.SetActive(false);
        tutorialMenu.SetActive(true);
        SetupTutorialScreen(tutorialData[tutorialID]);
    }

    public void NextTutorialScreen()
    {
        tutorialID++;
        if (tutorialID < tutorialData.Length)
        {
            SetupTutorialScreen(tutorialData[tutorialID]);
        }
        else
        {
            StartGame();
        }
    }

    private void SetupTutorialScreen(TutorialData data)
    {
        tutorialHeading.text = data.screenTitle.GetLocalizedString();
        tutorialImage.sprite = data.screenImage;
        tutorialBody.text = data.screenDescription.GetLocalizedString();
    }
    public void LocaleSelected(int index)
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
        textMenu.SetActive(true);
        //languageMenu.SetActive(false);
        StartCoroutine(FadeInTextMenu());
    }

    async Task SignInAnonymously()
    {
        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in as: " + AuthenticationService.Instance.PlayerId);
        };
        AuthenticationService.Instance.SignInFailed += s =>
        {
            // Take some action here...
            Debug.Log(s);
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }
    public void GoToText()
    {
        textMenu.SetActive(true);
        StartCoroutine(FadeInTextMenu());
    }

    private IEnumerator FadeInTextMenu()
    {
        float t = 0;
        while(t<1)
        {
            textMenuCanvas.alpha = fadeCurve.Evaluate(t);
            t += Time.deltaTime;
            yield return null;
        }
        textMenuCanvas.alpha = 1;
    }
    public void StartGame()
    {
        MusicController.instance.soundFX.Stop();
        SceneManager.LoadScene(1);
    }
}

[System.Serializable]
public class TutorialData
{
    public LocalizedString screenTitle;
    public Sprite screenImage;
    public LocalizedString screenDescription;
}
