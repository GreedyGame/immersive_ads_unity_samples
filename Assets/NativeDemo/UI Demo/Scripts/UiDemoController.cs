using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PubScale.SdkOne.NativeAds;
using GoogleMobileAds.Api;
using System;

public class UiDemoController : MonoBehaviour
{
    public GameObject Btn_ShowFullScreenAd;

    public GameObject Btn_AnimatedAd;

    public NativeAdHolder AdHolderFullScreen;

    public NativeAdHolder AdHolderAnimated;


    void Start()
    {
        if (Btn_ShowFullScreenAd)
            Btn_ShowFullScreenAd.SetActive(false);

        if (Btn_AnimatedAd)
            Btn_AnimatedAd.SetActive(false);

        if (AdHolderAnimated != null)
            AdHolderAnimated.Event_AdLoaded += OnAnimatedAdLoaded;

        if (AdHolderFullScreen != null)
            AdHolderFullScreen.Event_AdLoaded += OnFullScreenAdLoaded;

    }

    private void OnAnimatedAdLoaded(object arg1, NativeAdEventArgs args)
    {
        StartCoroutine(BeginAnimatedAd());
    }

    IEnumerator BeginAnimatedAd()
    {
        while (AdHolderAnimated.AdLoaded == false)
            yield return null;

        yield return new WaitForSeconds(1.5f);

        if (Btn_AnimatedAd && AdHolderAnimated.AdLoaded)
        {
            AdHolderAnimated.adDisplay.transform.SetParent(Btn_AnimatedAd.transform);
            Btn_AnimatedAd.SetActive(true);
        }
    }

    private void OnFullScreenAdLoaded(object arg1, NativeAdEventArgs args)
    {
        if (Btn_ShowFullScreenAd)
            Btn_ShowFullScreenAd.SetActive(true);
    }


}
