using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
//using Unity.Advertisement.IosSupport;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { set; get; }
    public static bool isGamoever;
    public static bool isGamestart;
    public static bool isMove;
    public static bool isGameScreen = true;
    public static bool isUsingItem = false;

    public GameObject GenBoxPrefab;
    public GameObject GenBoomBoxPrefab;
    public GameObject GenMergeBoxPrefab;
    public Transform GenPosition;

    Rigidbody _BoxRB;
    Collider _BoxCollider;

    public float Speed;
    GameObject _Box;

    [Space]
    public GameObject SplashParticle;

    [Space]
    public GameObject PrefabCoin;
    public GameObject ScoreParticle;
    public Transform UICanvas;
    public RectTransform TargetCoinObject;
    public RectTransform TargetScoreObject;
    public GameObject AdBreakPane;

    public static int Coin;
    public static int Score;
    public static int Highscore;
    public int countGen;
    void Start()
    {
        Instance = this;
        isMove = true;
        Score = 0;
        //playerore
        Coin = PlayerPrefs.GetInt("Coin", 0);
        UIManager.Instance.UpdateCoinText(Coin);
        Highscore = PlayerPrefs.GetInt("Highscore", 0);
        UIManager.Instance.UpdateHighScore(Highscore);
        OnGenBox();
        isGamestart = false;
        isGamoever = false;
        isUsingItem = false;
        //if(ATTrackingStatusBinding.GetAuthorizationTrackingStatus() == ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED)
        //{
        //    ATTrackingStatusBinding.RequestAuthorizationTracking();
        //}
    }

    private void Update()
    {
        if (Application.targetFrameRate < 60)
            Application.targetFrameRate = 60;

        //_BoxRB.transform.position = new Vector3(Mathf.Clamp(_BoxRB.transform.position.x, -1.4f, 1.4f), _BoxRB.transform.position.y, _BoxRB.transform.position.z);

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved && !isGamoever && isMove && isGameScreen && isUsingItem == false)
            {
                UIManager.Instance.StartGame();

                if (isGamestart)
                {
                    Vector3 DeltaPosition = touch.deltaPosition;

                    Vector3 TouchValue = new Vector3((_Box.transform.position.x - DeltaPosition.x) * Time.deltaTime * Speed, _Box.transform.position.y, _Box.transform.position.z);

                    Vector3 ClmapValue = _Box.transform.position;

                    ClmapValue.x += TouchValue.x;

                    ClmapValue.x = Mathf.Clamp(ClmapValue.x, -1.9f, 1.029f);

                    _Box.transform.position = ClmapValue;
                }



            }
            else if (touch.phase == TouchPhase.Ended && !isGamoever && isGamestart && isGameScreen && isUsingItem == false)
            {
                isMove = false;
                _BoxRB.isKinematic = false;
                _BoxCollider.isTrigger = false;
                _BoxRB.AddForce(-Vector3.up * 600, ForceMode.Impulse);
                _BoxRB.transform.GetChild(0).gameObject.SetActive(false);
            }
            else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && isUsingItem == true && isGameScreen)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject.tag == "Box" && hit.rigidbody.isKinematic == false)
                    {
                        Destroy(hit.collider.gameObject);
                        // Người dùng chạm vào game object của bạn
                        // Thực hiện các hành động tương ứng tại đây

                        StartCoroutine(DelaySetUsingItem());

                    }
                }
            }
        }


        IEnumerator DelaySetUsingItem()
        {
            yield return new WaitForSeconds(0.1f);
            isUsingItem = false;
        }
        //if(Input.anyKeyDown)
        //  {
        //     
        //  }
    }

    public void OnGenBox()
    {
        int BoxOption = SelectRanDomBox();
        if (BoxOption == 2)
        {
            _Box = Instantiate(GenBoomBoxPrefab, GenPosition.position, Quaternion.identity);
            _Box.layer = 7;
            _Box.transform.DOScale(new Vector3(0.78f, 0.78f, 0.78f), 0.5f).SetEase(Ease.OutCubic);
            _BoxRB = _Box.GetComponent<Rigidbody>();
            _BoxCollider = _Box.GetComponent<Collider>();
            _BoxCollider.isTrigger = true;
            _BoxRB.isKinematic = true;
        }
        else if (BoxOption == 1)
        {
            _Box = Instantiate(GenBoxPrefab, GenPosition.position, Quaternion.identity);
            _Box.layer = 7;
            _Box.transform.DOScale(new Vector3(0.78f, 0.78f, 0.78f), 0.5f).SetEase(Ease.OutCubic);
            _BoxRB = _Box.GetComponent<Rigidbody>();
            _BoxCollider = _Box.GetComponent<Collider>();
            _BoxCollider.isTrigger = true;
            _BoxRB.isKinematic = true;
        }
        else if (BoxOption == 3)
        {
            _Box = Instantiate(GenMergeBoxPrefab, GenPosition.position, Quaternion.identity);
            _Box.layer = 7;
            _Box.transform.DOScale(new Vector3(0.78f, 0.78f, 0.78f), 0.5f).SetEase(Ease.OutCubic);
            _BoxRB = _Box.GetComponent<Rigidbody>();
            _BoxCollider = _Box.GetComponent<Collider>();
            _BoxCollider.isTrigger = true;
            _BoxRB.isKinematic = true;
        }



    }

    private int SelectRanDomBox()
    {

        int[] choices = { 1, 2, 3 }; // Danh sách các trường hợp để chọn từ
        int[] ratios = { 93, 5, 2 }; // Tỉ lệ tương ứng

        int totalRatio = 0;
        foreach (int ratio in ratios)
        {
            totalRatio += ratio;
        }
        System.Random random;
        random = new System.Random();
        int rand = random.Next(1, totalRatio + 1); // Lấy một số ngẫu nhiên từ 1 đến tổng tỷ lệ

        int cumulativeRatio = 0;
        for (int i = 0; i < ratios.Length; i++)
        {
            cumulativeRatio += ratios[i];
            if (rand <= cumulativeRatio)
            {
                int randomCase = choices[i];
                return randomCase;
            }
        }
        return choices[0];
    }


    public void GenSplashParticle(Vector3 Position)
    {
        Instantiate(SplashParticle, Position, Quaternion.identity);
    }

    public void GenPrefabCoin(Vector3 Position)
    {
        GameObject _coin = Instantiate(PrefabCoin, UICanvas);
        GameObject _ScoreParticle = Instantiate(ScoreParticle, UICanvas);
        _ScoreParticle.transform.position = Camera.main.WorldToScreenPoint(Position);
        _coin.transform.position = Camera.main.WorldToScreenPoint(Position);

    }



    public void GenAdBreak()
    {
        Instantiate(AdBreakPane, UICanvas);
    }
}
