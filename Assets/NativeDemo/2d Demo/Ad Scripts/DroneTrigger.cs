using System;
using UnityEngine;

namespace PubScale.SdkOne.NativeAds.Sample
{
    /// <summary>
    /// Used to activate/diactivate drone unity when player is nearby
    /// </summary>
    public class DroneTrigger : MonoBehaviour
    {
        public bool CanFollow;
        public static event Action<bool> DroneState;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                DroneState?.Invoke(CanFollow);
            }
        }
    }
}