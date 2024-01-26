using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using PubScale.SdkOne.NativeAds;
using System;
using PubScale.Common;
public class LandscapeAdAnimationVariant2 : MonoBehaviour
{
    public Transform pilot;
    public List<Sprite> images;  // Array to hold your big images
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
    private float slideDuration = 1.5f;
    public float yOffset;
    NativeAdHolder nativeAdHolder;
    float transitionDuration = 14/4;
    float refreshTime = 15;

    private void Awake() 
    {
        dynamicADFormatHandler = GetComponent<DynamicADFormatHandler>();
        targetCtaString = dynamicADFormatHandler?.adCallToActionTxt.text;
    }

    private void OnEnable() 
    {
        InitState();
        StartCoroutine(Start());
    }
    IEnumerator Start()
    {
        AnimUtils.DoPunchScale(AdIcon.gameObject,.5f, .5f);
        GetBigImageSprites();
        yield return new WaitForSeconds(.5f);
        AnimUtils.DoScale(AdIcon, Vector3.zero, 0.2f);
        yield return new WaitForSeconds(.5f);
        // AdDetails.gameObject.SetActive(true);
        // yield return new WaitForSeconds(slideDuration); 
        // AnimUtils.MoveLocal(AdDetails, new Vector3(0, AdDetails.localPosition.y, AdDetails.position.z), slideDuration);
        AnimUtils.DoMoveLocal(buttonCta, new Vector3(0, buttonCta.localPosition.y, buttonCta.position.z), slideDuration, this);
        AnimUtils.DoMoveLocal(pilot, new Vector2(0,yOffset), slideDuration, this);  
        MyCallback();
        // Sequence mySequence = DOTween.Sequence();
        // mySequence.Append(AdIcon.DOPunchScale(Vector3.one * .1f,.5f,0,0));
        // mySequence.Append(AdIcon.DOScale(0, .5f));
        // mySequence.InsertCallback(1,MyCallback);
    }

    private IEnumerator MyNewMethod()
    {
        int currentIndex = 0;
        int count = 0;
        // AdDetails.DOLocalMoveX(0, .01f);
        // pilot.DOLocalMoveX(0,slideDuration);    
        // buttonCta.DOLocalMoveX(0,slideDuration);
        // AdDetails.SetLocalPositionAndRotation(new Vector2(moveDistance, AdDetails.localPosition.y),AdDetails.rotation);
        yield return new WaitForSeconds(transitionDuration); 
        // Initialize the first image position to the left of the screen
        AnimUtils.DoMoveLocal(pilot, new Vector2(-moveDistance, yOffset), slideDuration, this);
        // pilot.DOLocalMoveX(-moveDistance, slideDuration);
        imageIn.rectTransform.anchoredPosition = new Vector2(moveDistance, yOffset);
        imageIn.sprite = images[currentIndex];
        // imageIn.rectTransform.DOAnchorPosX(0f, slideDuration);
        AnimUtils.DoAnchorMoveX(imageIn.rectTransform, 0, slideDuration,imageIn.GetComponent<MonoBehaviour>());
        yield return new WaitForSeconds(transitionDuration);
        pilot.SetLocalPositionAndRotation(new Vector2(moveDistance, yOffset),pilot.rotation);
        while (count < images.Count - 1)
        {
            // Set the outgoing image to the current image
            imageOut.sprite = images[currentIndex];
            
            // Move out to the left
            imageOut.rectTransform.anchoredPosition = new Vector2(0, yOffset);
            // imageOut.rectTransform.DOAnchorPosX(-moveDistance, slideDuration);
            AnimUtils.DoAnchorMoveX(imageOut.rectTransform, -moveDistance, slideDuration,imageIn.GetComponent<MonoBehaviour>());
            // Prepare the next index
            int nextIndex = (currentIndex + 1) % images.Count;

            // Move in from the right
            imageIn.sprite = images[nextIndex];
            imageIn.rectTransform.anchoredPosition = new Vector2(moveDistance, yOffset);
            // imageIn.rectTransform.DOAnchorPosX(0f, slideDuration);
            AnimUtils.DoAnchorMoveX(imageIn.rectTransform, 0, slideDuration,imageIn.GetComponent<MonoBehaviour>());
            yield return new WaitForSeconds(transitionDuration);

            currentIndex = nextIndex;
            count++;
        }
        // pilot.DOLocalMoveX(0,1,true);  
        yield return new WaitForSeconds(transitionDuration);
        // Sequence mySequence = DOTween.Sequence();
        AnimUtils.DoAnchorMoveX(imageIn.rectTransform, -moveDistance, slideDuration,imageIn.GetComponent<MonoBehaviour>());
        // mySequence.Append(imageIn.rectTransform.DOAnchorPosX(-moveDistance, slideDuration));
        // mySequence.Insert(0,pilot.DOLocalMoveX(0,slideDuration));
        AnimUtils.DoMoveLocal(pilot, new Vector2(0, yOffset), slideDuration, this);
        // mySequence.InsertCallback(3,MyCallback);
        // yield return new WaitForSeconds(slideDuration);
        MyCallback();
    }

    private void MyCallback()
    {
        if(images.Count > 0)
            StartCoroutine(MyNewMethod());
    }

    private void InitState()
    {
        pilot.SetLocalPositionAndRotation(new Vector2(moveDistance, yOffset),pilot.rotation);
        AdIcon.localScale = Vector3.one * 2;
        AdIcon.SetLocalPositionAndRotation(new Vector2(0, 0),pilot.rotation);
        buttonCta.SetLocalPositionAndRotation(new Vector2(moveDistance, buttonCta.transform.localPosition.y),buttonCta.rotation);
        buttonCta.localScale = Vector3.one;
        // AdDetails.localScale = Vector3.one;
        // AdDetails.gameObject.SetActive(false);
        // AdDetails.SetLocalPositionAndRotation(new Vector2(moveDistance, AdDetails.localPosition.y),AdDetails.rotation);
        imageIn.rectTransform.anchoredPosition = new Vector2(moveDistance, yOffset);
        imageOut.rectTransform.anchoredPosition = new Vector2(moveDistance, yOffset);
        ctaText.text = "";
        ctaString = "";
    }
    private List<Texture2D> bigImageTextures = new();
    private void GetBigImageSprites()
    {
        bigImageTextures = GetComponentInParent<NativeAdHolder>().adDisplay.BigMediaImages;
        Debug.Log("Number of big image textures : " + bigImageTextures.Count);
        // if(bigImageTextures == null || bigImageTextures.Count <= 0) return;

        images.Clear();
        foreach (Texture2D texture in bigImageTextures)
        {
            ConvertTextureToSprite(texture, (sprite) =>
            {
                for (int i = 0; i < bigImageTextures.Count; i++)
                {
                    images.Add(sprite);
                    Debug.Log("image name : " + images[i].name);
                }
            });
        }
    }
    void ConvertTextureToSprite(Texture2D texture, Action<Sprite> OnSpriteConverted)
    {
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100, 0, meshType: SpriteMeshType.FullRect);
        OnSpriteConverted?.Invoke(sprite);
    }
}
