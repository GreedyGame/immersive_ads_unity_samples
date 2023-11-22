using PubScale.OfferWall;
using TMPro;
using UnityEngine;

public class OfferWallHandler : MonoBehaviour
{
    [SerializeField] private string appKey= "ce0fc906-01a2-4d2d-82bf-cdde1f7c8944";
    [SerializeField] private Sprite OfferWallBackgroundImage;
    [SerializeField] private bool isFullScreen = false;
    [SerializeField] private bool devModeEnabled = false;
    [SerializeField] private TextMeshProUGUI errorTxt;
    void Awake()
    {
        string resourceFileName = "";
        if(OfferWallBackgroundImage != null)
        {
            resourceFileName = OfferWallBackgroundImage.name;
        }
        OfferWallManager.InitializeOfferWall(appKey, SystemInfo.deviceUniqueIdentifier, resourceFileName, isFullScreen, devModeEnabled);
        OfferWallManager.OnInitializeFailed += OfferWallManager_OnInitializeFailed;
        OfferWallManager.OnOfferWallShowFail += OfferWallManager_OnOfferWallShowFail;
    }
    private void OnDestroy()
    {
        OfferWallManager.OnInitializeFailed -= OfferWallManager_OnInitializeFailed;
        OfferWallManager.OnOfferWallShowFail -= OfferWallManager_OnOfferWallShowFail;
    }
    private void OfferWallManager_OnOfferWallShowFail(string obj)
    {
        ShowError("Failed to show OfferWall: " + obj);
    }

    private void OfferWallManager_OnInitializeFailed(string obj)
    {
        ShowError("OfferWall Initialize Failed: " + obj);
    }
    void ShowError(string error)
    {
        Debug.LogError(error);
        if(errorTxt == null)
        {
            return;
        }
        errorTxt.gameObject.SetActive(true);
        errorTxt.text = error;
    }
}
