using UnityEngine;
using DG.Tweening;
using System.Collections;
namespace PubScale.SdkOne.NativeAds.Hightower
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private BoxCollider2D leftWall;
        [SerializeField] private BoxCollider2D rightWall;
        [SerializeField] private BoxCollider2D bottomWall;
        private bool followX;
        private Transform target;
        private bool canFollow = true;
        private Vector3 velocity;
        private float xpos;
        private Vector3 offset;
        private Vector3 destination;
        private Vector3 finalPos;
        private Camera cam;
        private Vector3 preViousPos;

        public float BackThrehold;

        private void Awake()
        {
            cam = Camera.main;
        }
        public void ResetCam()
        {
            preViousPos = transform.position;
            //   transform.position = new Vector3(target.position.x, transform.position.y, transform.position.z);
        }
        public void InitCam(Transform target)
        {
            leftWall.size = new Vector2(1f, cam.ScreenToWorldPoint(new Vector3(0f, Screen.height * 4f, 0)).y);
            leftWall.offset = new Vector2(cam.ScreenToWorldPoint(Vector3.zero).x - (0.5f + transform.position.x), 0f);
            rightWall.size = new Vector2(1f, cam.ScreenToWorldPoint(new Vector3(0f, Screen.height * 4f, 0)).y);
            rightWall.offset = new Vector2(cam.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x + (0.5f - transform.position.x), 0f);
            bottomWall.size = new Vector2(cam.ScreenToWorldPoint(new Vector3(0f, Screen.height * 4.5f, 0)).y, 2);
            bottomWall.offset = new Vector2(0f, cam.ScreenToWorldPoint(new Vector3(0f, 0, 0)).y - 1f);
            followX = false;
            this.target = target;
            //transform.position = new Vector3(target.position.x, target.position.y + 2, -10);
            offset = target.position - transform.position;
            destination = target.position - offset;
            xpos = destination.x;
            canFollow = true;
        }
        public void LockCam(bool Horizontal, Vector3 centre)
        {
            preViousPos = transform.position;
            if (Horizontal)
                speed = 0.1f;
            else
            {
                canFollow = false;
                leftWall.enabled = false;
                rightWall.enabled = false;
                centre= new Vector3(centre.x-1.5f, centre.y, transform.position.z);
                transform.DOMoveX(centre.x, 1).OnComplete(() =>
                {
                    leftWall.enabled = true;
                    rightWall.enabled = true;
                    canFollow = true;
                    speed = 0.1f;
                    xpos = centre.x;
                });

            }
            followX = Horizontal;


        }
        public void DisableBounds()
        {
            leftWall.enabled = false;
            rightWall.enabled = false;
        }
        public void DelayEnableBounds()
        {
            leftWall.enabled = false;
            rightWall.enabled = false;
            StartCoroutine(DelayEnable());
        }
        IEnumerator DelayEnable()
        {
            yield return new WaitForSeconds(1f);
            leftWall.enabled = true;
            rightWall.enabled = true;
        }
        private void LateUpdate()
        {
            if (target != null && canFollow)
            {
                destination = target.position - offset;
                if (!followX)
                {
                    if (destination.y < transform.position.y)
                        return;
                    finalPos = new Vector3(xpos, destination.y, transform.position.z);

                }
                else
                {
                    float myPos = transform.position.y;
                    if (destination.y > transform.position.y)
                        myPos = destination.y;
                    finalPos = new Vector3(destination.x, myPos, transform.position.z);

                }
                transform.position = Vector3.SmoothDamp(transform.position, finalPos, ref velocity, speed);
                //if (destination.y > transform.position.y)
                //    preViousPos = transform.position;
            }
        }
    }
}