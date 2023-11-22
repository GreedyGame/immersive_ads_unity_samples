using UnityEngine;
using UnityEngine.UI;

namespace PubScale.OfferWall
{
    public class OfferWallButton : MonoBehaviour
    {
        [SerializeField] private GameObject loadingState;
        [SerializeField ]private Button offerBtn;

        private void Awake()
        {
            OfferWallManager.OnInitialize += OfferWallManager_OnInitialize;
            offerBtn.onClick.RemoveAllListeners();
            offerBtn.onClick.AddListener(() =>
            {
                OfferWallManager.ShowOfferWall();
            });
        }
        private void OnDestroy()
        {
            OfferWallManager.OnInitialize -= OfferWallManager_OnInitialize;
        }
        private void OfferWallManager_OnInitialize()
        {
            SwitchLoadingState(false);
        }

        private void OnEnable()
        {
            SwitchLoadingState(true);
            if (OfferWallManager.IsInitialized)
                SwitchLoadingState(false);
        }
        void SwitchLoadingState(bool loading)
        {
            loadingState.SetActive(loading);
            offerBtn.gameObject.SetActive(!loading);
        }
    }
}