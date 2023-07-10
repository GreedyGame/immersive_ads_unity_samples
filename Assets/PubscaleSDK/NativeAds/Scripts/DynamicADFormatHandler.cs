using GoogleMobileAds.Api;
using PubScale.SdkOne.NativeAds;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class DynamicADFormatHandler : NativeAdDisplayHandler
{
    public GameObject ctaStars;
    public GameObject starAdvertiser;
   public GameObject starTypeText;
   public GameObject starTypeVisuals;

    [SerializeField] private bool preserveLayout = false;
    [SerializeField] private int NumberOfAnimations = -1;
    [SerializeField] private Animator animator;
    private Dictionary<GameObject, bool> elementsState = new Dictionary<GameObject, bool>();
    void Awake()
    {
        if (NumberOfAnimations > 0)
        {
            animator = GetComponent<Animator>();
        }
        if (preserveLayout)
            SaveState();

        Show_StarRating = true;
        Show_RatingAsVisual = true;
        Show_RatingAsText = true;
    }
    //Save the active state of the all the gameobject variable in a dictionary
    private void SaveState()
    {
        if (adIconImgHolder != null && !elementsState.ContainsKey(adIconImgHolder.gameObject))
            elementsState.Add(adIconImgHolder.gameObject, adIconImgHolder.gameObject.activeInHierarchy);
        if (adIconImg != null && !elementsState.ContainsKey(adIconImg.gameObject))
            elementsState.Add(adIconImg.gameObject, adIconImg.gameObject.activeInHierarchy);
        if (adChoicesImg != null && !elementsState.ContainsKey(adChoicesImg.gameObject))
            elementsState.Add(adChoicesImg.gameObject, adChoicesImg.gameObject.activeInHierarchy);
        if (imageTextures != null && !elementsState.ContainsKey(imageTextures.gameObject))
            elementsState.Add(imageTextures.gameObject, imageTextures.gameObject.activeInHierarchy);
        if (adHeadlineTxt != null && !elementsState.ContainsKey(adHeadlineTxt.gameObject))
            elementsState.Add(adHeadlineTxt.gameObject, adHeadlineTxt.gameObject.activeInHierarchy);
        if (adCallToActionTxt != null && !elementsState.ContainsKey(adCallToActionTxt.gameObject))
            elementsState.Add(adCallToActionTxt.gameObject, adCallToActionTxt.gameObject.activeInHierarchy);
        if (adAdvertiserTxt != null && !elementsState.ContainsKey(adAdvertiserTxt.gameObject))
            elementsState.Add(adAdvertiserTxt.gameObject, adAdvertiserTxt.gameObject.activeInHierarchy);
        if (bodyTxt != null && !elementsState.ContainsKey(bodyTxt.gameObject))
            elementsState.Add(bodyTxt.gameObject, bodyTxt.gameObject.activeInHierarchy);
        if (priceTxt != null && !elementsState.ContainsKey(priceTxt.gameObject))
            elementsState.Add(priceTxt.gameObject, priceTxt.gameObject.activeInHierarchy);
        if (storeTxt != null && !elementsState.ContainsKey(storeTxt.gameObject))
            elementsState.Add(storeTxt.gameObject, storeTxt.gameObject.activeInHierarchy);
        if (starRatingStroke != null && !elementsState.ContainsKey(starRatingStroke.gameObject))
            elementsState.Add(starRatingStroke.gameObject, starRatingStroke.gameObject.activeInHierarchy);
        if (starRatingFill != null && !elementsState.ContainsKey(starRatingFill.gameObject))
            elementsState.Add(starRatingFill.gameObject, starRatingFill.gameObject.activeInHierarchy);
        if (ctaStars != null && !elementsState.ContainsKey(ctaStars.gameObject))
            elementsState.Add(ctaStars.gameObject, ctaStars.gameObject.activeInHierarchy);
        if (starAdvertiser != null && !elementsState.ContainsKey(starAdvertiser.gameObject))
            elementsState.Add(starAdvertiser.gameObject, starAdvertiser.gameObject.activeInHierarchy);
        if (starTypeText != null && !elementsState.ContainsKey(starTypeText.gameObject))
            elementsState.Add(starTypeText.gameObject, starTypeText.gameObject.activeInHierarchy);
        if (starTypeVisuals != null && !elementsState.ContainsKey(starTypeVisuals.gameObject))
            elementsState.Add(starTypeVisuals.gameObject, starTypeVisuals.gameObject.activeInHierarchy);
        if (starRatingTxt != null && !elementsState.ContainsKey(starRatingTxt.gameObject))
            elementsState.Add(starRatingTxt.gameObject, starRatingTxt.gameObject.activeInHierarchy);
    }

    public override void FillAndRegister(NativeAd nativeAd, bool registerElement = true)
    {

        base.FillAndRegister(nativeAd, true);

        var starRating = nativeAd.GetStarRating();

        if (starRating > 0)
        {
            if (Random.Range(0,2)==0)
            {
                if (starRatingTxt != null)
                    starRatingTxt.text = starRating.ToString();
                if (starTypeText != null)
                    starTypeText.gameObject.SetActive(true);
                if (starTypeVisuals != null)
                {
                    starTypeVisuals.gameObject.SetActive(false);
                    starRatingFill.gameObject.SetActive(false);
                    starRatingStroke.gameObject.SetActive(false);
                }
            }
            else
            {
                if (starTypeText != null)
                    starTypeText.gameObject.SetActive(false);
                if (starTypeVisuals != null)
                {
                    starRatingFill.gameObject.SetActive(true);
                    starRatingStroke.gameObject.SetActive(true);
                    starTypeVisuals.gameObject.SetActive(true);
                }
            }
        }
        else
        {
            if (starTypeText != null)
                starTypeText.SetActive(false);
            if (starTypeVisuals != null)
                starTypeVisuals.SetActive(false);
        }
        if (storeTxt != null && adAdvertiserTxt != null)
            if (string.IsNullOrEmpty(adAdvertiserTxt.text) && !string.IsNullOrEmpty(storeTxt.text))
                adAdvertiserTxt.text = storeTxt.text;
        if (priceTxt != null && adAdvertiserTxt != null)
            if (string.IsNullOrEmpty(adAdvertiserTxt.text) && !string.IsNullOrEmpty(priceTxt.text))
                adAdvertiserTxt.text = priceTxt.text;
        if (adAdvertiserTxt != null)
            adAdvertiserTxt.gameObject.SetActive(!(string.IsNullOrEmpty(nativeAd.GetAdvertiserText()) && string.IsNullOrEmpty(nativeAd.GetPrice()) && string.IsNullOrEmpty(nativeAd.GetStore())));
        if (adCallToActionTxt != null)
            adCallToActionTxt.transform.parent.gameObject.SetActive(!string.IsNullOrEmpty(nativeAd.GetCallToActionText()));

        bool isStarOrAdvertiserAvailable = false;
        if (adAdvertiserTxt != null)
            isStarOrAdvertiserAvailable = (starRating > 0 || !string.IsNullOrEmpty(adAdvertiserTxt.text));
        if (starAdvertiser != null)
            starAdvertiser.SetActive(isStarOrAdvertiserAvailable);

        if (ctaStars != null)
            ctaStars.SetActive(!string.IsNullOrEmpty(nativeAd.GetCallToActionText()) || isStarOrAdvertiserAvailable);

        if (preserveLayout)
            RevertState();

        AnimateNow();
    }

    public void AnimateNow()
    {
        if (NumberOfAnimations > 0)
        {
            if (animator != null)
            {
                animator.enabled = true;
                animator.Play("Type" + Random.Range(1, NumberOfAnimations + 1));
            }
        }
        else
        {
            if (animator != null)
                animator.enabled = false;
        }
    }

    void RevertState()
    {
        foreach (var item in elementsState)
        {
            item.Key.SetActive(item.Value);
        }
    }
}
