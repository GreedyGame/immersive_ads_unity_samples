using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PubScale.SdkOne.NativeAds;
using PubScale.SdkOne;

public class LogTest : MonoBehaviour
{
    void Start()
    {
        PubScaleManager.LogHandler += NativeAdManager_LogHandler;

    }
    private void OnDestroy()
    {
        PubScaleManager.LogHandler -= NativeAdManager_LogHandler;

    }
    private void NativeAdManager_LogHandler(string eventName,NativeAdHolder holderRef)
    {
        Debug.Log(eventName);
    }
}
