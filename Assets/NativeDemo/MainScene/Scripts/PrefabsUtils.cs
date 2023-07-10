#if UNITY_EDITOR
using UnityEngine;
using PubScale.SdkOne.NativeAds;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

[CustomEditor(typeof(PrefabsUtils))]
public class PrefabUtilsEditor:Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        PrefabsUtils myScript = (PrefabsUtils)target;
        if (GUILayout.Button("Change Name"))
        {
            myScript.ChangeNameOfComponents();
        }
        if (GUILayout.Button("Assign Components"))
        {
            myScript.AssignComponents();
        }
    }
}

[RequireComponent(typeof(NativeAdDisplayHandler))]
public class PrefabsUtils : MonoBehaviour
{
    public NativeAdDisplayHandler nativeAdDisplayHandler;
    public DynamicADFormatHandler dynamicADFormatHandler;
    private NativeAdHolder nativeAdHolder;
    public void OnValidate()
    {

        dynamicADFormatHandler = null;
        TryGetComponent<DynamicADFormatHandler>(out dynamicADFormatHandler);
        nativeAdDisplayHandler = null;
        if (dynamicADFormatHandler == null)
        {
            TryGetComponent<NativeAdDisplayHandler>(out nativeAdDisplayHandler);
        }
           
        if(nativeAdDisplayHandler == null&& dynamicADFormatHandler==null)
            Debug.LogError("No NativeAdDisplayHandler or DynamicADFormatHandler found");
    }



    public void ChangeNameOfComponents()
    {
        if (nativeAdDisplayHandler != null)
            ChangeName();
        else if (dynamicADFormatHandler != null)
            ChangeNameDynamic();
        else
        {
            Debug.LogError("No NativeAdDisplayHandler or DynamicADFormatHandler found");
        }
    }

    public void AssignComponents()
    {
        if (nativeAdDisplayHandler != null)
            AssignElements();
        else if (dynamicADFormatHandler != null)
            AssignElementsDynamic();
        else
        {
            Debug.LogError("No NativeAdDisplayHandler or DynamicADFormatHandler found");
        }
    }

