
using System;
using UnityEngine;

namespace PubScale.Common
{
    public class Utils
    {
        #region Fully-qualified class names

        public const string UnityActivityClassName = "com.unity3d.player.UnityPlayer";
        public const string OfferWallListnerClassName = "com.pubscale.offerwallplugin.OfferWallListener";
        public const string OfferWallPluginName = "com.pubscale.offerwallplugin.OfferWallPlugin";
        public const string OfferWallClassName = "com.pubscale.offerwallplugin.OfferWallUnity";
        #endregion



        public static AndroidJavaObject GetUnityActivity()
        {
            AndroidJavaClass playerClass = new AndroidJavaClass(Utils.UnityActivityClassName);
            AndroidJavaObject activity = playerClass.GetStatic<AndroidJavaObject>("currentActivity");
            return activity;
        }

        public static string GetStringFromEnum(AndroidJavaObject initErrorJavaObject)
        {
            return initErrorJavaObject.Get<string>("name");
        }
        public static string GetAndroidAdvertiserId()
        {
            string advertisingID = "";
            try
            {
                AndroidJavaClass up = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
                AndroidJavaObject currentActivity = up.GetStatic<AndroidJavaObject> ("currentActivity");
                AndroidJavaClass client = new AndroidJavaClass ("com.google.android.gms.ads.identifier.AdvertisingIdClient");
                AndroidJavaObject adInfo = client.CallStatic<AndroidJavaObject> ("getAdvertisingIdInfo", currentActivity);
        
                advertisingID = adInfo.Call<string> ("getId").ToString();  
            }
            catch (Exception)
            {
                
            }

            Debug.Log("Advertising ID" + advertisingID);
            return advertisingID;
        }
    }
}