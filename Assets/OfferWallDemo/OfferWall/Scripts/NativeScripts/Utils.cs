
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
    }
}