using PubScale.Common;
using System;
using System.IO;
using UnityEngine;

namespace PubScale.OfferWall
{
    public class OfferWallManager
    {
        public static event Action OnInitialize;
        public static event Action<string> OnInitializeFailed;
        public static event Action OnOfferWallShown;
        public static event Action OnOfferWallClosed;
        public static event Action<float> OnOfferClaimed;
        public static event Action<string> OnOfferWallShowFail;
        private static OfferWall offerWall;
        private static float amountEarned;
        private static Action<bool, float> UnlockRewardAction;
        public static bool IsInitialized=false;
        public static void InitializeOfferWall(string appKey, string uniqueID="",string resourceFileName = "" ,bool isFullScreen=true, bool DebugMode = false)
        {
            if(Application.isEditor)
            {
                Log("Offer Wall won't work in editor");
                return;
            }
            if(appKey == "")
            {
                Log("App key is empty");
                return;
            }
            string resourcePath = "";
            if (resourceFileName != "")
            {
                if (CheckIfImageExists(resourceFileName))
                {
                    Log("Image file found");
                    resourcePath = GetPersistentPath(resourceFileName);
                }
                else
                {
                    Log("Image file not found");
                    resourcePath = CopyImageToPersistentPath(resourceFileName);
                }
            }
            else
            {
                Log("Resource file name is empty");
            }
            Log("Initializing Offer Wall");
            amountEarned = 0;
            UnityMainThreadDispatcher.Initialize();
            offerWall = new OfferWall(appKey, uniqueID, resourcePath, isFullScreen, DebugMode);
            offerWall.InitOfferWall();
            offerWall.OnOfferWallInitSuccess += () =>
            {
                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {
                    IsInitialized = true;
                    OnInitialize?.Invoke();
                    Log("Offer wall init success");
                });
            };
            offerWall.OnOfferWallInitFailed += (cause) =>
            {
                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {
                    IsInitialized = false;
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
            offerWall.OnOfferWallRewardClaimed += (amount, currency) =>
            {
                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {
                    float.TryParse(amount, out amountEarned);
                    OnOfferClaimed?.Invoke(amountEarned);
                    Log($"Offer wall reward claimed amount {amount} and currency {currency}");

                });
            };
            offerWall.OnOfferWallShowFailed += (cause) =>
            {
                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {
                    OnOfferWallShowFail?.Invoke(cause);
                    Log($"Offer wall offer failed cause {cause}");
                });
            };
        }
        public static void ShowOfferWall()
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
        public static void DisposeOfferWall()
        {
            if (offerWall == null)
            {
                Log("OfferWall is not Initialized");
                return;
            }
            offerWall.DisposeOfferWall();
        }
        public static void UnlockReward(Action<bool, float> action)
        {
            UnlockRewardAction = action;
            ShowOfferWall();
        }
        static string CopyImageToPersistentPath(string resourceName)
        {
            // Load the image from Resources
            Sprite imageSprite = Resources.Load<Sprite>(resourceName);

            if (imageSprite != null)
            {
                // Convert the sprite to a texture
                Texture2D texture = imageSprite.texture;

                // Encode the texture to a PNG byte array
                byte[] imageBytes = texture.EncodeToPNG();

                // Combine the destination path with the image name and extension
                string destinationFilePath = GetPersistentPath(resourceName);

                // Write the image bytes to the persistent path
                File.WriteAllBytes(destinationFilePath, imageBytes);

                Debug.Log("Image saved to: " + destinationFilePath);
                return destinationFilePath;
            }
            else
            {
                Debug.LogWarning("Image not found in Resources: " + resourceName);
                return "";
            }
        }
        static string GetPersistentPath(string resourceName)
        {
            string destinationFilePath = Path.Combine(Application.persistentDataPath, resourceName + ".png");
            return destinationFilePath;
        }
        static bool CheckIfImageExists(string resourceName)
        {
            return File.Exists(GetPersistentPath(resourceName));
        }
        public static void Log(string log)
        {
            Debug.Log(log);
        }
    }
}