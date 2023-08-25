using System;
using UnityEngine;

namespace Pubscale.OfferWall
{
    public class OfferWall
    {
        private OfferWallClient client;
        private string appKey;
        private string uniqueId;
        private int orientation;
        private bool isDev;

        public event Action OnOfferWallShowed;
        public event Action OnOfferWallClosed;
        public event Action<string, string> OnRewardClaimed;
        public event Action<string> OnFailed;
        public event Action OnInitSuccess;
        public event Action<string> OnInitFailed;
        public event Action<string> OnDataEncrypted;
        public event Action<string> OnAppographyDataRecieved;

        // Creates an OfferWall Instance.
        public OfferWall(string appKey, string uniqueId, int orientation, bool isDev)
        {
            this.client = new OfferWallClient();
            this.appKey = appKey;
            this.uniqueId = uniqueId;
            this.orientation = orientation;
            this.isDev = isDev;
            ConfigureInterEvents();
        }

        // Loads an Reward.
        public void InitOfferWall()
        {
            if (client == null)
            {
                Debug.Log("OfferWall is null");
                return;
            }
            client.InitOfferWall(uniqueId, appKey, orientation, isDev);
        }
        public void ShowOfferWall()
        {
            if (client == null)
            {
                Debug.Log("OfferWall is null");
                return;
            }
            client.ShowOfferWall();
        }
        public void DisposeOfferWall()
        {
            if (client == null)
            {
                Debug.Log("OfferWall is null");
                return;
            }
            client.DisposeOfferWall();
        }
        public void EncryptData(string key, string json)
        {
            if (client == null)
            {
                Debug.Log("OfferWall is null");
                return;
            }
            client.EncryptData(key, json);
        }
        public void GetAppographyData()
        {
            if (client == null)
            {
                Debug.Log("OfferWall is null");
                return;
            }
            client.GetAppographyData();
        }
        private void ConfigureInterEvents()
        {
            if (client == null)
            {
                Debug.Log("OfferWall is null");
                return;
            }
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
            this.client.OnRewardClaimed += (amount, currency) =>
            {
                Debug.Log("OnRewardClaimed");
                OnRewardClaimed?.Invoke(amount, currency);
            };
            this.client.OnFailed += (cause) =>
            {
                Debug.Log("OnFailed");
                OnFailed?.Invoke(cause);
            };
            this.client.OnInitSuccess += () =>
            {
                Debug.Log("OnInitSuccess");
                OnInitSuccess?.Invoke();
            };
            this.client.OnInitFailed += (initError) =>
            {
                Debug.Log("OnInitFailed");
                OnInitFailed?.Invoke(initError);
            };
            this.client.OnDataEncrypted += (encryptedData) =>
            {
                Debug.Log("OnDataEncrypted");
                OnDataEncrypted?.Invoke(encryptedData);
            };
            this.client.OnAppographyDataFetched += (appographyData) =>
            {
                Debug.Log("OnAppographyDataRecieved");
                OnAppographyDataRecieved?.Invoke(appographyData);
            };
        }

    }
}