using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
namespace PubScale.SdkOne.NativeAds.Hightower
{
    public static class DGAnimationController
    {
        public static bool canUse = false;
        public static void AnimateBgPanel(GameObject panel, bool result, Action Complete = null, Action start = null)
        {
            start?.Invoke();
            if (result)
            {
                panel.SetActive(true);
                // panel.transform.GetComponent<CanvasGroup>().blocksRaycasts = false;
                panel.transform.GetComponent<CanvasGroup>().DOFade(1, 0.5f).From(0).SetUpdate(true).OnComplete(() =>
                {
                    //panel.transform.GetComponent<CanvasGroup>().blocksRaycasts = true;
                    Complete?.Invoke();
                }).SetId(panel.GetInstanceID().ToString());
                panel.transform.GetChild(0).GetComponent<RectTransform>().DOAnchorPosY(0, 0.5f).From(Vector2.down * -2000).SetUpdate(true).SetId("1" + panel.GetInstanceID().ToString()); ;
            }
            else
            {
                // panel.transform.GetComponent<CanvasGroup>().blocksRaycasts = false;
                panel.transform.GetChild(0).GetComponent<RectTransform>().DOAnchorPosY(-2000, 0.5f).From(Vector2.zero).SetUpdate(true).SetId("1" + panel.GetInstanceID().ToString()); ;
                panel.transform.GetComponent<CanvasGroup>().DOFade(0, 0.5f).From(1).SetUpdate(true).OnComplete(() =>
                {
                    Complete?.Invoke();
                    panel.SetActive(false);
                }).SetId(panel.GetInstanceID().ToString()); ;
            }
        }
        public static void KillAnimation(GameObject g)
        {
            DOTween.Kill(g.GetInstanceID().ToString());
            DOTween.Kill("1" + g.GetInstanceID().ToString());
        }
        public static void EnablePanel(GameObject panel, bool ans)
        {
            if (ans)
            {
                panel.SetActive(true);
                panel.transform.GetComponent<CanvasGroup>().blocksRaycasts = true;
                panel.transform.GetComponent<CanvasGroup>().alpha = 1;
                panel.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            }
            else
            {
                panel.SetActive(false);
            }
        }
        public static void PopInOut(GameObject panel, bool result)
        {
            if (result)
            {
                panel.SetActive(true);
                panel.transform.DOScale(1, 0.3f).From(0).SetEase(Ease.InBounce);
            }
            else
            {
                panel.transform.DOScale(0, 0.3f).From(1).SetEase(Ease.OutBounce).OnComplete(() => { panel.SetActive(false); });
            }
        }
        public static void AnimatePanel(GameObject window, bool open, Action openA = null, Action closeA = null)
        {

            if (open)
            {
                window.SetActive(true);
                window.GetComponent<Image>().DOFade(0.8f, 0.4f).From(0).OnComplete(() =>
                {
                    openA?.Invoke();
                });
                window.transform.GetChild(0).transform.DOScale(1, 0.4f).From(0);

            }
            else
            {
                window.GetComponent<Image>().DOFade(0, 0.5f).From(0.8f).OnComplete(() =>
                {
                    closeA?.Invoke();
                    window.SetActive(false);
                });
                window.transform.GetChild(0).transform.DOScale(0, 0.4f).From(1);
            }
        }
    }
}