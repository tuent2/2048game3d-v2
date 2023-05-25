using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using HmsPlugin;
public class RepondRewardAds : MonoBehaviour
{
    public GameObject RespondRwaPanel;
    public TextMeshProUGUI RespondRwaText;
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
            currentTime = countdownTime;
            Debug.Log("kkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkk" + currentTime);
            RespondRwaPanel.SetActive(false);
        }

        RespondRwaText.text = "Ads is not ready! \n Please wait a few minutes";
    }
}
