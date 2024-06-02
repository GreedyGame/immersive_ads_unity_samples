using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace GMG
{
    public class IconHandler : MonoBehaviour
    {
        [SerializeField] private Button firstBtn;
        [SerializeField] private Button secondBtn;
        [SerializeField] private float scaleSpeed;
        [SerializeField] private Transform Holder;

        private Game[] gameList;
        private bool scalingFirst;
        int currentIndex;
        private void Awake()
        {
            GMGSDK.ShowIconAd += FillGames;
            GMGSDK.HideIconAd += Hide;
        }
        private void OnDestroy()
        {
            GMGSDK.ShowIconAd -= FillGames;
            GMGSDK.HideIconAd -= Hide;
        }
        public void FillGames(Game[] gamesList)
        {
            if (Holder.gameObject.activeInHierarchy)
                return;
            this.gameList = gamesList;
            Holder.gameObject.SetActive(true);
            firstBtn.transform.localScale = Vector3.one;
            secondBtn.transform.localScale = new Vector3(0, 1, 1);
            scalingFirst = true;
            UpdateButtons(firstBtn);
            UpdateButtons(secondBtn);
            StartCoroutine(ScaleUp(Holder, 2));
            StartCoroutine(SwtichBtns());
        }
        IEnumerator ScaleUp(Transform Holder, float speed)
        {
            while (Holder.localScale.x < 1)
            {
                Holder.localScale = new Vector3(Holder.localScale.x + Time.deltaTime * speed, Holder.localScale.y + Time.deltaTime * speed, Holder.localScale.z + Time.deltaTime * speed);
                yield return new WaitForEndOfFrame();
            }
        }
        IEnumerator SwtichBtns()
        {
            yield return new WaitForSeconds(3.3f);
            while (true)
            {
                if (scalingFirst)
                {
                    firstBtn.transform.localScale = new Vector3(firstBtn.transform.localScale.x - Time.deltaTime * scaleSpeed, firstBtn.transform.localScale.y, firstBtn.transform.localScale.z);
                    secondBtn.transform.localScale = new Vector3(secondBtn.transform.localScale.x + Time.deltaTime * scaleSpeed, secondBtn.transform.localScale.y, secondBtn.transform.localScale.z);
                    if (firstBtn.transform.localScale.x <= 0)
                    {
                        scalingFirst = false;
                        secondBtn.GetComponent<RectTransform>().pivot = new Vector2(0, 0.5f);
                        firstBtn.GetComponent<RectTransform>().pivot = new Vector2(1, 0.5f);
                        yield return StartCoroutine(SwitchBanners(firstBtn));
                    }
                }
                else
                {
                    secondBtn.transform.localScale = new Vector3(secondBtn.transform.localScale.x - Time.deltaTime * scaleSpeed, secondBtn.transform.localScale.y, secondBtn.transform.localScale.z);
                    firstBtn.transform.localScale = new Vector3(firstBtn.transform.localScale.x + Time.deltaTime * scaleSpeed, firstBtn.transform.localScale.y, firstBtn.transform.localScale.z);
                    if (secondBtn.transform.localScale.x <= 0)
                    {
                        scalingFirst = true;
                        secondBtn.GetComponent<RectTransform>().pivot = new Vector2(1, 0.5f);
                        firstBtn.GetComponent<RectTransform>().pivot = new Vector2(0, 0.5f);
                        yield return StartCoroutine(SwitchBanners(secondBtn));
                    }
                }
                yield return new WaitForEndOfFrame();
            }
        }

        IEnumerator SwitchBanners(Button button)
        {
            yield return new WaitForSecondsRealtime(3f);
            UpdateButtons(button);
        }
        void UpdateButtons(Button button)
        {
            if (currentIndex >= gameList.Length - 1)
                currentIndex = 0;
            button.GetComponent<Image>().sprite = Sprite.Create(gameList[currentIndex].icon, new Rect(0, 0, gameList[currentIndex].icon.width, gameList[currentIndex].icon.height), Vector2.zero);
            button.onClick.RemoveAllListeners();
            // button.onClick.AddListener(delegate { Application.OpenURL(gameList[currentIndex].GameLink + "?utm_source=" + Application.identifier + "&utm_medium=banner&utm_campaign=getmoregames"); });
            button.onClick.AddListener(delegate { OpenLink(); });
            currentIndex++;
        }
        public void OpenLink()
        {
            
            if (PluginInit.instance != null)
                PluginInit.instance.OpenCTT("https://widget0001.epicplay.in/");
            else
                Application.OpenURL("https://widget0001.epicplay.in/");
        }
        public void Hide()
        {
            this.StopAllCoroutines();
            Holder.gameObject.SetActive(false);
        }
    }

}