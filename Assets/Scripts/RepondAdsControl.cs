using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using HmsPlugin;
public class RepondAdsControl : MonoBehaviour
{
    public GameObject RespondPanel;
    public TextMeshProUGUI RespondText;
    // Start is called before the first frame update
    public float countdownTime = 3.0f;

    private float currentTime = 0.0f;
    void Start()
    {
        currentTime = countdownTime;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime -= Time.deltaTime;

        if (currentTime <= 0.0f)
        {
            // RespondText.text = "Ads will appear in: ";
            // return;
            currentTime = countdownTime;
            RespondPanel.SetActive(false);
            if (PlayerPrefs.GetInt("Purchase", 0) == 0)
            {
                //AdManager.Instance.ShowInterstitial();
            }
        }
        RespondText.text = "Ads will appear in: " + Mathf.FloorToInt(currentTime).ToString();
    }
}
