using System;
using System.Collections.Generic;
using PubScale.SdkOne.NativeAds;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreenTemplate : MonoBehaviour
{
    public Slider slider;
    public float sliderTime = 15;
    public Button btn_openLoadingPanel;
    public Button btn_closeLoadingPanel;

    public NativeAdHolder nativeAdHolder;
    public RectTransform backPanel;
    private RectTransform backPanelHolder;
    public RectTransform bigImage;
    

    void Start()
    {
        slider.maxValue = sliderTime;
        btn_openLoadingPanel.onClick.AddListener(ReloadSlider);
        btn_closeLoadingPanel.onClick.AddListener(CloseLoadingPanel);
        Invoke(nameof(EnableSlider), 1f);

        backPanelHolder = backPanel.parent.GetComponent<RectTransform>();
        nativeAdHolder.Event_AdRendered += OnAdRendered;

        Debug.Log(backPanelHolder.sizeDelta.x);
    }

    private void OnAdRendered(Sprite sprite, List<Texture2D> list, string arg3)
    {
        // throw new NotImplementedException();
        Debug.Log("Total Rendered textures :" + list.Count);
        Debug.Log("Rendered texture width : " + list[0].width +" --"+ "Rendered texture height :" + list[0].height );
        // backPanel.sizeDelta = new Vector2(backPanelHolder.sizeDelta.x - 20, backPanelHolder.sizeDelta.y - 20);
        // backPanelHolder.sizeDelta = new Vector2(bigImage.sizeDelta.x + 20, bigImage.sizeDelta.y + 20);
        Debug.Log("big Image width : " + bigImage.sizeDelta.x +" --"+ "Big Image height :" + bigImage.sizeDelta.y);
        // backPanel.localScale = n
    }

    void Update()
    {
        if(slider != null && slider.gameObject.activeInHierarchy)
        {
            slider.value += Time.deltaTime;

            if(slider.value >= slider.maxValue)
            {
                slider.value = 0;
            }
        }
    }

    private void OpenLoadingPanel()
    {
        this.gameObject.SetActive(true);
    }
    private void CloseLoadingPanel()
    {
        this.gameObject.SetActive(false);
    }

    public void ReloadSlider()
    {
        CloseLoadingPanel();
        Invoke(nameof(OpenLoadingPanel), .5f);
    }

    public void EnableSlider()
    {
        slider.gameObject.SetActive(true);
    }
    public void DisableSlider()
    {
        slider.gameObject.SetActive(false);
    }
}
