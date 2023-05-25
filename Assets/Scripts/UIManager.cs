using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { set; get; }

    public GameObject GamoverPanel;
    public GameObject MenuPanel;
    public GameObject InAppPanel;
    public GameObject NoADSIAP;

    [Space]
    public TextMeshProUGUI CoinText;
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI HighScoreText;


    public GameObject RespondAds;
    public GameObject RespondRewardAds;
    public GameObject RespondGem;

    private void Awake()
    {
        Instance = this;

        // if (PlayerPrefs.GetInt("Purchase", 0) == 1)
        // {
        //     NoADSIAP.SetActive(false);
        // }
        // else
        // {
        //     NoADSIAP.SetActive(true);
        // }
        NoADSIAP.SetActive(false);
    }

    public void Gameover()
    {
        if (!GameManager.isGamoever)
        {
            if (PlayerPrefs.GetInt("Purchase", 0) == 0)
            {
                // AdManager.Instance.ShowInterstitial();
            }
            GameManager.isGamoever = true;
            GamoverPanel.SetActive(true);
        }

    }

    public void RestartButton()
    {
        SceneManager.LoadScene(0);
    }

    public void UpdateCoinText(int Value)
    {
        PlayerPrefs.SetInt("Coin", Value);
        CoinText.text = Value.ToString();
    }
    public void UpdateScore(int Score)
    {
        ScoreText.text = Score.ToString();
    }

    public void UpdateHighScore(int Highscore)
    {
        HighScoreText.text = Highscore.ToString();
    }
    public void StartGame()
    {
        if (!GameManager.isGamestart)
        {
            GameManager.isGamestart = true;
            MenuPanel.SetActive(false);
        }
    }

    public void OpenInAppPanel()
    {
        GameManager.isGameScreen = false;
        InAppPanel.SetActive(true);
        // StartCoroutine(OnOpenInAppPanel());
    }

    // IEnumerator OnOpenInAppPanel()
    // {
    //     yield return new WaitForSeconds(0f);

    //     InAppPanel.SetActive(true);
    // }


    public void CloseInAppPanel()
    {
        InAppPanel.SetActive(false);
        StartCoroutine(OnCloseInAppPanel());
    }

    IEnumerator OnCloseInAppPanel()
    {
        yield return new WaitForSeconds(1f);
        GameManager.isGameScreen = true;
    }

    public void buyGemIAP(string productID)
    {
        // HMSIAPManager.Instance.PurchaseProduct(productID);
        AdManager.Instance.buyIAPChecked(productID);
        InAppPanel.SetActive(false);
    }

    public void isUsingHammerItem()
    {
        // GameManager.isUsingItem = true;
        // AdManager.Instance.ShowRewardBasedVideo();

        if (GameManager.isUsingItem != true)
        {
            if (GameManager.Coin >= 200)
            {
                // PlayerPrefs.SetInt("Coin", GameManager.Coin - 200);
                GameManager.Coin = GameManager.Coin - 200;
                UpdateCoinText(GameManager.Coin);
                GameManager.isUsingItem = true;
            }
            else
            {
                GameManager.isGameScreen = false;
                RespondGem.gameObject.SetActive(true);
                Debug.Log("kkkkkkkkkkkkkkkkk" + RespondGem);
                StartCoroutine(OnCloseInAppPanel());
            }
        }
    }

    public void getMoreCoin()
    {
        GameManager.isGameScreen = false;
        AdManager.Instance.ShowRewardBasedVideo();
        StartCoroutine(wattingset());
    }

    IEnumerator wattingset()
    {
        yield return new WaitForSeconds(0.5f);
        GameManager.isGameScreen = true;
    }

}
