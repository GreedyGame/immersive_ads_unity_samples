using UnityEngine;

namespace PubScale.SdkOne.NativeAds
{
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
        private void NativeAdManager_LogHandler(string eventName, PubScaleLogData holderRef)
        {
            //add all values to a string and log it
            string log = "";
            foreach (var item in holderRef.EventData)
            {
                log += item.k + "=" + item.v + "\n";
            }
            Debug.Log(eventName + "\n" + log);
        }
    }
}
