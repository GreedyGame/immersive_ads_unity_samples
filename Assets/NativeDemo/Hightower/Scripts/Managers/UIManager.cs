using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;
using System.Collections;
using PubScale.SdkOne;
using PubScale.SdkOne.NativeAds.Sample;


namespace PubScale.SdkOne.NativeAds.Hightower
{
    public class UIManager : MonoBehaviour
    {

        #region  STORE

        public StoreCharacterDisplay Char1;
        public StoreCharacterDisplay Char2;
        public StoreCharacterDisplay Char3;
        public StoreCharacterDisplay Char4;

        public Color ClrBtnCurrentSelected;
        public Color ClrBtnChoose;

        [SerializeField] private CanvasGroup storeUI;

        [SerializeField] private GameObject storeBtn;

        [SerializeField] private TextMeshProUGUI storeCoinsUI;

        #endregion

        [Space(20)]


        #region Decalaration


        [SerializeField] private GameObject gmgBtn;

        public static event Action<bool> GamePaused;
        public TextMeshProUGUI timerTxt;
        [SerializeField] private CanvasGroup gameCanvas;
        [SerializeField] private CanvasGroup gameOverUI;
        [SerializeField] private CanvasGroup pauseUI;
        [SerializeField] private CanvasGroup tutorialUI;
        [SerializeField] private CanvasGroup countdownUI;
        [SerializeField] private Button pauseBtn;
        [SerializeField] private TextMeshProUGUI scoreTxt;
        [SerializeField] private TextMeshProUGUI bestScoreTxt;
        [SerializeField] private TextMeshProUGUI countDownTxt;
        [SerializeField] private TextMeshProUGUI gameOvertotalScoreTxt;
        [SerializeField] private TextMeshProUGUI gameOverbestScoreTxt;
        [SerializeField] private GameObject newBest;
        [SerializeField] private GameObject continueButton;
        [SerializeField] private Image audioToggle;
        [SerializeField] private Sprite mute, umute;
        [SerializeField] private NativeAdHolder ads;
        private Coroutine countDownRoutine;
        private bool pauseState = false;
        private bool canMute = false;
        public Action<bool> ShowHideAd;
        #endregion

        #region Intialisation
        private void Awake()
        {
            NativeAdClickDisabler.ShowHideAd += NativeAdClickDisabler_ShowHideAd;
            bestScoreTxt.text = PlayerPrefs.GetInt(PrefsHelper.Key_BestScore, 0).ToString();
            gameCanvas.gameObject.SetActive(false);
            AudioListener.volume = PlayerPrefs.GetInt("mute", 0) == 1 ? 0 : 1;
            AudioManager.isMute = PlayerPrefs.GetInt("mute", 0) == 1;
            audioToggle.sprite = AudioListener.volume == 0 ? mute : umute;
            canMute = true;
            Time.timeScale = 1;
            continueButton.gameObject.SetActive(false);

            gmgBtn.SetActive(false);
            storeBtn.SetActive(false);
            storeUI.gameObject.SetActive(false);
        }

        private void NativeAdClickDisabler_ShowHideAd(Action<bool> obj)
        {
            ShowHideAd = obj;
        }

        public void InitUI(int score)
        {
            tutorialUI.blocksRaycasts = true;
            tutorialUI.gameObject.SetActive(true);
            tutorialUI.DOFade(1, 1).From(0);
            gameCanvas.gameObject.SetActive(false);
            scoreTxt.text = score.ToString();
            //AdManager.instance.LoadInterstitialAd();
        }

        #endregion


        #region Events
        public void StartLevel()
        {
            tutorialUI.blocksRaycasts = false;
            tutorialUI.DOFade(0, 1).From(1).OnComplete(() => { tutorialUI.gameObject.SetActive(false); });
            gameCanvas.gameObject.SetActive(true);
            gameCanvas.DOFade(1, 1).From(0);
        }
        public void UpdateScore(int score)
        {
            scoreTxt.text = score.ToString();
        }
        public void RefreshAd()
        {
            ads.FetchAd();
        }
        public void MuteToggle()
        {
            if (!canMute)
                return;
            AudioListener.volume = 1 - AudioListener.volume;
            AudioManager.instance.Play("button");
            PlayerPrefs.SetInt("mute", AudioListener.volume == 0 ? 1 : 0);
            audioToggle.sprite = AudioListener.volume == 0 ? mute : umute;
        }
        public void RestartLevel()
        {
            PlayerPrefs.SetInt(PrefsHelper.Key_CurrentScore, GameManager.instance.GetCurrentCoins());
            AudioManager.instance.Play("button");
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
            //  AdManager.instance.ShowInterstitialAd();
            Time.timeScale = 1;

#if UNITY_WEBGL && !UNITY_EDITOR
        WebUtils.ShowInterstitialAd();
#endif
        }
        public void Pause()
        {
            AudioManager.instance.Play("button");
            pauseState = !pauseState;
            if (pauseState)
            {
                GamePaused?.Invoke(true);
                pauseUI.blocksRaycasts = true;
                pauseBtn.interactable = false;
                pauseUI.interactable = false;
                AudioManager.instance.Paused(true);
                Time.timeScale = 0;
                ShowHideAd?.Invoke(true);
                DGAnimationController.AnimateBgPanel(pauseUI.gameObject, true, Complete: () => { pauseUI.interactable = true; gmgBtn.SetActive(true); });
            }
            else
            {
                gmgBtn.SetActive(false);

                ShowHideAd?.Invoke(false);
                if (countDownRoutine == null)
                    countDownRoutine = StartCoroutine(CountDown());
                AudioManager.instance.Paused(false);

            }
        }
        public void QuitGame()
        {
            Time.timeScale = 1;

            if (Fader.instance != null)
                Fader.LoadScene("MainScene");
            else
                UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");

        }


