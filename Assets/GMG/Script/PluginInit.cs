using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PluginInit : MonoBehaviour
{
    public static PluginInit instance;
    AndroidJavaClass unityClass;
    AndroidJavaObject unityActivity;
    AndroidJavaObject _pluginInstance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            InitialisePlugin("com.gmg.unityplugin.PluginInstance");

            // InitialisePlugin("com.pubscale.unity.android.common_utils.CustomTabs");
        }
        else
        {
            Destroy(this);
        }
    }
    void InitialisePlugin(string pluginName)
    {

#if UNITY_EDITOR
        return;
#endif
        unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        unityActivity = unityClass.GetStatic<AndroidJavaObject>("currentActivity");
        _pluginInstance = new AndroidJavaObject(pluginName);

        // if (_pluginInstance == null)
        // {
        //     Debug.Log("Plugin Instance Error");
        // }
        // else
        // {

        //     Debug.Log("Plugin Init");
        // }
        // _pluginInstance.CallStatic("receiveUnityActivity", unityActivity);
    }

    public void OpenCTT(string url)
    {
        if (Application.isEditor)
        {
            Application.OpenURL(url);
            return;
        }
        else if (Application.platform == RuntimePlatform.Android)
        {

            unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            unityActivity = unityClass.GetStatic<AndroidJavaObject>("currentActivity");

            AndroidJavaClass customTabObject = new AndroidJavaClass("com.pubscale.unity.android.common_utils.CustomTabs");

            customTabObject.CallStatic("openUrl", unityActivity, url, true);
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            Application.OpenURL(url);
            return;
        }



        // if (_pluginInstance != null)
        // {
        //     _pluginInstance.Call("OpenTab", url);

        // }
        // else
        // {
        //     Application.OpenURL(url);
        //     Debug.Log("Plugin Instance Error");
        // }
    }
}
