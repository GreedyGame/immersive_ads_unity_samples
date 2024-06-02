#if PUBSCALE_EFFECTS
using Coffee.UIEffects;
#endif
using GoogleMobileAds.Api;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace PubScale.SdkOne.NativeAds
{
    public class DynamicADFormatHandler : NativeAdDisplayHandler
    {
        public GameObject ctaStars;
        public GameObject starAdvertiser;
        public GameObject starTypeText;
        public GameObject starTypeVisuals;
        public Image singleStarImg;
        public Image ctaHolderImg;

        public int NumberOfAnimations = -1;
        public Animator animator;


#if UNITY_EDITOR
#pragma warning disable CS0649
        public TMP_FontAsset defaultFont;
        public bool OverrideColors = false;
        public Color starsColor;
        public Color ctaButtonColor;
        public Color textColor;
#pragma warning restore CS0649
#if PUBSCALE_EFFECTS
    public bool useShineEffect = false;
#endif



        public void OnValidate()
        {
#if PUBSCALE_EFFECTS
        UIShiny effects = null;
        ctaHolderImg.TryGetComponent(out effects);
        if (effects != null)
        {
            effects.enabled = useShineEffect;

        }
        else if(effects==null&&useShineEffect)
        {
            effects = ctaHolderImg.gameObject.AddComponent<UIShiny>();
        }
        if (useShineEffect)
            SetEffectValues(effects);

        void SetEffectValues(UIShiny effect)
        {
            if(effect==null)
            {
                return;
            }
            effect.width = 0.108f;
            effect.rotation = 135;
            effect.softness= 1;
            effect.brightness= 0.573f;
            effect.gloss= 0.252f;
            effect.effectPlayer.play=true;
            effect.effectPlayer.loop=true;
            effect.effectPlayer.initialPlayDelay = 2;
            effect.effectPlayer.duration = 2;
            effect.effectPlayer.loopDelay= 3;
        }
#endif
            ChangeColors();
            UpdateFont();
        }
        void UpdateFont()
        {
            if (defaultFont == null)
                return;
            CheckAndUpdateTxtFont(adHeadlineTxt, defaultFont);
            CheckAndUpdateTxtFont(adAdvertiserTxt, defaultFont);
            CheckAndUpdateTxtFont(bodyTxt, defaultFont);
            CheckAndUpdateTxtFont(adCallToActionTxt, defaultFont);
            CheckAndUpdateTxtFont(priceTxt, defaultFont);
            CheckAndUpdateTxtFont(storeTxt, defaultFont);
            CheckAndUpdateTxtFont(starRatingTxt, defaultFont);
        }
        void CheckAndUpdateTxtFont(TextMeshProUGUI txt, TMP_FontAsset tMP_Font)
        {
            if (txt != null)
            {
                txt.font = tMP_Font;
            }
        }
        void ChangeColors()
        {
            if (!OverrideColors)
                return;
            CheckAndUpdateColor(textColor, adHeadlineTxt);
            CheckAndUpdateColor(textColor, adAdvertiserTxt);
            CheckAndUpdateColor(textColor, bodyTxt);
            CheckAndUpdateColor(textColor, adCallToActionTxt);
            CheckAndUpdateColor(textColor, priceTxt);
            CheckAndUpdateColor(textColor, storeTxt);
            CheckAndUpdateColor(textColor, starRatingTxt);
            CheckAndUpdateColor(starsColor, img: starRatingStroke);
            CheckAndUpdateColor(starsColor, img: starRatingFill);
            CheckAndUpdateColor(starsColor, img: singleStarImg);
            CheckAndUpdateColor(ctaButtonColor, img: ctaHolderImg);
        }
#endif
        public void CheckAndUpdateColor(Color color, TextMeshProUGUI txt = null, Image img = null)
        {
            if (txt != null)
            {
                txt.color = color;
            }
            if (img != null)
            {
                img.color = color;
            }
        }
        void Awake()
        {
            if (NumberOfAnimations > 0)
            {
                TryGetComponent(out animator);
            }
        }


        public override void FillAndRegister(NativeAd nativeAd, NativeAdHolder holder, bool registerElement = true)
        {
            base.FillAndRegister(nativeAd, holder, true);

            var starRating = nativeAd.GetStarRating();

            if (Show_StarRating && starRating > 0)
            {
                if (Random.Range(0, 2) == 0) //randomly show stars as text or visuals
                {
                    //shows star rating as text

                    if (starRatingTxt != null)
                        starRatingTxt.text = starRating.ToString();
                    if (starTypeText != null)
                        starTypeText.gameObject.SetActive(true);
                    if (starTypeVisuals != null)
                    {
                        starTypeVisuals.gameObject.SetActive(false);
                        if (starRatingFill != null)
                            starRatingFill.gameObject.SetActive(false);
                        if (starRatingStroke != null)
                            starRatingStroke.gameObject.SetActive(false);
                    }
                }
                else
                {
                    //shows star rating as visuals

                    if (starTypeText != null)
                        starTypeText.gameObject.SetActive(false);
                    if (starTypeVisuals != null)
                    {
                        starTypeVisuals.gameObject.SetActive(true);
                        if (starRatingFill != null)
                            starRatingFill.gameObject.SetActive(true);
                        if (starRatingStroke != null)
                            starRatingStroke.gameObject.SetActive(true);
                    }
                }
            }
            else
            {
                //disable star rating visuals and text if we don't have a star rating
                if (starTypeText != null)
                    starTypeText.SetActive(false);
                if (starTypeVisuals != null)
                    starTypeVisuals.SetActive(false);
            }


            //---------  Advertiser Text 

            bool isAdvertiserTxtUIAssigned = (adAdvertiserTxt != null);
            bool isStoreTxtUIAssigned = (storeTxt != null);
            bool isPriceTxtUIAssigned = (priceTxt != null);

            string advertiserTxtValueFromAdmob = nativeAd.GetAdvertiserText();
            string storeTxtValueFromAdmob = nativeAd.GetStore();
            string priceTxtValueFromAdmob = nativeAd.GetPrice();

            bool isValid_AdvertiserTxtValue = isAdvertiserTxtUIAssigned && (!string.IsNullOrEmpty(advertiserTxtValueFromAdmob));
            bool isValid_StoreTextValue = isStoreTxtUIAssigned && (!string.IsNullOrEmpty(storeTxtValueFromAdmob));
            bool isValid_PriceTextValue = isPriceTxtUIAssigned && (!string.IsNullOrEmpty(priceTxtValueFromAdmob));


            if (!isValid_AdvertiserTxtValue && !isValid_StoreTextValue && !isValid_PriceTextValue)
            {
                if (isAdvertiserTxtUIAssigned)
                    adAdvertiserTxt.gameObject.SetActive(false);
            }
            else
            {
                //check if we have a store or price and if we don't have an advertiser, use the store or price as advertiser
                if (isAdvertiserTxtUIAssigned && !isValid_AdvertiserTxtValue)
                {
                    if (isValid_StoreTextValue)
                        adAdvertiserTxt.text = storeTxtValueFromAdmob;

                    if (isValid_PriceTextValue)
                        adAdvertiserTxt.text = priceTxtValueFromAdmob;
                }
            }

            string ctaTxtFromAdmob = nativeAd.GetCallToActionText();
            bool isValidCTATxt = !string.IsNullOrEmpty(ctaTxtFromAdmob);

            //disable CTA if we don't have a CTA text
            if (adCallToActionTxt != null)
                adCallToActionTxt.transform.parent.gameObject.SetActive(isValidCTATxt);

            //check if we have a star rating or advertiser text then show the star advertiser parent gameobject else hide it
            bool isStarOrAdvertiserAvailable = false;

            if (isAdvertiserTxtUIAssigned)
                isStarOrAdvertiserAvailable = (starRating > 0 || isValid_AdvertiserTxtValue);

            if (starAdvertiser != null)
                starAdvertiser.SetActive(isStarOrAdvertiserAvailable);

            //check if we have a CTA text or star rating then show the CTA stars parent gameobject else hide it
            if (ctaStars != null)
                ctaStars.SetActive(isValidCTATxt || isStarOrAdvertiserAvailable);

            if (DisableAnimation == false)
                AnimateNow();
        }

        public override void FillAndRegister(GGAdDownloader ggDL, NativeAdHolder nativeAdHolder, System.Action<GGAdDownloader> impCB, System.Action<GGAdDownloader> clickCB, bool registerElement)
        {
            base.FillAndRegister(ggDL, nativeAdHolder, impCB, clickCB);

            if (DisableAnimation == false && (ggDL.AdFormat == "nat"))
                AnimateNow();
        }



        public void AnimateNow()
        {
            if (DisableAnimation)
                return;

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
    }
}