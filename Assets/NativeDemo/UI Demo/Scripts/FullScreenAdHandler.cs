using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PubScale.SdkOne.NativeAds.Sample
{
    public class FullScreenAdHandler : MonoBehaviour
    {
        [SerializeField] private GameObject showBtn;
        [SerializeField] private NativeAdHolder adHolder;
        [SerializeField] private Button closeBtn;
        [SerializeField] private TextMeshProUGUI closeTxt;
        [SerializeField] private GameObject fullScreenAd;


        private void Awake()
        {
            adHolder.Event_AdLoaded += AdHolder_Event_AdLoaded;
            adHolder.Event_AdImpression += AdHolder_Event_AdImpression;
#if UNITY_EDITOR
            showBtn.SetActive(true);
#endif
        }
        private void OnDestroy()
        {
            adHolder.Event_AdLoaded -= AdHolder_Event_AdLoaded;
            adHolder.Event_AdImpression -= AdHolder_Event_AdImpression;

        }
        private void AdHolder_Event_AdImpression(object arg1, System.EventArgs arg2)
        {
            StartCoroutine(StartCloseCountdown());
        }
        private void AdHolder_Event_AdLoaded(object arg1, GoogleMobileAds.Api.NativeAdEventArgs arg2)
        {
            showBtn.SetActive(true);
        }
      
        public void ShowFullScreen()
        {
            showBtn.SetActive(false);
            fullScreenAd.SetActive(true);
#if UNITY_EDITOR
            StartCoroutine(StartCloseCountdown());
#endif
        }
    
        IEnumerator StartCloseCountdown()
        {
            int num = 5;
            while(num > 0)
            {
                closeTxt.text = num.ToString();
                yield return new WaitForSeconds(1f);
                num--;
            }
            closeTxt.text = "X";
            closeBtn.onClick.AddListener(() => { fullScreenAd.SetActive(false); });

        }
    }
}
