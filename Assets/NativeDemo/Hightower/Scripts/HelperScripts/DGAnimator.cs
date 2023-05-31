using DG.Tweening;
using UnityEngine;
namespace PubScale.SdkOne.NativeAds.Hightower
{
    public class DGAnimator : MonoBehaviour
    {
        public bool isJump = true;
        public bool isScale = false;
        public bool isRotating = false;
        public bool isRotatingY = false;
        public Animator anim;
        public AudioSource src;
        public ParticleSystem particle;

        public Vector2 OrgPos { get; private set; }

        private void Awake()
        {
            if (isJump)
                OrgPos = GetComponent<RectTransform>().anchoredPosition;
        }
        private void OnEnable()
        {
            if (isJump)
                transform.GetComponent<RectTransform>().DOAnchorPosY(30, 0.5f).From(OrgPos).SetEase(Ease.InBounce).SetLoops(-1, LoopType.Yoyo).SetId("jump" + gameObject.GetInstanceID());
            else if (isScale)
                transform.DOScale(Vector3.one * 1.1f, 0.5f).From(Vector3.one).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo).SetId("jump2" + gameObject.GetInstanceID());
            else if (isRotating)
                transform.DORotate(Vector3.forward * 360, 1, RotateMode.FastBeyond360).From(Vector3.zero).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental).SetId("jump3" + gameObject.GetInstanceID());
            else if (isRotating)
                transform.DORotate(Vector3.right * 360, 1, RotateMode.FastBeyond360).From(Vector3.zero).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental).SetId("jump4" + gameObject.GetInstanceID());
        }
        private void OnDisable()
        {
            if (isJump)
                DOTween.Kill("jump" + gameObject.GetInstanceID());
            else if (isScale)
                DOTween.Kill("jump2" + gameObject.GetInstanceID());
            else if (isRotating)
                DOTween.Kill("jump3" + gameObject.GetInstanceID());
            else if (isRotatingY)
                DOTween.Kill("jump4" + gameObject.GetInstanceID());
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                anim.Play("bush");
                src.Play();
                particle.Play();
                //  transform.DOShakeScale(0.2f, strength: 0.5f);
            }
        }
    }
}