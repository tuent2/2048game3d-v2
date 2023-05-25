using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using HmsPlugin;
public class RepondGem : MonoBehaviour
{
    public GameObject RespondGemPanel;
    public TextMeshProUGUI RespondGemText;
    // Start is called before the first frame update
    public float countdownTime = 3.0f;

    private float currentTime = 0.0f;

    void Start()
    {
        Debug.Log("kkkkkkkk1");
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
            RespondGemPanel.SetActive(false);
        }

        RespondGemText.text = "Not enough gems! \n Please click gems to get more";
    }
}

