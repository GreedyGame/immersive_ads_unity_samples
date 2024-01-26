using System.Collections;
using UnityEngine;
using DG.Tweening;
using PubScale.SdkOne.NativeAds;
using UnityEngine.UI;
public class FlipPanel : MonoBehaviour
{
    [SerializeField] GameObject frontSide;
    [SerializeField] GameObject backSide;

    public Image bigImage;
    public NativeAdHolder nativeAdHolder;
    // Start is called before the first frame update

    private void OnEnable()
    {
        transform.rotation = Quaternion.identity;
        StartCoroutine(DoFlip());
    }

    private void CropBigImage()
    {
        bigImage.sprite = ImageUtility.TextureToSprite(nativeAdHolder.adDisplay.BigMediaImages[0]);
    }

    void Start()
    {
        frontSide.SetActive(false);
        backSide.SetActive(false);

        StartCoroutine(DoFlip());

    }

    IEnumerator DoFlip()
    {
        yield return new WaitForSeconds(1);
        CropBigImage();
        while (true) 
        {
            backSide.SetActive(false);
            frontSide.SetActive(true); 
            yield return new WaitForSeconds(2f);
            transform.DORotate(new Vector2(0, 90), 1f, RotateMode.Fast);
            yield return new WaitForSeconds(1f);
            transform.DORotate(new Vector2(0, 0), 1f, RotateMode.Fast);
            backSide.SetActive(true);
            frontSide.SetActive(false);
            yield return new WaitForSeconds(2f);
            transform.DORotate(new Vector2(0, 90), 1f, RotateMode.Fast);
            yield return new WaitForSeconds(1f);
            transform.DORotate(new Vector2(0, 0), 1f, RotateMode.Fast);
        }

    }
}
