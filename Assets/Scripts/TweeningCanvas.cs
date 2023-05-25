using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class TweeningCanvas : MonoBehaviour
{

    [Header("Shop")]
    public RectTransform ShopButtion;
    public float ShopEndPosition;

    public RectTransform Title;
    public Image BackgroundImage;



    private void Start()
    {
        OnGameStart();
    }



    public void OnGameStart()
    {
        ShopButtion.DOLocalMoveY(ShopEndPosition, 0.5f).SetEase(Ease.OutQuad);
        BackgroundImage.DOFade(0.7f, 1).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        Title.DOShakeScale(0.5f, 0.5f,0,0).SetEase(Ease.Linear);

    }

}
