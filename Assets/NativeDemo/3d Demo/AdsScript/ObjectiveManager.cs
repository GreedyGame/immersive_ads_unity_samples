using System;
using TMPro;
using UnityEngine;
namespace PubScale.SdkOne.NativeAds.Sample
{
    public class ObjectiveManager : MonoBehaviour
    {
        [SerializeField] private NativeAdHolder nativeAdHolder1;
        [SerializeField] private NativeAdHolder nativeAdHolder2;
        [SerializeField] private NativeAdHolder nativeAdHolder3;
        [SerializeField] private TextMeshProUGUI impressionNumber;
        [SerializeField] private RotateTowardsTarget arrow;
        [SerializeField] private GameObject GameOverScreen;
        private int numberOfImpression = 0;
        private void Awake()
        {
            nativeAdHolder1.Event_AdImpression += NativeAdHolder1_Event_AdImpression;        //Subscribe to native ad impression event
            nativeAdHolder2.Event_AdImpression += NativeAdHolder2_Event_AdImpression;        //Subscribe to native ad impression event
            nativeAdHolder3.Event_AdImpression += NativeAdHolder3_Event_AdImpression;        //Subscribe to native ad impression event
            nativeAdHolder1.Event_AdFailed += NativeAdHolder1_Event_AdImpression;        //Subscribe to native ad failed event
            nativeAdHolder2.Event_AdFailed += NativeAdHolder2_Event_AdImpression;        //Subscribe to native ad failed event
            nativeAdHolder3.Event_AdFailed += NativeAdHolder3_Event_AdImpression;        //Subscribe to native ad failed event
        }
        private void OnDestroy()
        {
            nativeAdHolder1.Event_AdImpression -= NativeAdHolder1_Event_AdImpression;       //Unsubscribe to native ad impression event
            nativeAdHolder2.Event_AdImpression -= NativeAdHolder2_Event_AdImpression;       //Unsubscribe to native ad impression event
            nativeAdHolder3.Event_AdImpression -= NativeAdHolder3_Event_AdImpression;       //Unsubscribe to native ad impression event
            nativeAdHolder1.Event_AdFailed -= NativeAdHolder1_Event_AdImpression;        //Unsubscribe to native ad failed event
            nativeAdHolder2.Event_AdFailed -= NativeAdHolder2_Event_AdImpression;        //Unsubscribe to native ad failed event
            nativeAdHolder3.Event_AdFailed -= NativeAdHolder3_Event_AdImpression;        //Unsubscribe to native ad failed event

        }
        private void NativeAdHolder3_Event_AdImpression(object arg1, System.EventArgs arg2)
        {
            UpdateImpression(3);
        }

        private void NativeAdHolder2_Event_AdImpression(object arg1, System.EventArgs arg2)
        {
            UpdateImpression(2);
        }

        private void NativeAdHolder1_Event_AdImpression(object arg1, System.EventArgs arg2)
        {
            UpdateImpression(1);
        }
        void UpdateImpression(int index)
        {
            numberOfImpression++;
            switch(index)
            {
                case 1:
                    arrow.targetTransform = nativeAdHolder2.transform;
                    break;
                case 2:
                    arrow.targetTransform = nativeAdHolder3.transform;
                    break;
                case 3:
                    arrow.gameObject.SetActive(false);
                    break;
            }
            impressionNumber.text = numberOfImpression + "/3";
            if (numberOfImpression == 3)
                Invoke(nameof(LevelComplete), 2f);
        }

        private void LevelComplete()
        {
            GameOverScreen.SetActive(true);
        }
    }
}
