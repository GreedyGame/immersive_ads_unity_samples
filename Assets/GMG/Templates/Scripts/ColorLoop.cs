using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ColorLoop : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Color startColor;
    [SerializeField] private Color endColor;
    [SerializeField] private float duraction=0.2f;

    private void OnEnable()
    {

        image.DOColor(endColor, duraction).From(startColor).SetLoops(-1, LoopType.Yoyo);
    }
}
