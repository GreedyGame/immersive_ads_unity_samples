using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class RectAnimation : MonoBehaviour
{
    [SerializeField] private Vector2 startPos;
    [SerializeField] private Vector2 endPos;
    [SerializeField] private float moveSpeed = 1;
    [SerializeField] private RectTransform targetRect;
    [SerializeField] private UnityEvent onComplete; 

    private void Awake()
    {
        targetRect.gameObject.SetActive(true);
        targetRect.DOAnchorPos(endPos, moveSpeed).From(startPos).OnComplete(()=> {
            onComplete?.Invoke();
        });
    }
}
