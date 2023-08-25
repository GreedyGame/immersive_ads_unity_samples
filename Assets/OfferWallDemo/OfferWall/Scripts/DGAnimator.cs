using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace PubScale.SdkOne.Common
{

public enum  AnimationType
{
    Rotate,Move,Scale
}
public class DGAnimator : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private AnimationType animationType;
    [SerializeField] private Vector3 startValue;
    [SerializeField] private Vector3 endValue;
    [SerializeField] private float duration;
    [SerializeField] private Ease ease;
    [SerializeField] private LoopType loopType;
    [SerializeField] private int loops;
    [SerializeField] private UnityEvent OnComplete;
    [SerializeField] private UnityEvent OnStepComplete;
    private void Awake()
    {
        Tween tween=null;
        switch (animationType)
        {
            case AnimationType.Rotate:
                tween = target.transform.DORotate(endValue, duration,RotateMode.FastBeyond360).From(startValue);
                break;
            case AnimationType.Move:
                target.TryGetComponent(out RectTransform rectTransform);
                if (rectTransform != null)
                    tween = rectTransform.DOAnchorPos(endValue, duration).From(startValue);
                else
                    tween = target.transform.DOMove(endValue, duration).From(startValue);
                break;
            case AnimationType.Scale:
                break;
        }
        if (tween == null)
            return;
        tween.SetEase(ease).SetLoops(loops, loopType).SetId(animationType.ToString() + gameObject.GetInstanceID());
        tween.OnComplete(() => OnComplete?.Invoke());
        tween.OnStepComplete(() => OnStepComplete?.Invoke());
        tween.Play();
    }
}

}