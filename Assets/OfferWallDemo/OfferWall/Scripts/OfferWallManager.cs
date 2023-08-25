using Pubscale.Common;
using System;
using UnityEngine;

namespace Pubscale.OfferWall
{
    public enum OfferWallOrientation
    {
        PORTRAIT = 0,
        LANDSCAPE = 1
    }
    public class OfferWallManager : MonoBehaviour
    {
        public static OfferWallManager instance;
        public static event Action OnInitialize;
        public static event Action<string> OnInitializeFailed;
        public static event Action OnOfferWallShown;
        public static event Action OnOfferWallClosed;
        public static event Action<float> OnOfferClaimed;
        public static event Action<string> OnOfferClaimFail;
        private OfferWall offerWall;
        private float amountEarned;
        private static Action<bool, float> UnlockRewardAction;
        public bool IsInitialized = false;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this);
            }
        }
        public static void Initialize(string appKey, string uniqueID, OfferWallOrientation orientation, bool DebugMode = false)
        {
            if (instance == null)
            {
                Debug.LogError("Offer wall manager is not initiated");
                return;
            }
            if (string.IsNullOrEmpty(appKey))
            {
                Debug.LogError("App key is null or empty");
                return;
            }
            if (string.IsNullOrEmpty(uniqueID))
            {
                Debug.LogError("Unique ID is null or empty");
                return;
            }

            instance.InitializePlugin(appKey, uniqueID, orientation, DebugMode);
        }
        void InitializePlugin(string appKey, string uniqueID,OfferWallOrientation orientation, bool DebugMode = false)
        {
            amountEarned = 0;
            UnityMainThreadDispatcher.Initialize();
#if UNITY_EDITOR 
            return;
#endif
#pragma warning disable CS0162 // Unreachable code detected
            InitializeOfferWall(appKey,uniqueID,orientation == OfferWallOrientation.PORTRAIT ? 0 : 1, DebugMode);
#pragma warning restore CS0162 // Unreachable code detected
        }
     
        public void InitializeOfferWall(string appKey, string uniqueID, int orientation, bool DebugMode = false)
        {
            offerWall = new OfferWall(appKey, uniqueID, orientation, DebugMode);
            offerWall.InitOfferWall();
            offerWall.OnInitSuccess += () =>
            {
                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {
                    IsInitialized = true;
                    OnInitialize?.Invoke();
                    Log("Offer wall init success");
                });
            };
            offerWall.OnInitFailed += (cause) =>
            {
                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {
                    IsInitialized=false;
                    OnInitializeFailed?.Invoke(cause);
                    Log($"Offer wall init failed cause {cause}");

                });
            };
            offerWall.OnOfferWallShowed += () =>
            {
                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {
                    amountEarned = 0;
                    OnOfferWallShown?.Invoke();
                    Log("Offer wall Showed");
                });
            };
            offerWall.OnOfferWallClosed += () =>
            {
                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {
                    OnOfferWallClosed?.Invoke();
                    Log("Offer wall closed");

                    if (amountEarned != 0)
                    {
                        UnlockRewardAction?.Invoke(true, amountEarned);
                    }
                    else
                    {
                        UnlockRewardAction?.Invoke(false, amountEarned);
                    }
                });
            };
            offerWall.OnRewardClaimed += (amount, currency) =>
            {
                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {
                    float.TryParse(amount, out amountEarned);
                    OnOfferClaimed?.Invoke(amountEarned);
                    Log($"Offer wall reward claimed amount {amount} and currency {currency}");

                });
            };
            offerWall.OnFailed += (cause) =>
            {
                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {
                    OnOfferClaimFail?.Invoke(cause);
                    Log($"Offer wall offer failed cause {cause}");
                });
            };
        }
        public static void UnlockReward(Action<bool, float> action)
        {
            UnlockRewardAction = action;
            instance.ShowOfferWall();
        }
        public static void OpenOfferWall()
        {
            if (instance == null)
            {
                Debug.LogError("Offer wall manager is not initiated");
                return;
            }
            instance.ShowOfferWall();
        }
        public void ShowOfferWall()
        {
            if (offerWall == null)
            {
                Log("OfferWall is not Initialized");
                return;
            }
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                offerWall.ShowOfferWall();
            });
        }
        public void DisposeOfferWall()
        {
            if (offerWall == null)
            {
                Log("OfferWall is not Initialized");
                return;
            }
            offerWall.DisposeOfferWall();
        }
        public void Log(string log)
        {
            Debug.Log(log);
        }
    }
}