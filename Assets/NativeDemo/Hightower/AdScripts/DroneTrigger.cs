using System;
using UnityEngine;

namespace PubScale.SdkOne.NativeAds.Hightower
{
    /// <summary>
    /// Used to activate/diactivate drone unity when player is nearby
    /// </summary>
    public class DroneTrigger : MonoBehaviour
    {
        public bool CanFollow;

        //private void OnTriggerEnter2D(Collider2D collision)
        //{
        //    if (collision.CompareTag("Player"))
        //    {
        //        DroneState?.Invoke(CanFollow);
        //    }
        //}
    }
}