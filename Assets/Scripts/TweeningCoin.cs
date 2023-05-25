using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class TweeningCoin : MonoBehaviour
{
    public bool ScoreParticle;

    private void Start()
    {

        if(ScoreParticle)
        {
            transform.GetComponent<Image>().DOFade(0, 0.5f);
            transform.DOMove(GameManager.Instance.TargetScoreObject.position, 1.4f).SetEase(Ease.OutQuad).OnComplete(() => {
                Destroy(this.gameObject);
            });
        }
        else
        {
            transform.DOMove(GameManager.Instance.TargetCoinObject.position, 0.5f).SetEase(Ease.OutQuad).OnComplete(() => {
                FeedBackManager.Instance.CoinFeedback.PlayFeedbacks();
                Destroy(this.gameObject);
            });
        }
       
    }
}
