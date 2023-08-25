using PubScale.SdkOne.NativeAds;
using System;
using System.Collections.Generic;
using UnityEngine;

public class NativeAdClickDisabler : MonoBehaviour
{
     private List<NativeAdHolder> nativeAds=new List<NativeAdHolder>();
    [SerializeField] private List<NativeAdHolder> blackList=new List<NativeAdHolder>();
    public static event Action<Action<bool>> ShowHideAd;
    public static event Action<bool> customAdShowHide;

    private void Awake()
    {
        NativeAdHolder.Event_OnNativeAdActive += NativeAdHolder_Event_OnNativeAdActive;
    }
    private void OnDestroy()
    {
        nativeAds.Clear();
        NativeAdHolder.Event_OnNativeAdActive -= NativeAdHolder_Event_OnNativeAdActive;
    }
    private void NativeAdHolder_Event_OnNativeAdActive(NativeAdHolder obj)
    {
        if (blackList.Contains(obj) && blackList.Count > 0)
            return;
        nativeAds.Add(obj);
    }

    private void Start()
    {
        ShowHideAd?.Invoke(ShowHideAllAds);
    }
    private void ShowHideAllAds(bool obj)
    {
        customAdShowHide?.Invoke(obj);
        foreach (var item in nativeAds)
        {
            item.DisableAd(obj);
        }
    }
}
