using DG.Tweening;
using UnityEngine;

public class ScaleAnimation : MonoBehaviour
{
    [SerializeField] private Transform targetObj;
    [SerializeField] private float startScaleValue = 1;
    [SerializeField] private float endScaleValue = 1;
    [SerializeField] private float scaleSpeed = 1;
    [SerializeField] private float DelayScale = 1;

    public void ScaleObj()
    {
        targetObj.gameObject.SetActive(true);
        targetObj.DOScale(endScaleValue, scaleSpeed).From(startScaleValue).SetEase(Ease.InBounce).SetDelay(DelayScale);
    }
}
