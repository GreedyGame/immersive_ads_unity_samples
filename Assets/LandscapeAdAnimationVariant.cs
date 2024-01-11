using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using PubScale.SdkOne.NativeAds;
using System;
using PubScale.Common;
public class LandscapeAdAnimationVariant : MonoBehaviour
{
    public Transform pilot;
    public Sprite[] images;  // Array to hold your big images
    // public Transform[] slides;
    public Transform AdIcon = null;
    public Transform AdDetails = null;
    public Transform buttonCta = null;
    public TextMeshProUGUI ctaText = null;
    string ctaString = "";

    private DynamicADFormatHandler dynamicADFormatHandler = null;
    string targetCtaString = "";
    public Image imageDisplay;
    public Image imageIn;    // Reference to the Image component for the incoming image
    public Image imageOut; 
    private float moveDistance = 800;
    private float slideDuration = 2;
    public float yOffset;

    bool beginSlideshow = false;

    private void Awake() 
    {
        Utils.GetAndroidAdvertiserId();
        dynamicADFormatHandler = GetComponent<DynamicADFormatHandler>();
        targetCtaString = dynamicADFormatHandler?.adCallToActionTxt.text;

        // Application.RequestAdvertisingIdentifierAsync()
    }

    IEnumerator Start()
    {
        yield return new WaitForSeconds(1);
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(AdIcon.DOScale(1.5f, .2f));
        mySequence.Append(AdIcon.DOPunchScale(Vector3.one * .1f,.5f,0,0));
        mySequence.Insert(1,AdIcon.DOScale(1, .3f));
        mySequence.Insert(1,AdIcon.DOLocalMove(new Vector2(-270,80),.3f));
        mySequence.Append(AdDetails.DOScale(1,0.3f));
        mySequence.Append(AdDetails.DOPunchScale(Vector3.one * .05f,.5f,0,0));
        mySequence.Append(buttonCta.DOScale(1,0.3f));
        mySequence.Append(DOTween.To(()=> ctaString, x=>ctaString = x,targetCtaString, targetCtaString.Length/5f).SetEase(Ease.InSine)).OnUpdate(()=>
        {
            ctaText.text = ctaString;
        });
        mySequence.InsertCallback(3,MyCallback);

    }

    private IEnumerator MyNewMethod()
    {
        int currentIndex = 0;
        int count = 0;
        yield return new WaitForSeconds(slideDuration); 
        pilot.DOLocalMoveX(-moveDistance,slideDuration);    
            // Initialize the first image position to the left of the screen

        imageIn.rectTransform.anchoredPosition = new Vector2(moveDistance, yOffset);
        imageIn.sprite = images[currentIndex];
        imageIn.rectTransform.DOAnchorPosX(0f, slideDuration);
        yield return new WaitForSeconds(slideDuration + 1);
        pilot.SetLocalPositionAndRotation(new Vector2(moveDistance, yOffset),pilot.rotation);
        while (count < images.Length - 1)
        {
            // Set the outgoing image to the current image
            imageOut.sprite = images[currentIndex];
            
            // Move out to the left
            imageOut.rectTransform.anchoredPosition = new Vector2(0, yOffset);
            imageOut.rectTransform.DOAnchorPosX(-moveDistance, slideDuration);

            // Prepare the next index
            int nextIndex = (currentIndex + 1) % images.Length;

            // Move in from the right
            imageIn.sprite = images[nextIndex];
            imageIn.rectTransform.anchoredPosition = new Vector2(moveDistance, yOffset);
            imageIn.rectTransform.DOAnchorPosX(0f, slideDuration);

            yield return new WaitForSeconds(slideDuration);

            currentIndex = nextIndex;
            count++;
        }
        // pilot.DOLocalMoveX(0,1,true);  
        yield return new WaitForSeconds(slideDuration);
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(imageIn.rectTransform.DOAnchorPosX(-moveDistance, slideDuration));
        mySequence.Insert(0,pilot.DOLocalMoveX(0,slideDuration));
        mySequence.InsertCallback(3,MyCallback);
    }

    private void MyCallback()
    {
        StartCoroutine(MyNewMethod());
    }
}
