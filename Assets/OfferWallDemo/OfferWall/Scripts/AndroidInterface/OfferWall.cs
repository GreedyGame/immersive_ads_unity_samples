using System;
using UnityEngine;

namespace PubScale.OfferWall
{
    public class OfferWall
    {
        private OfferWallClient client;
        private string appKey;
        private string uniqueId;
        private bool isFullScreen;
        private bool isDev;
        private string bgImagePath;

        public event Action OnOfferWallShowed;
        public event Action OnOfferWallClosed;
        public event Action<string, string> OnOfferWallRewardClaimed;
        public event Action<string> OnOfferWallShowFailed;
        public event Action OnOfferWallInitSuccess;
        public event Action<string> OnOfferWallInitFailed;
        //public event Action<string> OnDataEncrypted;
        //public event Action<string> OnAppographyDataRecieved;

        // Creates an OfferWall Instance.
        public OfferWall(string appKey, string uniqueId,string bgImagePath,bool isFullScreen, bool isDev)
        {
            this.client = new OfferWallClient();
            this.appKey = appKey;
            this.uniqueId = uniqueId;
            this.isFullScreen = isFullScreen;
            this.isDev = isDev;
            this.bgImagePath = bgImagePath;
            ConfigureInterEvents();
        }

        // Loads an Reward.
        public void InitOfferWall()
        {
            client.InitOfferWall(appKey,uniqueId,bgImagePath, isFullScreen, isDev);
        }
        public void ShowOfferWall()
        {
            client.ShowOfferWall();
        }
        public void DisposeOfferWall()
        {
            client.DisposeOfferWall();
        }
        //public void EncryptData(string key, string json)
        //{
        //    client.EncryptData(key, json);
        //}
        //public void GetAppographyData()
        //{
        //    client.GetAppographyData();
        //}
        private void ConfigureInterEvents()
        {
            this.client.OnOfferWallShowed += () =>
            {
                Debug.Log("OnOfferWallShowed");
                OnOfferWallShowed?.Invoke();
            };
            this.client.OnOfferWallClosed += () =>
            {
                Debug.Log("OnOfferWallClosed");
                OnOfferWallClosed?.Invoke();
            };
            this.client.OnOfferWallRewardClaimed += (amount, currency) =>
            {
                Debug.Log("OnRewardClaimed");
                OnOfferWallRewardClaimed?.Invoke(amount, currency);
            };
            this.client.OnOfferWallShowFailed += (cause) =>
            {
                Debug.Log("OnFailed");
                OnOfferWallShowFailed?.Invoke(cause);
            };
            this.client.OnOfferWallInitSuccess += () =>
            {
                Debug.Log("OnInitSuccess");
                OnOfferWallInitSuccess?.Invoke();
            };
            this.client.OnOfferWallInitFailed += (initError) =>
            {
                Debug.Log("OnInitFailed");
                OnOfferWallInitFailed?.Invoke(initError);
            };
            //this.client.OnDataEncrypted += (encryptedData) =>
            //{
            //    Debug.Log("OnDataEncrypted");
            //    OnDataEncrypted?.Invoke(encryptedData);
            //};
            //this.client.OnAppographyDataFetched += (appographyData) =>
            //{
            //    Debug.Log("OnAppographyDataRecieved");
            //    OnAppographyDataRecieved?.Invoke(appographyData);
            //};
        }

    }
}