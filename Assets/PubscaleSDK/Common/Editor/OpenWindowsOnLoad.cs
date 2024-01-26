using UnityEditor;
using UnityEngine;

namespace PubScale.SdkOne
{
    [InitializeOnLoad]
    public static class OpenWindowsOnLoad
    {
        static OpenWindowsOnLoad()
        {
            PubScaleSettings settings = AssetDatabase.LoadAssetAtPath<PubScaleSettings>(PubEditorUX.PackageSettingsPath);

            if (settings == null)
            {
                settings = PubEditorUX.CreateAndSavePubScaleSettings();
            }

            if (settings != null && settings.IsFirstTimeUsingTheAsset)
            {
                EditorApplication.delayCall += PubScaleWindow.OpenWindow;
            }

        }
    }
}
