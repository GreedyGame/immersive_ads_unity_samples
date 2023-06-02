using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PubScale.SdkOne.NativeAds;
public class LogTest : MonoBehaviour
{
    void Start()
    {
        NativeAdManager.LogHandler += NativeAdManager_LogHandler;

    }
    private void OnDestroy()
    {
        NativeAdManager.LogHandler -= NativeAdManager_LogHandler;

    }
    private void NativeAdManager_LogHandler(string obj)
    {
        Debug.Log(obj);
    }
}
