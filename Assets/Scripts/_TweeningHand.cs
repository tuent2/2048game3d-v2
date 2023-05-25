using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class _TweeningHand : MonoBehaviour
{

    public float EndValue;
    void Start()
    {

        transform.DOLocalMoveX(EndValue, 1).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);

    }
}
