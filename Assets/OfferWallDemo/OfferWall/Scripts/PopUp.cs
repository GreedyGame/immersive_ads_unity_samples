using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopUp : MonoBehaviour
{
    [SerializeField] private Image bgTint;
    [SerializeField] private RectTransform holder;
    [SerializeField]  private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI message;
    [SerializeField] private Button okButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private GameObject offerCompleteImg;
    [SerializeField] private GameObject offerNotCompleted;

    private void Awake()
    {
        okButton.onClick.RemoveAllListeners();
        cancelButton.onClick.RemoveAllListeners();
       closeButton.onClick.RemoveAllListeners();
        offerCompleteImg.SetActive(false);
        offerNotCompleted.SetActive(false);

        okButton.onClick.AddListener(()=>SetPopupState(false));
        cancelButton.onClick.AddListener(()=>SetPopupState(false));
        closeButton.onClick.AddListener(()=>SetPopupState(false));
    }
    public void SetPopupState(bool state)
    {
        if (state)
        {
            bgTint.gameObject.SetActive(true);
            holder.gameObject.SetActive(true);
            bgTint.DOFade(0.8f, 0.2f).From(0);
            holder.DOScale(0.8f, 0.2f).From(Vector2.zero);
        }
        else
        {
            bgTint.DOFade(0, 0.2f).From(0.8f);
            holder.DOScale(0, 0.2f).From(0.8f).OnComplete(()=>{
                bgTint.gameObject.SetActive(false);
                holder.gameObject.SetActive(false);
            });
        }
    }

    public void ShowCompleteState(string rewardAmount)
    {
        offerCompleteImg.SetActive(true);
        offerNotCompleted.SetActive(false);
        SetPopupState(true);
        message.text= $"You have earned {rewardAmount} coins";
        title.text = "Congratulations";
        okButton.GetComponentInChildren<TextMeshProUGUI>().text = "OK";
    }
    public void ShowNotCompleteState()
    {
        offerCompleteImg.SetActive(false);
        offerNotCompleted.SetActive(true);
        SetPopupState(true);
        message.text= $"Uh Oh You Did't\n completed any offer";
        title.text = "Sorry";
        okButton.GetComponentInChildren<TextMeshProUGUI>().text = "Let us know why";
    }
}
