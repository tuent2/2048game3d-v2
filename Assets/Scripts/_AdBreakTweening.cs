using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class _AdBreakTweening : MonoBehaviour
{
    public RectTransform LoadingIcon;
    public Image BackgroundImage;

    private void Start()
    {
        BackgroundImage.DOFade(1, 1f).SetEase(Ease.Linear).OnComplete(() =>
        {
            if (PlayerPrefs.GetInt("Purchase", 0) == 0)
            {
                //AdManager.Instance.ShowInterstitial();
            }
            Destroy(gameObject, 2);

        }


        );
        LoadingIcon.DOLocalRotate(new Vector3(0, 0, 90), 0.2f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);
    }
}
