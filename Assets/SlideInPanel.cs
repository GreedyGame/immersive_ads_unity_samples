using DG.Tweening;
using PubScale.SdkOne.NativeAds;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PubScale.SdkOne.NativeAds
{
public class SlideInPanel : MonoBehaviour
{
    [SerializeField] float initPosition = -800f;
    [SerializeField] float endPosition = -800f;
    [SerializeField] GameObject element1;
    [SerializeField] GameObject bigImageCarousel;
    // [SerializeField] GameObject element2;

    [SerializeField] float iterationsPerRefresh = 4;
    private float durationPerIteration;

    private const float DURATION_WHEN_NO_REFRESH = 6;

    private List<Texture2D> bigImageTextures = new();

    public List<Image> bigImages = new();

    private const int BIG_IMAGE_LIMIT = 3;

    private void Start() 
    {
        transform.DOLocalMoveX(endPosition, 1);
    }
    private void OnEnable()
    {
        GetBigImageSprites();
        // transform.DOLocalMoveX(endPosition, 1);
        transform.localPosition = new Vector2(endPosition, transform.localPosition.y);

        // float refreshDelay = GetComponentInParent<NativeAdHolder>().RefreshDelay;
        bigImageCarousel.transform.localPosition = new Vector2(initPosition, bigImageCarousel.transform.localPosition.y);

        // if(refreshDelay > 0)
        //     durationPerIteration =  refreshDelay / iterationsPerRefresh;
        // else 
        durationPerIteration = DURATION_WHEN_NO_REFRESH;

        StartCoroutine(SlideIn());

    }

    private void GetBigImageSprites()
    {
        // bigImageTextures = GetComponentInParent<NativeAdHolder>().adDisplay.BigMediaImages;

        if(bigImageTextures == null || bigImageTextures.Count <= 0) return;

        foreach (Texture2D texture in bigImageTextures)
        {
            ConvertTextureToSprite(texture, (sprite) =>
            {
                for (int i = 0; i < bigImages.Count; i++)
                {
                    bigImages[i].sprite = sprite;
                }
            });
        }
    }

    private void ResetCarousel()
    {
        bigImageCarousel.SetActive(false);
        bigImageCarousel.transform.localPosition = new Vector2(initPosition, bigImageCarousel.transform.localPosition.y);
    }

    IEnumerator SlideIn()
    {
        while(true)
        {
            Debug.Log(durationPerIteration + " : duration Per Iteration");
            bigImageCarousel.SetActive(true);
            element1.transform.DOLocalMoveX(0, 2);
            yield return new WaitForSeconds(durationPerIteration);
            bigImageCarousel.transform.localPosition = new Vector2(initPosition, bigImageCarousel.transform.localPosition.y);
            element1.transform.DOLocalMoveX(-initPosition, 2);

            for (int i = 0; i < 3; i++)
            {
                bigImageCarousel.transform.DOLocalMoveX(initPosition * -i, 2);//.OnComplete(()=>bigImageCarousel.transform.DOLocalMoveX(-800, 2).OnComplete(()=>bigImageCarousel.transform.DOLocalMoveX(-1600, 1)).OnComplete(()=>element1.transform.DOLocalMoveX(0, 2)));
                
                if(i == 2)
                {
                    element1.transform.DOLocalMoveX(0, 2);
                }
                yield return new WaitForSeconds(2);
            }
            
               
            
            yield return new WaitForSeconds(durationPerIteration);
            element1.transform.localPosition = new Vector2(initPosition, element1.transform.localPosition.y);
            // bigImageCarousel.transform.DOLocalMoveX(-initPosition, 2);
            // element2.transform.localPosition = new Vector2(-initPosition, element2.transform.localPosition.y);
            // element1.transform.DOLocalMoveX(0, 2);
            // yield return new WaitForSeconds(3);
            // element1.transform.DOLocalMoveX(-initPosition, 2);
            // element2.transform.DOLocalMoveX(0, 2);
            // yield return new WaitForSeconds(4);
        }
        // transform.DOLocalMoveX(initPosition, 1).OnComplete(() => transform.gameObject.SetActive(false));
    }


    void ConvertTextureToSprite(Texture2D texture, Action<Sprite> OnSpriteConverted)
    {
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100, 0, meshType: SpriteMeshType.FullRect);
        OnSpriteConverted?.Invoke(sprite);
    }
}
}