using System;
using UnityEngine;
using PubScale.Common;

namespace PubScale.OfferWall
{
    public class OfferWallClient : AndroidJavaProxy, IBaseClient
    {
        private AndroidJavaObject offerWallClient;

        public event Action OnOfferWallShowed;
        public event Action OnOfferWallClosed;
        public event Action<string, string> OnOfferWallRewardClaimed;
        public event Action<string> OnOfferWallShowFailed;
        public event Action OnOfferWallInitSuccess;
        public event Action<string> OnOfferWallInitFailed;
        //public event Action<string> OnDataEncrypted;
        //public event Action<string> OnAppographyDataFetched;

        public OfferWallClient() : base(Utils.OfferWallListnerClassName)
        {
            AndroidJavaClass playerClass = new AndroidJavaClass(Utils.UnityActivityClassName);
            AndroidJavaObject activity = playerClass.GetStatic<AndroidJavaObject>("currentActivity");
            this.offerWallClient = new AndroidJavaObject(Utils.OfferWallClassName, activity, this);
        }
        #region OfferWallClass methods.

        public void InitOfferWall(string appKey, string uniqueId,string bgImagePath, bool isFullScreen,bool isDev)
        {
            this.offerWallClient.Call("InitOfferWall", appKey, uniqueId,bgImagePath,isFullScreen, isDev);
        }
        public void ShowOfferWall()
        {
            this.offerWallClient.Call("ShowOfferWall");
        }
        public void DisposeOfferWall()
        {
            this.offerWallClient.Call("DisposeOfferWall");

        }
        //public void EncryptData(string key, string json)
        //{
        //    this.offerWallClient.Call("EncryptJson", key,json);
        //}
        //public void GetAppographyData()
        //{
        //    this.offerWallClient.Call("GetAppographyData");
        //}
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
            OnOfferWallRewardClaimed?.Invoke(amount, currency);
        }
        public void onFailed(string cause)
        {
            OnOfferWallShowFailed?.Invoke(cause);
        }

        public void onInitSuccess()
        {
            OnOfferWallInitSuccess?.Invoke();
        }
        public void onInitFailed(string initError)
        {
            OnOfferWallInitFailed?.Invoke(initError);
        }

        //public void onDataEncrypted(string encryptedData)
        //{
        //    OnDataEncrypted?.Invoke(encryptedData);
        //}

        //public void onAppographyDataFetched(string appographyData)
        //{
        //    OnAppographyDataFetched?.Invoke(appographyData);
        //}


        #endregion
    }
}