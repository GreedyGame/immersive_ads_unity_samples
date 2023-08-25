using GoogleMobileAds.Api;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace PubScale.SdkOne.NativeAds.Sample
{
    /// <summary>
    /// Sends the fill to the native companion unit
    /// </summary>
    public class CompanionDisplayer : NativeAdDisplayHandler
    {
        public static event Action<NativeAd,NativeAdHolder> FillCompanion;
        public bool isTwoDScene = true;

        public override void FillAndRegister(NativeAd nativeAd, NativeAdHolder holder, bool registerElement = true)
        {
            Debug.Log("Fill the Companion");

            base.FillAndRegister(nativeAd,holder, false);
            if(isTwoDScene)
            {
            adHeadlineTxt.gameObject.SetActive(false);
            adCallToActionTxt.gameObject.SetActive(false);
            }
            FillCompanion?.Invoke(nativeAd,holder);
        }
    }
}
