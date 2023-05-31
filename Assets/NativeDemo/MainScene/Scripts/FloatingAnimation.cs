using DG.Tweening;
using UnityEngine;

public class FloatingAnimation : MonoBehaviour
{
    [SerializeField] private float strength=5;
    [SerializeField] private float time = 1;
    [SerializeField] private Ease ease;
  private RectTransform rectTransform;

    private void OnEnable()
    {
        TryGetComponent<RectTransform>(out rectTransform);
        if (rectTransform != null)
            rectTransform.DOAnchorPosY(rectTransform.anchoredPosition.y + strength, time).From(rectTransform.anchoredPosition).SetEase(ease).SetDelay(Random.Range(0,2f)).SetLoops(-1, LoopType.Yoyo).SetId("float" + gameObject.GetInstanceID());
    }
    private void OnDestroy()
    {
        DOTween.Kill("float" + gameObject.GetInstanceID());
    }
}
