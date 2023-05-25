using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;
public class BoomBox : MonoBehaviour
{
    public Rigidbody rb;

    public TextMeshPro[] Texts;

    public string Value = "Boom";

    public MeshRenderer _Mesh;

    public Color _color;

    int ColorValue = 0;

    public static int MaxLevel = 4;

    int PlayAbleMaxValue;

    bool isGen;

    static bool isMerge;
    // Start is called before the first frame update
    void Start()
    {
        isMerge = true;

        Texts = GetComponentsInChildren<TextMeshPro>();

        UpdateText();

        isGen = true;

        _Mesh.material.color = _color;
    }


    private void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "ignore")
        {

            if (isGen)
            {
                GameManager.isMove = true;
                FeedBackManager.Instance.LandBox.PlayFeedbacks();
                gameObject.layer = 6;
                GameManager.Instance.OnGenBox();
                isGen = false;
            }
        }

    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Box" || collision.gameObject.tag == "SpBox")
        {
            MergeBox(collision.gameObject);
        }
        else
        {
            if (collision.gameObject.tag == "Bottom")
            {
                GameManager.Instance.GenSplashParticle(transform.position);
                Destroy(gameObject);
            }

        }
    }



    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Limit" && gameObject.layer == 6)
        {
            if (GetComponent<Rigidbody>().velocity.y > -0.001 && GetComponent<Rigidbody>().velocity.y < 0.001)
            {
                MaxLevel = 4;

                UIManager.Instance.Gameover();
            }
        }
    }


    void MergeBox(GameObject _object)
    {




        GameManager.Instance.GenSplashParticle(transform.position);

        GameManager.Coin++;

        GameManager.Score++;

        if (GameManager.Highscore < GameManager.Score)
        {
            PlayerPrefs.SetInt("Highscore", GameManager.Score);
            UIManager.Instance.UpdateHighScore(GameManager.Score);
        }
        FeedBackManager.Instance.ScoreFeedback.PlayFeedbacks();

        UIManager.Instance.UpdateScore(GameManager.Score);


        UIManager.Instance.UpdateCoinText(GameManager.Coin);

        GameManager.Instance.GenPrefabCoin(transform.position);


        StartCoroutine(TrueMerge());
        FeedBackManager.Instance.BoxMerge.PlayFeedbacks();

        if (PlayAbleMaxValue > MaxLevel)
        {

            MaxLevel += 1;

            if (PlayerPrefs.GetInt("Purchase", 0) == 0)
            {

                //  AdManager.Instance.ShowInterstitial();

            }

        }
        else
        {
            if (PlayAbleMaxValue < 9)
            {
                PlayAbleMaxValue++;

            }
        }

        Debug.Log("Max Level " + MaxLevel + "Playable Level " + PlayAbleMaxValue);

        if (ColorValue <= 11)
            ColorValue++;

        Destroy(gameObject);
        // Value += _object.GetComponent<Box>().Value;

        // // UpdateText();

        Destroy(_object);
        // _Mesh.material.DOColor(_color, 0.5f);
        // transform.DOShakeScale(0.6f, 0.4f, 5, 20).SetEase(Ease.Linear).OnComplete(() =>
        // {
        //     transform.DOScale(new Vector3(0.78f, 0.78f, 0.78f), 1f).SetEase(Ease.Linear).OnComplete(() =>
        //     {


        //     });
        // });



    }

    void UpdateText()
    {
        for (int i = 0; i < Texts.Length; i++)
        {
            Texts[i].text = Value + "";
        }
    }

    static IEnumerator TrueMerge()
    {
        yield return new WaitForSeconds(0.2f);
        isMerge = true;
    }
}
