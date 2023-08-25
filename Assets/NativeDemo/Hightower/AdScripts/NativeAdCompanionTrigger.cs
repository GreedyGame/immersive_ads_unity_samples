using System;
using UnityEngine;

namespace PubScale.SdkOne.NativeAds.Hightower
{
    /// <summary>
    /// Activate/Deactivate the UI Companion unit when player is nearby
    /// </summary>
    public class NativeAdCompanionTrigger : MonoBehaviour
    {
        [SerializeField] private NativeAdHolder nativeAd;
        public static event Action<bool> ShowAd;
        private bool adLoaded;

        private void Awake()
        {
            nativeAd.Event_AdLoaded += NativeAd_Event_AdLoaded;     //Subscribe to native ad loaded event
#if UNITY_EDITOR                                              
            adLoaded = true;
#endif
        }
        private void OnDestroy()
        {   
            nativeAd.Event_AdLoaded -= NativeAd_Event_AdLoaded;    //Unsubscribe to native ad loaded event
        }
        private void NativeAd_Event_AdLoaded(object arg1, GoogleMobileAds.Api.NativeAdEventArgs arg2)
        {
            adLoaded = true;
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!adLoaded)                                          //Don't trigger ad if it is not loaded
                return;
            if (collision.CompareTag("Player"))
            {
                ShowAd?.Invoke(true);
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                ShowAd?.Invoke(false);
            }
        }
    }
}
