using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
public class FeedBackManager : MonoBehaviour
{
    public static FeedBackManager Instance { set; get; }

    public MMFeedbacks BoxMerge;
    public MMFeedbacks LandBox;
    public MMFeedbacks CoinFeedback;
    public MMFeedbacks ScoreFeedback;

    private void Awake()
    {
        Instance = this;
    }
}
