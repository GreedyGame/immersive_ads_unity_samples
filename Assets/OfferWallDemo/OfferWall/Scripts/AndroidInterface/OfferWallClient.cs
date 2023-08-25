using System;
using UnityEngine;
using Pubscale.Common;

namespace Pubscale.OfferWall
{
    public class OfferWallClient : AndroidJavaProxy, IBaseClient
    {
        private AndroidJavaObject offerWallClient;

        public event Action OnOfferWallShowed;
        public event Action OnOfferWallClosed;
        public event Action<string, string> OnRewardClaimed;
        public event Action<string> OnFailed;
        public event Action OnInitSuccess;
        public event Action<string> OnInitFailed;
        public event Action<string> OnDataEncrypted;
        public event Action<string> OnAppographyDataFetched;

        public OfferWallClient() : base(Utils.OfferWallListenerClassName)
        {
            AndroidJavaClass playerClass = new AndroidJavaClass(Utils.UnityActivityClassName);
            AndroidJavaObject activity = playerClass.GetStatic<AndroidJavaObject>("currentActivity");
            this.offerWallClient = new AndroidJavaObject(Utils.OfferWallClassName, activity, this);
        }
        #region OfferWallClass methods.

        public void InitOfferWall(string uniqueId, string appKey, int orientation, bool isDev)
        {
            if(this.offerWallClient == null)
            {
                Debug.Log("OfferWallClient is null");
                return;
            }
            this.offerWallClient.Call("InitOfferWall", uniqueId, appKey, orientation, isDev);
        }
        public void ShowOfferWall()
        {
            if (this.offerWallClient == null)
            {
                Debug.Log("OfferWallClient is null");
                return;
            }
            this.offerWallClient.Call("ShowOfferWall");
        }
        public void DisposeOfferWall()
        {
            if (this.offerWallClient == null)
            {
                Debug.Log("OfferWallClient is null");
                return;
            }
            this.offerWallClient.Call("DisposeOfferWall");

        }
        public void EncryptData(string key, string json)
        {
            if (this.offerWallClient == null)
            {
                Debug.Log("OfferWallClient is null");
                return;
            }
            this.offerWallClient.Call("EncryptJson", key,json);
        }
        public void GetAppographyData()
        {
            if (this.offerWallClient == null)
            {
                Debug.Log("OfferWallClient is null");
                return;
            }
            this.offerWallClient.Call("GetAppographyData");
        }
        #endregion

        #region Callbacks from Offer Wall Listner.
        public void onOfferWallShowed()
        {
            OnOfferWallShowed?.Invoke();
        }
        public void onOfferWallClosed()
        {
            OnOfferWallClosed?.Invoke();
        }
        public void onRewardClaimed(string amount, string currency)
        {
            OnRewardClaimed?.Invoke(amount, currency);
        }
        public void onFailed(string cause)
        {
            OnFailed?.Invoke(cause);
        }

        public void onInitSuccess()
        {
            OnInitSuccess?.Invoke();
        }
        public void onInitFailed(string initError)
        {
            OnInitFailed?.Invoke(initError);
        }

        public void onDataEncrypted(string encryptedData)
        {
            OnDataEncrypted?.Invoke(encryptedData);
        }

        public void onAppographyDataFetched(string appographyData)
        {
            OnAppographyDataFetched?.Invoke(appographyData);
        }


        #endregion
    }
}