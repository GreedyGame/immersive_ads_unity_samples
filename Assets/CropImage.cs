using System.Collections;
using System.Collections.Generic;
using PubScale.SdkOne.NativeAds;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public enum VariantType
{
    NONE = -1,
    SLIDE_HORIZONTAL,
    SLIDE_VERTICAL,
    ROLL
}
public class CropImage : MonoBehaviour
{
    public Image bigImage; 
    public NativeAdHolder nativeAdHolder;
    Animator _animator;
    public TextMeshProUGUI ctaText;
    string ctaString = "";
    public VariantType variantType;

    private void Awake() {
        _animator = GetComponent<Animator>();
    }
    private void OnEnable() 
    {
        _animator.Play("Portrait_intro");
        _animator.SetInteger("VariantId", (int) variantType);
        StartCoroutine(GetCroppedBigImage());
    }
    IEnumerator GetCroppedBigImage()
    {
        yield return new WaitForSeconds(.1f);
        
        // bigImage.sprite = ImageUtility.TextureToSprite(nativeAdHolder?.adDisplay.BigMediaImages[0]);
    }


    public List<Sprite> bigImages;
    int index = 0;
    public void SwitchBigImage()
    {
        Debug.Log("Switch");
        if(index > bigImages.Count - 1) //2
        {
            index = 0;
        }

        bigImage.sprite = bigImages[index];
        index ++;
        Debug.Log(index);

        // _animation.clip.

    }

}
