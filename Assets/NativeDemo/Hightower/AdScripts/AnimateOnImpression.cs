using PubScale.SdkOne.NativeAds;
using UnityEngine;

public class AnimateOnImpression : MonoBehaviour
{
    [SerializeField] private DynamicADFormatHandler dynamicADFormatHandler;
    [SerializeField] private NativeAdHolder nativeAdHolder;
    [SerializeField] private float delayTime = 3;

    private void OnValidate()
    {
        if (dynamicADFormatHandler == null)
            dynamicADFormatHandler = GetComponent<DynamicADFormatHandler>();
        if (nativeAdHolder == null)
            nativeAdHolder = GetComponentInParent<NativeAdHolder>();
    }
    private void Awake()
    {
        nativeAdHolder.Event_AdImpression += NativeAdHolder_Event_AdImpression;
    }

    private void NativeAdHolder_Event_AdImpression(object arg1, System.EventArgs arg2)
    {
        if (dynamicADFormatHandler == null || nativeAdHolder == null)
            return;

        Invoke("DelayAnimate", delayTime);

    }
    public void DelayAnimate()
    {
        dynamicADFormatHandler.AnimateNow();
    }

}
