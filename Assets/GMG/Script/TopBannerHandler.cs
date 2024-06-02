using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GMG
{
    public class TopBannerHandler : MonoBehaviour
    {
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private RectTransform Holder;
        [SerializeField] private RectTransform gamesHolder;
        [SerializeField] private RectTransform epicplayHeader;
        [SerializeField] private RectTransform gamesHeader;
        [SerializeField] private Button gameIconPrefab;
        [SerializeField] private float scrollSpeed;
        private float targetValue = 1;
        private List<Button> gameIcons = new List<Button>();
        private bool forward = true;
        private Coroutine scrollCoroutine;
        private bool Inited = false;
        public bool Auto = false;

        private void Awake()
        {
            GMGSDK.ShowBannerAd += GMGSDK_ShowBannerAd;
            GMGSDK.HideBannerAd += GMGSDK_HideBannerAd;
        }
        private void OnDestroy()
        {
            GMGSDK.ShowBannerAd -= GMGSDK_ShowBannerAd;
            GMGSDK.HideBannerAd -= GMGSDK_HideBannerAd;
        }
        private void OnEnable()
        {
            if (Auto && GMGSDK.Instance.IconsFetched)
                FillGames(GMGSDK.Instance.availableGames.ToArray());
        }
        private void GMGSDK_HideBannerAd()
        {
            Hide();
        }

        private void GMGSDK_ShowBannerAd(Game[] obj)
        {
            FillGames(obj);
        }

        public void FillGames(Game[] gamesList)
        {
            if (Holder.gameObject.activeInHierarchy && !Auto)
                return;
            Holder.gameObject.SetActive(true);
            if (!Inited)
            {
                foreach (var item in gamesList)
                {
                    Button game = Instantiate(gameIconPrefab, gamesHolder);
                    if (item.icon != null)
                        game.GetComponent<Image>().sprite = Sprite.Create(item.icon, new Rect(0, 0, item.icon.width, item.icon.height), Vector2.zero);
                    game.onClick.RemoveAllListeners();
                    game.onClick.AddListener(delegate { OpenLink(); });
                    game.gameObject.SetActive(true);
                    gameIcons.Add(game);
                }
                Inited = true;
            }
            targetValue = 1;
            scrollCoroutine = StartCoroutine(InfiniteScroll());
        }
        public void OpenLink()
        {
            if (PluginInit.instance != null)
                PluginInit.instance.OpenCTT("https://widget0001.epicplay.in/");
            else
                Application.OpenURL("https://widget0001.epicplay.in/");
        }
        IEnumerator InfiniteScroll()
        {
            epicplayHeader.gameObject.SetActive(true);
            gamesHeader.gameObject.SetActive(false);
            yield return new WaitForSecondsRealtime(1f);
            epicplayHeader.gameObject.SetActive(false);
            gamesHeader.gameObject.SetActive(true);
            scrollRect.horizontalNormalizedPosition = 0;
            targetValue = 1;
            forward = true;
            while (true)
            {
                if (forward && targetValue == 1)
                {
                    scrollRect.horizontalNormalizedPosition += Time.deltaTime * scrollSpeed;
                    if (scrollRect.horizontalNormalizedPosition >= 1)
                    {
                        targetValue = 0;
                        forward = false;
                        yield return StartCoroutine(SwitchBanners());
                    }
                }
                else
                {
                    scrollRect.horizontalNormalizedPosition -= Time.deltaTime * scrollSpeed;
                    if (scrollRect.horizontalNormalizedPosition <= 0)
                    {
                        targetValue = 1;
                        forward = true;
                        yield return StartCoroutine(SwitchBanners());
                    }

                }

                yield return new WaitForEndOfFrame();
            }
        }
        IEnumerator SwitchBanners()
        {
            yield return new WaitForSecondsRealtime(2f);
            epicplayHeader.gameObject.SetActive(true);
            gamesHeader.gameObject.SetActive(false);
            yield return new WaitForSecondsRealtime(1f);
            epicplayHeader.gameObject.SetActive(false);
            gamesHeader.gameObject.SetActive(true);
        }
        public void Hide()
        {
            this.StopAllCoroutines();
            Holder.gameObject.SetActive(false);
        }

    }
}
