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
        public static event Action<NativeAd> FillCompanion;
        public override void FillAndRegister(NativeAd nativeAd, bool registerElement = true)
        {
            Debug.Log("Fill the Companion");
            base.FillAndRegister(nativeAd, false);
            FillCompanion?.Invoke(nativeAd);
        }
    }
}
