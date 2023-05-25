using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InAppPurchase : MonoBehaviour
{
    public static InAppPurchase Intance { set; get; }
    private void Awake()
    {
        Intance = this;
    }
    public void OnPurchaseComplate()
    {
        PlayerPrefs.SetInt("Purchase", 1);
        UIManager.Instance.NoADSIAP.gameObject.SetActive(false);
        UIManager.Instance.CloseInAppPanel();
        AdManager.Instance.HideBanner();
        AdManager.Instance.DestroyBanner();
    }

    public void OnPurchaseFailed()
    {
        PlayerPrefs.SetInt("Purchase", 0);
    }


}