        IEnumerator CountDown()
        {
            pauseUI.blocksRaycasts = false;
            pauseUI.interactable = false;
            DGAnimationController.AnimateBgPanel(pauseUI.gameObject, false, Complete: () =>
            {
                pauseBtn.interactable = true;
            });
            countdownUI.gameObject.SetActive(true);
            countdownUI.DOFade(1, 0.3f).From(0).SetUpdate(true);
            int timer = 1;
            while (timer > 0)
            {
                countDownTxt.text = "Starts in\n" + timer;
                //print("Starts in" + timer);
                yield return new WaitForSecondsRealtime(1);
                timer--;
            }
            countdownUI.gameObject.SetActive(false);
            GamePaused?.Invoke(false);
            Time.timeScale = 1;
            countDownRoutine = null;
        }
        public void ShowLevelComplete(int best, bool newScore)
        {
            ShowHideAd?.Invoke(true);
            newBest.SetActive(newScore);
            gameOvertotalScoreTxt.text = scoreTxt.text;
            gameOverbestScoreTxt.text = best.ToString();
            DGAnimationController.AnimateBgPanel(gameOverUI.gameObject, true, () => { gmgBtn.SetActive(true); storeBtn.SetActive(true); });

        }
        public void HideLevelComplete()
        {
            ShowHideAd?.Invoke(false);
            continueButton.SetActive(false);
            gmgBtn.SetActive(false);
            storeBtn.SetActive(false);

            DGAnimationController.AnimateBgPanel(gameOverUI.gameObject, false);
        }
        public void ContinueButton(bool ans)
        {
            continueButton.gameObject.SetActive(ans);
        }
        #endregion

        #region UtilityFunctions
        private void OnApplicationPause(bool pause)
        {
            //if (pause)
            //    ForcePause();
        }
        private void OnApplicationFocus(bool focus)
        {
            //if (!focus)
            //    ForcePause();
        }
        public void ForcePause()
        {
            //if (countDownRoutine != null)
            //{
            //    countdownUI.gameObject.SetActive(false);
            //    pauseUI.blocksRaycasts = true;
            //    pauseBtn.interactable = false;
            //    pauseState = true;
            //    pauseUI.interactable = true;
            //    DGAnimationController.EnablePanel(pauseUI.gameObject, true);
            //    StopCoroutine(countDownRoutine);
            //    countDownRoutine = null;
            //}
            //if (Time.timeScale == 0 || gameOverUI.gameObject.activeInHierarchy || tutorialUI.blocksRaycasts)
            //    return;
            //pauseUI.blocksRaycasts = true;
            //pauseUI.interactable = true;
            //pauseBtn.interactable = false;
            //pauseState = true;
            //Time.timeScale = 0;
            //DGAnimationController.KillAnimation(pauseUI.gameObject);
            //DGAnimationController.EnablePanel(pauseUI.gameObject, true);
            //GamePaused?.Invoke(true);
        }
        public void ForceUnPause()
        {
            //pauseUI.gameObject.SetActive(false);
            //if (countDownRoutine == null)
            //{
            //    Time.timeScale = 0;
            //    StartCoroutine(CountDown());
            //}
        }


        #region STORE

        public void ShowStore()
        {
            GameManager.instance.audioMgr.PlayMenuOpen();

            gmgBtn.SetActive(false);
            storeBtn.SetActive(false);
            storeUI.gameObject.SetActive(true);

            UpdateStoreDisplay();
        }


        public void UpdateStoreDisplay()
        {
            Char1.UpdateDisplay(this);
            Char2.UpdateDisplay(this);
            Char3.UpdateDisplay(this);
            Char4.UpdateDisplay(this);

            storeCoinsUI.text = GameManager.instance.GetCurrentCoins().ToString();
        }


        public void HideStore()
        {
            gmgBtn.SetActive(true);
            storeBtn.SetActive(true);

            GameManager.instance.audioMgr.PlayMenuClose();
            storeUI.gameObject.SetActive(false);
        }



        #endregion


        public StoreCharacterDisplay GetCurrentCharDisplay(int id)
        {
            switch (id)
            {
                case 1:
                    return Char1;

                case 2:
                    return Char2;

                case 3:
                    return Char3;

                case 4:
                    return Char4;

                default:
                    return Char1;
            }
        }

        public void BuyCharacter(int id)
        {
            StoreCharacterDisplay scd = GetCurrentCharDisplay(id);

            int currentCoins = GameManager.instance.GetCurrentCoins();

            if (currentCoins >= scd.Cost)
            {
                GameManager.instance.celebration.Play();
                currentCoins -= scd.Cost;

                storeCoinsUI.text = currentCoins.ToString();

                GameManager.instance.SetCurrentCoins(currentCoins);

                PlayerPrefs.SetInt(PrefsHelper.Key_PrefixCharUnlocked + id, 1);

                GameManager.instance.audioMgr.PlayCharUnlock();

                scd.UpdateDisplay(this);
            }

        }


        public void SelectCharacter(int id)
        {
            if (id < 1 || id > 4)
                id = 1;

            PlayerPrefs.SetInt(PrefsHelper.Key_CurCharSelected, id);

            Char1.UpdateDisplay(this);
            Char2.UpdateDisplay(this);
            Char3.UpdateDisplay(this);
            Char4.UpdateDisplay(this);

            RestartLevel();
        }

        #endregion
    }
}