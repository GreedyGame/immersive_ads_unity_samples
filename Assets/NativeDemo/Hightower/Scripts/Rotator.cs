using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PubScale.SdkOne.NativeAds.Hightower
{
    public class Rotator : MonoBehaviour
    {
        private void Awake()
        {
            transform.DORotate(Vector3.right * 360, 0.2f, RotateMode.FastBeyond360).From(Vector3.zero).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental).SetId("jump4" + gameObject.GetInstanceID());
        }
        private void OnDestroy()
        {
            DOTween.Kill("jump4" + gameObject.GetInstanceID());
        }
    }
}