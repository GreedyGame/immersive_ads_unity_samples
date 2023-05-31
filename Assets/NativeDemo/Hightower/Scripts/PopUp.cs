namespace Epicplay
{
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;
    using System;
    using DG.Tweening;
    public class PopUp : MonoBehaviour
    {
        [SerializeField] private GameObject Holder, closeBtn;
        [SerializeField] private Image InfoImage;
        [SerializeField] private Image LoadingImg;
        [SerializeField] private TextMeshProUGUI InfoTxt;
        [SerializeField] private TextMeshProUGUI YesTxt, NoTxt;
        [SerializeField] private TextMeshProUGUI HeadingText;
        [SerializeField] private Button YesBtn, NoBtn;
        public static PopUp i;
        private void Awake()
        {
            if (i == null)
                i = this;
            else
                Destroy(i);
        }
        public void ShowPopUp(string message, Sprite infoSprite = null, Action YesAction = null, string yesText = null, Action NoAction = null, string NoText = null, bool canClose = true, string HeadingTxt = null,bool loadType=false)
        {
            Holder.SetActive(true);
            //if (!Holder.activeInHierarchy)
            //    DGAnimationController.AnimatePanel(Holder, true);
            closeBtn.SetActive(canClose);
            if (HeadingTxt != null)
                HeadingText.text = HeadingTxt;
            else
                HeadingText.text = "Alert!";
            if (infoSprite == null)
            {
                InfoImage.gameObject.SetActive(false);
                Holder.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(800, 400);
            }
            else
            {
                InfoImage.gameObject.SetActive(true);
                Holder.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(800, 700);
                InfoImage.sprite = infoSprite;
            }
            InfoTxt.text = message;
            YesBtn.onClick.RemoveAllListeners();
            NoBtn.onClick.RemoveAllListeners();
            NoBtn.onClick.AddListener(() => { ClosePop(); });
            YesBtn.onClick.AddListener(() => { ClosePop(); });
            if (yesText != null)
            {
                YesBtn.onClick.AddListener(() => { YesAction?.Invoke(); });
                YesBtn.gameObject.SetActive(true);
                YesTxt.text = yesText;
            }
            else
            {
                YesBtn.gameObject.SetActive(false);
            }
            if (NoText != null)
            {
                NoBtn.onClick.AddListener(() => { NoAction?.Invoke(); });
                NoBtn.gameObject.SetActive(true);
                NoTxt.text = NoText;
            }
            else
            {
                NoBtn.gameObject.SetActive(false);
            }
            if(loadType)
            {
                Holder.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(800, 700);
                LoadingImg.transform.parent.gameObject.SetActive(true);
                Load();
            }
            else
            {
                Holder.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(800, 400);
                LoadingImg.transform.parent.gameObject.SetActive(false);
            }
        }
        public void Load()
        {
            LoadingImg.DOFillAmount(0, 1.5f).From(1).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo).OnStepComplete(() =>
            {
                LoadingImg.fillClockwise = !LoadingImg.fillClockwise;
            }).SetId("Load1").SetUpdate(true);
        }
        public void ClosePop()
        {
            DOTween.Kill("Load1");
            Holder.SetActive(false);
          //  DGAnimationController.AnimatePanel(Holder, false);
        }
    }
}   
