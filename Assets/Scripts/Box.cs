using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;
public class Box : MonoBehaviour
{

    public Rigidbody rb;

    public TextMeshPro[] Texts;

    public int Value = 12;

    public MeshRenderer _Mesh;

    public Color[] _color;

    int ColorValue = 0;

    public int[] _NumberList;

    public static int MaxLevel = 4;

    int PlayAbleMaxValue;

    bool isGen;

    static bool isMerge;

    void Start()
    {
        isMerge = true;

        Texts = GetComponentsInChildren<TextMeshPro>();

        int SelectedValue = Random.Range(0, MaxLevel - 2);

        Value = _NumberList[SelectedValue];

        _Mesh.material.color = _color[SelectedValue];

        ColorValue = SelectedValue;

        UpdateText();

        PlayAbleMaxValue = MaxLevel;

        isGen = true;
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
        if (collision.gameObject.tag == "Box")
        {
            MergeBox(collision.gameObject);
        }

    }



    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Limit" && gameObject.layer == 6)

        {
            Debug.Log("Limit or layer 6");
            if (GetComponent<Rigidbody>().velocity.y > -0.001 && GetComponent<Rigidbody>().velocity.y < 0.001)
            {
                MaxLevel = 4;

                UIManager.Instance.Gameover();
            }
        }
    }


    void MergeBox(GameObject _object)
    {


        if (_object.GetComponent<Box>().Value == Value && isMerge)
        {

            isMerge = false;

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

            Value += _object.GetComponent<Box>().Value;

            UpdateText();

            Destroy(_object);
            _Mesh.material.DOColor(_color[ColorValue], 0.5f);
            transform.DOShakeScale(0.6f, 0.4f, 5, 20).SetEase(Ease.Linear).OnComplete(() =>
            {
                transform.DOScale(new Vector3(0.78f, 0.78f, 0.78f), 1f).SetEase(Ease.Linear).OnComplete(() =>
                {


                });
            });

        }
        else
        {
            gameObject.tag = "Box";
        }


    }

    public void ChangeBox()
    {

        if (ColorValue <= 11)
            ColorValue++;

        Value = Value * 2;

        UpdateText();


        _Mesh.material.DOColor(_color[ColorValue], 0.5f);
        transform.DOShakeScale(0.6f, 0.4f, 5, 20).SetEase(Ease.Linear).OnComplete(() =>
        {
            transform.DOScale(new Vector3(0.78f, 0.78f, 0.78f), 1f).SetEase(Ease.Linear).OnComplete(() =>
            {


            });
        });
    }

    void UpdateText()
    {
        for (int i = 0; i < Texts.Length; i++)
        {
            Texts[i].text = Value.ToString();
        }
    }

    static IEnumerator TrueMerge()
    {
        yield return new WaitForSeconds(0.2f);
        isMerge = true;
    }


}
