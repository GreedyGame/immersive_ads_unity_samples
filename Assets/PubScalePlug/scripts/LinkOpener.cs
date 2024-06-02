using UnityEngine;
using UnityEngine.EventSystems;
public class LinkOpener : MonoBehaviour, IPointerDownHandler
{

    public string PlugUrl = "https://widget0001.epicplay.in/";

    public void OnPointerDown(PointerEventData eventData)
    {
        OpenLink();
    }

    private void OnMouseDown()
    {
        OpenLink();
    }
    public void OpenLink()
    {
        if (Application.platform == RuntimePlatform.Android)
            OpenAndroidCustomTab(PlugUrl);
        else
            Application.OpenURL(PlugUrl);
    }

    void OpenAndroidCustomTab(string url)
    {

        AndroidJavaClass unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject unityActivity = unityClass.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject customTabObject = new AndroidJavaObject("com.pubscale.unity.android.common_utils.CustomTabs", unityActivity);

        customTabObject.Call("openUrl", url, 0);

    }
}
