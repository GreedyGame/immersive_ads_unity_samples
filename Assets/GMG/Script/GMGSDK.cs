using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace GMG
{
    [Serializable]
    public class Game
    {
        public string Name = "";
        public string IconUrl = "";
        public string GameLink = "";
        public Texture2D icon;
    }

    [Serializable]
    public class GameRoot
    {
        public List<Game> games = new List<Game>();
    }

    public class GMGSDK : MonoBehaviour
    {
        public static GMGSDK Instance;
        public static event Action<Game[]> ShowBannerAd;
        public static event Action HideBannerAd;
        public static event Action<Game[]> ShowIconAd;
        public static event Action HideIconAd;
        private const string URL = "https://storage.googleapis.com/unity_web_games_dev/GetMoreGames/Games_List.json";

        private GameRoot root;
        [SerializeField] private bool autoInit = false;
        [HideInInspector] public List<Game> availableGames = new List<Game>();
        public static event Action OnGamesFetched;

        private int fetchedIconsCount;
        public bool IconsFetched = false;
        private Coroutine IconFetcher;
        private Coroutine bannerFetcher;
        private void Awake()
        {
            gameObject.hideFlags = HideFlags.HideInHierarchy;
            if (Instance == null)
                Instance = this;

            DontDestroyOnLoad(this);
            if (autoInit)
                Init();
        }

        public void Init()
        {
            GenerateRequest();
        }
        public void ShowBanner()
        {
            if (!IconsFetched)
            {
                bannerFetcher = StartCoroutine(CallAfter(ShowBanner));
                return;
            }
            ShowBannerAd?.Invoke(availableGames.ToArray());
            //  _anim.Play("TopBannerStart");
        }
        public void ShowIcon()
        {
            if (!IconsFetched)
            {
                IconFetcher = StartCoroutine(CallAfter(ShowIcon));
                return;
            }
            ShowIconAd?.Invoke(availableGames.ToArray());
        }
        IEnumerator CallAfter(Action callback)
        {
            print("I am Called");
            while (!IconsFetched)
            {
                yield return new WaitForSeconds(1f);
            }
            callback?.Invoke();
        }
        public void HideBanner()
        {
            if (bannerFetcher != null)
                StopCoroutine(bannerFetcher);
            HideBannerAd?.Invoke();
        }
        public void HideIcon()
        {
            if (IconFetcher != null)
                StopCoroutine(IconFetcher);
            HideIconAd?.Invoke();
        }
        private void GenerateRequest()
        {
            root = new GameRoot();
            StartCoroutine(ProcessRequest(URL));
        }

        private IEnumerator ProcessRequest(string uri)
        {
            using (UnityWebRequest request = UnityWebRequest.Get(uri))
            {
                yield return request.SendWebRequest();

                if (request.isNetworkError)
                {
                    Debug.LogError(request.error);
                }
                else
                {
                    // Debug.Log(request.downloadHandler.text);
                    JsonUtility.FromJsonOverwrite(request.downloadHandler.text, root);
                    GetAvailableGames();

                }
            }
        }

        private void GetAllIcons()
        {
            fetchedIconsCount = 0;
            foreach (Game game in availableGames)
            {
                //Debug.Log("Game " + game.Name);
                if (!string.IsNullOrEmpty(game.IconUrl))
                {
                    StartCoroutine(ProcessIconRequest(game));
                }
            }
            StartCoroutine(CheckForIcons());
        }
        IEnumerator CheckForIcons()
        {
            foreach (var item in availableGames)
            {
                yield return new WaitUntil(() => item.icon != null);
            }
            IconsFetched = true;
        }
        public void RefreshGames()
        {
            GetAvailableGames();
        }

        private void GetAvailableGames()
        {
            availableGames = new List<Game>();

            for (int i = 0; i < root.games.Count; i++)
            {
                availableGames.Add(root.games[i]);
            }
            GetAllIcons();
        }

        private IEnumerator ProcessIconRequest(Game game)
        {
            using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(game.IconUrl))
            {
                yield return request.SendWebRequest();

                if (request.isNetworkError)
                {
                    Debug.LogError(request.error);
                }
                else
                {

                    Texture2D myTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                    game.icon = myTexture;
                    fetchedIconsCount++;
                    //Debug.Log("games fetched " + fetchedIconsCount + " =  "+(availableGames.Count-1)+ game.Name);
                    if (OnGamesFetched != null && fetchedIconsCount == availableGames.Count)
                        OnGamesFetched.Invoke();
                }
            }
        }

        public Game getRandomGame()
        {
            int x = UnityEngine.Random.Range(0, availableGames.Count);
            if (x <= availableGames.Count)
                return availableGames[x];
            else if (availableGames.Count > 0)
                return availableGames[0];
            else
                return null;
        }

        public Sprite TextureToSprite(Texture2D texture)
        {
            Rect rec = new Rect(0, 0, texture.width, texture.height);
            Sprite spriteToUse = Sprite.Create(texture, rec, new Vector2(0.5f, 0.5f), 100);

            return spriteToUse;
        }
    }
}
