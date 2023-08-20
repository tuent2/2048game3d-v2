using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LoadingController : MonoBehaviour
{
    public static LoadingController THIS;

    public Canvas LoadingMainMenuCanvas;
    public Image Tile;
    public GameObject LoadingSliderObject;
    public Slider LoadingSlider;
    public Tween tween;
    public bool isFristTime = true;
    private void Awake()
    {
        if (THIS == null)
        {
            THIS = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void OnEnable()
    {
        //Debug.Log("123");
        //PlayButton.gameObject.SetActive(false);
        LoadingSliderObject.SetActive(false);
        LoadingSlider.value = 0;
        Tile.gameObject.SetActive(true);
        Tile.transform.localScale = Vector3.zero;
        if (tween != null)
        {
            tween.Kill();
        }
        tween = Tile.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).SetUpdate(true).OnComplete(() =>
        {

            LoadingSliderObject.SetActive(true);

            LoadingSlider.DOValue(1f, 1.5f).OnComplete(() =>
            {
                if (isFristTime == true)
                {
                    isFristTime = false;
                    //LoadingSliderObject.SetActive(false);
                    gameObject.SetActive(false);
                    LoadingMainMenuCanvas.gameObject.SetActive(true);
                }
                else
                {

                    SceneManager.sceneLoaded += OnSceneLoaded;
                    SceneManager.LoadScene(1);
                }
            }); ;
        });





    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 1)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            gameObject.SetActive(false);
        }
    }
}



