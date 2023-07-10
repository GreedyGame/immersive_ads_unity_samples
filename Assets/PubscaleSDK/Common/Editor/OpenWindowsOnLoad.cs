using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEditor.Compilation;

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
                if (!AssetDatabase.IsValidFolder(PubEditorUX.PackageSettingsFolderPath))
                {
                    AssetDatabase.CreateFolder("Assets", PubEditorUX.PackageSettingsFolder);
                }
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
