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
                PubEditorUX.CheckResourcesFolderInCommon();

                settings = ScriptableObject.CreateInstance<PubScaleSettings>();
                AssetDatabase.CreateAsset(settings, PubEditorUX.PackageSettingsPath);
            }

            if (settings.IsFirstTimeUsingTheAsset)
            {
                EditorApplication.delayCall += PubScaleWindow.OpenWindow;
            }

        }
    }
}
