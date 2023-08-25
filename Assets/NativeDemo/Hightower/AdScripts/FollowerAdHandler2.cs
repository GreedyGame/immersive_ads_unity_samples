
using PubScale.SdkOne.NativeAds.Hightower;
using UnityEngine;
namespace PubScale.SdkOne.NativeAds.Hightower
{
    /// <summary>
    /// Follows the target with given offset
    /// </summary>
    public class FollowerAdHandler2 : MonoBehaviour
    {
        [SerializeField] private NativeAdHolder nativeAdHolder;
        [SerializeField] private Transform targetTransform;
        [SerializeField] private float smoothTime = 0.3f;
        [SerializeField] private Vector3 offset = Vector3.zero;
        [SerializeField] private bool IgnoreX = false;
        [SerializeField] private GameObject smoke;

        private Vector3 velocity = Vector3.zero;
        private bool canFollow = false;
        private bool adLoaded;
        private bool gotFollowRequest;
        private float Xpos;

        private void Awake()
        {
            nativeAdHolder.Event_AdLoaded += NativeAdHolder_Event_AdLoaded; //Subscribe to native ad loaded event
            nativeAdHolder.Event_AdFailed += NativeAdHolder_Event_AdFailed; //Subscribe to native ad failed event
            GameManager.DroneState += DroneTrigger_DroneState;             //Subscribe to drone trigger event 
        }

        private void OnDestroy()
        {
            nativeAdHolder.Event_AdFailed -= NativeAdHolder_Event_AdFailed;  //Unsubscribe to native ad loaded event
            nativeAdHolder.Event_AdLoaded -= NativeAdHolder_Event_AdLoaded;  //Unsubscribe to native ad failed event
            GameManager.DroneState -= DroneTrigger_DroneState;              //Unsubscribe to drone trigger event 
        }

        private void NativeAdHolder_Event_AdFailed(object arg1, GoogleMobileAds.Api.AdFailedToLoadEventArgs arg2)
        {
            //if (gotFollowRequest)
            //    canFollow = false;
        }
        private void NativeAdHolder_Event_AdLoaded(object arg1, GoogleMobileAds.Api.NativeAdEventArgs arg2)
        {
            adLoaded = true;
            if (gotFollowRequest)
                canFollow = true;
        }

        private void DroneTrigger_DroneState(bool obj,Transform transform)
        {
            Xpos = transform.position.x + offset.x;
            gotFollowRequest = obj;
#if UNITY_EDITOR
            canFollow = obj;
#endif
            if (!adLoaded)
                return;
            smoke.SetActive(obj);
            canFollow = obj;
        }

  
        private void FixedUpdate()
        {
            if (!canFollow)
                return;
            if (targetTransform != null)
            {
                Vector3 targetPosition = targetTransform.position + offset;
                if (IgnoreX)
                    targetPosition = new Vector3(Xpos, targetPosition.y, targetPosition.z);
                transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime); //Follows the target with given offset
            }
        }
    }
}