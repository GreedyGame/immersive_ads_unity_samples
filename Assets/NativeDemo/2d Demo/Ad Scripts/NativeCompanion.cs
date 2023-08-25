using GoogleMobileAds.Api;
using UnityEngine;
namespace PubScale.SdkOne.NativeAds.Sample
{
    /// <summary>
    /// Shows and fill the UI native ad unit when requested
    /// </summary>
    public class NativeCompanion : MonoBehaviour
    {
        [SerializeField] private Animator anim;
        [SerializeField] private NativeAdDisplayHandler companionDisplay;
        private bool gotTheFill;

        private void OnEnable()
        {
            NativeAdCompanionTrigger.ShowAd += ShowAd;                              //Subscrilbe to show/hide the native ad when requested
            CompanionDisplayer.FillCompanion += CompanionDisplayer_FillCompanion;   //Subscrilbe to fill the native ad when requested
        }

        private void OnDisable()
        {
            NativeAdCompanionTrigger.ShowAd -= ShowAd;                               //Unsubscrilbe to show/hide the native ad when requested
            CompanionDisplayer.FillCompanion -= CompanionDisplayer_FillCompanion;    //Unsubscrilbe to fill the native ad when requested
        }
        private void CompanionDisplayer_FillCompanion(NativeAd nativeAd,NativeAdHolder holder)
        {
            Debug.Log("Got companion fill");
            gotTheFill = true;
            companionDisplay.FillAndRegister(nativeAd,holder ,true);
        }

         void ShowAd(bool show)
        {
#if UNITY_EDITOR
            if (anim != null)
                anim.SetBool("Show", show);
#endif
            if (!gotTheFill)
                return;
            if (anim != null)
                anim.SetBool("Show", show);

        }
    }
}