    void ChangeName()
    {
        if (nativeAdDisplayHandler.adIconImgHolder != null)
            nativeAdDisplayHandler.adIconImgHolder.name = nameof(nativeAdDisplayHandler.adIconImgHolder);
        if (nativeAdDisplayHandler.adIconImg != null)
            nativeAdDisplayHandler.adIconImg.name = nameof(nativeAdDisplayHandler.adIconImg);
        if (nativeAdDisplayHandler.adChoicesImg != null)
            nativeAdDisplayHandler.adChoicesImg.name = nameof(nativeAdDisplayHandler.adChoicesImg);
        if (nativeAdDisplayHandler.imageTextures != null)
            nativeAdDisplayHandler.imageTextures.name = nameof(nativeAdDisplayHandler.imageTextures);
        if (nativeAdDisplayHandler.adHeadlineTxt != null)
            nativeAdDisplayHandler.adHeadlineTxt.name = nameof(nativeAdDisplayHandler.adHeadlineTxt);
        if (nativeAdDisplayHandler.adCallToActionTxt != null)
            nativeAdDisplayHandler.adCallToActionTxt.name = nameof(nativeAdDisplayHandler.adCallToActionTxt);
        if (nativeAdDisplayHandler.adAdvertiserTxt != null)
            nativeAdDisplayHandler.adAdvertiserTxt.name = nameof(nativeAdDisplayHandler.adAdvertiserTxt);
        if (nativeAdDisplayHandler.bodyTxt != null)
            nativeAdDisplayHandler.bodyTxt.name = nameof(nativeAdDisplayHandler.bodyTxt);
        if (nativeAdDisplayHandler.priceTxt != null)
            nativeAdDisplayHandler.priceTxt.name = nameof(nativeAdDisplayHandler.priceTxt);
        if (nativeAdDisplayHandler.storeTxt != null)
            nativeAdDisplayHandler.storeTxt.name = nameof(nativeAdDisplayHandler.storeTxt);
        if (nativeAdDisplayHandler.starRatingStroke != null)
            nativeAdDisplayHandler.starRatingStroke.name = nameof(nativeAdDisplayHandler.starRatingStroke);
        if (nativeAdDisplayHandler.starRatingFill != null)
            nativeAdDisplayHandler.starRatingFill.name = nameof(nativeAdDisplayHandler.starRatingFill);
    }
    void ChangeNameDynamic()
    {
        if (dynamicADFormatHandler.adIconImgHolder != null)
            dynamicADFormatHandler.adIconImgHolder.name = nameof(dynamicADFormatHandler.adIconImgHolder);
        if (dynamicADFormatHandler.adIconImg != null)
            dynamicADFormatHandler.adIconImg.name = nameof(dynamicADFormatHandler.adIconImg);
        if (dynamicADFormatHandler.adChoicesImg != null)
            dynamicADFormatHandler.adChoicesImg.name = nameof(dynamicADFormatHandler.adChoicesImg);
        if (dynamicADFormatHandler.imageTextures != null)
            dynamicADFormatHandler.imageTextures.name = nameof(dynamicADFormatHandler.imageTextures);
        if (dynamicADFormatHandler.adHeadlineTxt != null)
            dynamicADFormatHandler.adHeadlineTxt.name = nameof(dynamicADFormatHandler.adHeadlineTxt);
        if (dynamicADFormatHandler.adCallToActionTxt != null)
            dynamicADFormatHandler.adCallToActionTxt.name = nameof(dynamicADFormatHandler.adCallToActionTxt);
        if (dynamicADFormatHandler.adAdvertiserTxt != null)
            dynamicADFormatHandler.adAdvertiserTxt.name = nameof(dynamicADFormatHandler.adAdvertiserTxt);
        if (dynamicADFormatHandler.bodyTxt != null)
            dynamicADFormatHandler.bodyTxt.name = nameof(dynamicADFormatHandler.bodyTxt);
        if (dynamicADFormatHandler.priceTxt != null)
            dynamicADFormatHandler.priceTxt.name = nameof(dynamicADFormatHandler.priceTxt);
        if (dynamicADFormatHandler.storeTxt != null)
            dynamicADFormatHandler.storeTxt.name = nameof(dynamicADFormatHandler.storeTxt);
        if (dynamicADFormatHandler.starRatingStroke != null)
            dynamicADFormatHandler.starRatingStroke.name = nameof(dynamicADFormatHandler.starRatingStroke);
        if (dynamicADFormatHandler.starRatingFill != null)
            dynamicADFormatHandler.starRatingFill.name = nameof(dynamicADFormatHandler.starRatingFill);
        if (dynamicADFormatHandler.ctaStars != null)
            dynamicADFormatHandler.ctaStars.name = nameof(dynamicADFormatHandler.ctaStars);
        if (dynamicADFormatHandler.starAdvertiser != null)
            dynamicADFormatHandler.starAdvertiser.name = nameof(dynamicADFormatHandler.starAdvertiser);
        if (dynamicADFormatHandler.starTypeText != null)
            dynamicADFormatHandler.starTypeText.name = nameof(dynamicADFormatHandler.starTypeText);
        if (dynamicADFormatHandler.starTypeVisuals != null)
            dynamicADFormatHandler.starTypeVisuals.name = nameof(dynamicADFormatHandler.starTypeVisuals);
        if (dynamicADFormatHandler.starRatingTxt != null)
            dynamicADFormatHandler.starRatingTxt.name = nameof(dynamicADFormatHandler.starRatingTxt);
    }
    void AssignElements()
    {
        NativeAdDisplayHandler ad = nativeAdDisplayHandler;

        ad.adIconImgHolder = ad.adIconImgHolder == null ? FindDeepChild(transform, nameof(ad.adIconImgHolder))?.GetComponent<Image>() : ad.adIconImgHolder;
        ad.adIconImg = ad.adIconImg == null ? FindDeepChild(transform, nameof(ad.adIconImg))?.GetComponent<Image>() : ad.adIconImg;
        ad.adChoicesImg = ad.adChoicesImg == null ? FindDeepChild(transform, nameof(ad.adChoicesImg))?.GetComponent<Image>() : ad.adChoicesImg;
        ad.imageTextures = ad.imageTextures == null ? FindDeepChild(transform, nameof(ad.imageTextures))?.GetComponent<Image>() : ad.imageTextures;
        ad.adHeadlineTxt = ad.adHeadlineTxt == null ? FindDeepChild(transform, nameof(ad.adHeadlineTxt))?.GetComponent<TextMeshProUGUI>() : ad.adHeadlineTxt;
        ad.adCallToActionTxt = ad.adCallToActionTxt == null ? FindDeepChild(transform, nameof(ad.adCallToActionTxt))?.GetComponent<TextMeshProUGUI>() : ad.adCallToActionTxt;
        ad.adAdvertiserTxt = ad.adAdvertiserTxt == null ? FindDeepChild(transform, nameof(ad.adAdvertiserTxt))?.GetComponent<TextMeshProUGUI>() : ad.adAdvertiserTxt;
        ad.bodyTxt = ad.bodyTxt == null ? FindDeepChild(transform, nameof(ad.bodyTxt))?.GetComponent<TextMeshProUGUI>() : ad.bodyTxt;
        ad.priceTxt = ad.priceTxt == null ? FindDeepChild(transform, nameof(ad.priceTxt))?.GetComponent<TextMeshProUGUI>() : ad.priceTxt;
        ad.storeTxt = ad.storeTxt == null ? FindDeepChild(transform, nameof(ad.storeTxt))?.GetComponent<TextMeshProUGUI>() : ad.storeTxt;
        ad.starRatingStroke = ad.starRatingStroke == null ? FindDeepChild(transform, nameof(ad.starRatingStroke))?.GetComponent<Image>() : ad.starRatingStroke;
        ad.starRatingFill = ad.starRatingFill == null ? FindDeepChild(transform, nameof(ad.starRatingFill))?.GetComponent<Image>() : ad.starRatingFill;
        //ad.starTypeText = ad.starTypeText == null ? FindDeepChild(transform, nameof(ad.starTypeText))?.gameObject : ad.starTypeText;
        //ad.starTypeVisuals = ad.starTypeVisuals == null ? FindDeepChild(transform, nameof(ad.starTypeVisuals))?.gameObject : ad.starTypeVisuals;
        ad.starRatingTxt = ad.starRatingTxt == null ? FindDeepChild(transform, nameof(ad.starRatingTxt))?.GetComponent<TextMeshProUGUI>() : ad.starRatingTxt;

        ad.adChoicesImg = ad.adChoicesImg == null ? FindDeepChild(transform.parent.parent, nameof(ad.adChoicesImg))?.GetComponent<Image>() : null;
        ad.bodyTxt = ad.bodyTxt == null && ad.adAdvertiserTxt != null ? ad.adAdvertiserTxt : ad.bodyTxt;
        ad.priceTxt = ad.priceTxt == null && ad.adAdvertiserTxt != null ? ad.adAdvertiserTxt : ad.priceTxt;
        ad.storeTxt = ad.storeTxt == null && ad.adAdvertiserTxt != null ? ad.adAdvertiserTxt : ad.storeTxt;
    }
    void AssignElementsDynamic()
    {
        DynamicADFormatHandler ad= dynamicADFormatHandler;
        ad.adIconImgHolder = ad.adIconImgHolder == null ? FindDeepChild(transform, nameof(ad.adIconImgHolder))?.GetComponent<Image>() : ad.adIconImgHolder;
        ad.adIconImg = ad.adIconImg == null ? FindDeepChild(transform, nameof(ad.adIconImg))?.GetComponent<Image>() : ad.adIconImg;
        ad.adChoicesImg = ad.adChoicesImg == null ? FindDeepChild(transform, nameof(ad.adChoicesImg))?.GetComponent<Image>() : ad.adChoicesImg;
        ad.imageTextures = ad.imageTextures == null ? FindDeepChild(transform, nameof(ad.imageTextures))?.GetComponent<Image>() : ad.imageTextures;
        ad.adHeadlineTxt = ad.adHeadlineTxt == null ? FindDeepChild(transform, nameof(ad.adHeadlineTxt))?.GetComponent<TextMeshProUGUI>() : ad.adHeadlineTxt;
        ad.adCallToActionTxt = ad.adCallToActionTxt == null ? FindDeepChild(transform, nameof(ad.adCallToActionTxt))?.GetComponent<TextMeshProUGUI>() : ad.adCallToActionTxt;
        ad.adAdvertiserTxt = ad.adAdvertiserTxt == null ? FindDeepChild(transform, nameof(ad.adAdvertiserTxt))?.GetComponent<TextMeshProUGUI>() : ad.adAdvertiserTxt;
        ad.bodyTxt = ad.bodyTxt == null ? FindDeepChild(transform, nameof(ad.bodyTxt))?.GetComponent<TextMeshProUGUI>() : ad.bodyTxt;
        ad.priceTxt = ad.priceTxt == null ? FindDeepChild(transform, nameof(ad.priceTxt))?.GetComponent<TextMeshProUGUI>() : ad.priceTxt;
        ad.storeTxt = ad.storeTxt == null ? FindDeepChild(transform, nameof(ad.storeTxt))?.GetComponent<TextMeshProUGUI>() : ad.storeTxt;
        ad.starRatingStroke = ad.starRatingStroke == null ? FindDeepChild(transform, nameof(ad.starRatingStroke))?.GetComponent<Image>() : ad.starRatingStroke;
        ad.starRatingFill = ad.starRatingFill == null ? FindDeepChild(transform, nameof(ad.starRatingFill))?.GetComponent<Image>() : ad.starRatingFill;
        ad.starTypeText = ad.starTypeText == null ? FindDeepChild(transform, nameof(ad.starTypeText))?.gameObject : ad.starTypeText;
        ad.starTypeVisuals = ad.starTypeVisuals == null ? FindDeepChild(transform, nameof(ad.starTypeVisuals))?.gameObject : ad.starTypeVisuals;
        ad.starRatingTxt = ad.starRatingTxt == null ? FindDeepChild(transform, nameof(ad.starRatingTxt))?.GetComponent<TextMeshProUGUI>() : ad.starRatingTxt;

        ad.adChoicesImg = ad.adChoicesImg == null ? FindDeepChild(transform.parent.parent, nameof(ad.adChoicesImg))?.GetComponent<Image>() : null;
        ad.bodyTxt = ad.bodyTxt == null && ad.adAdvertiserTxt != null ? ad.adAdvertiserTxt : ad.bodyTxt;
        ad.priceTxt = ad.priceTxt == null && ad.adAdvertiserTxt != null ? ad.adAdvertiserTxt : ad.priceTxt;
        ad.storeTxt = ad.storeTxt == null && ad.adAdvertiserTxt != null ? ad.adAdvertiserTxt : ad.storeTxt;

        ad.ctaStars = ad.ctaStars == null ? FindDeepChild(transform, nameof(ad.ctaStars))?.gameObject: ad.ctaStars;
        ad.starAdvertiser = ad.starAdvertiser == null ? FindDeepChild(transform, nameof(ad.starAdvertiser))?.gameObject : ad.starAdvertiser;
    }
    //Function to find gameobject with name in all the children and children of children of the transform
    private Transform FindDeepChild(Transform aParent, string aName)
    {
        var result = aParent.Find(aName);
        if (result != null)
            return result;
        foreach (Transform child in aParent)
        {
            result = FindDeepChild(child, aName);
            if (result != null)
                return result;
        }
        return null;
    }
}
#endif