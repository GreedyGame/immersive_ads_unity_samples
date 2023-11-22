using PubScale.OfferWall;
using UnityEngine;
using UnityEngine.UI;

public class OfferWallButton : MonoBehaviour
{
    [SerializeField] private Button offerBtn;

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
        offerBtn.gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        offerBtn.gameObject.SetActive(false);
        if (OfferWallManager.IsInitialized)
            offerBtn.gameObject.SetActive(true);
    }
}
