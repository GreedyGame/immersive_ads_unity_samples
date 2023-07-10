using UnityEditor;
using UnityEngine;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.Compilation;
using PubScale.SdkOne;



public class PubScaleBuildProcessor : IPreprocessBuildWithReport, IPostprocessBuildWithReport
{
    public int callbackOrder => 0;

    private static object _compilationContext;
    private static int _compilationErrorCount;

    private static PubScaleSettings psSettings = null;


    static void LoadPubScaleSettings()
    {
        psSettings = AssetDatabase.LoadAssetAtPath<PubScaleSettings>(PubEditorUX.PackageSettingsPath);

        if (psSettings == null)
        {
            psSettings = ScriptableObject.CreateInstance<PubScaleSettings>();
            AssetDatabase.CreateAsset(psSettings, PubEditorUX.PackageSettingsPath);
        }

        PubScaleSettings.Instance = (PubScaleSettings)AssetDatabase.LoadAssetAtPath(PubEditorUX.PackageSettingsPath, typeof(PubScaleSettings));
    }



    public void OnPreprocessBuild(BuildReport report)
    {
        _compilationContext = null;
        _compilationErrorCount = 0;

        CompilationPipeline.compilationStarted += CompilationPipelineOnCompilationStarted;
        CompilationPipeline.assemblyCompilationFinished += CompilationPipelineOnAssemblyCompilationFinished;
        CompilationPipeline.compilationFinished += CompilationPipelineOnCompilationFinished;

        LoadPubScaleSettings();
        PerformPubScaleBuildChecks();
    }

    private static void CompilationPipelineOnCompilationStarted(object compilationContext)
    {
        _compilationContext = compilationContext;
        _compilationErrorCount = 0;
    }

    private static void CompilationPipelineOnAssemblyCompilationFinished(string path, CompilerMessage[] messages)
    {
        for (int i = messages?.Length ?? 0; --i >= 0;)
        {
            if (messages[i].type == CompilerMessageType.Error)
                ++_compilationErrorCount;
        }
    }

    private static void CompilationPipelineOnCompilationFinished(object compilationContext)
    {
        if (compilationContext != _compilationContext)
            return;

        _compilationContext = null;

        CompilationPipeline.compilationStarted -= CompilationPipelineOnCompilationStarted;
        CompilationPipeline.assemblyCompilationFinished -= CompilationPipelineOnAssemblyCompilationFinished;
        CompilationPipeline.compilationFinished -= CompilationPipelineOnCompilationFinished;

        if (_compilationErrorCount > 0)
        {
            Debug.LogError($"Compilation finished with errors ({_compilationErrorCount})");
            // Custom compilation failure processing
        }
    }

    public void OnPostprocessBuild(BuildReport report)
    {
        if (_compilationErrorCount > 0)
        {
            throw new BuildFailedException("Sorry");
        }
    }



    void PerformPubScaleBuildChecks()
    {
        int pubScaleErrors = 0;

        if (psSettings == null)
        {
            Debug.LogError("PubScale SDK settings not found");
            pubScaleErrors++;
        }
        else
        {
            if (psSettings.AppId == string.Empty || psSettings.AppId == "")
            {
                Debug.LogWarning($"PubScale App ID is Empty. Please assign your SDK App ID in the PubScale Window.");
                pubScaleErrors++;
            }

#if UNITY_ANDROID
            if (psSettings.Fallback_NativeAdID_Android == string.Empty || psSettings.Fallback_NativeAdID_Android == "")
            {
                Debug.LogWarning($"PubScale Fallback Native Ad ID for Android is Empty. Please provide an Ad Unit ID in the PubScale Window.");
                pubScaleErrors++;
            }
#endif

#if UNITY_IOS
           if (psSettings.Fallback_NativeAdID_IOS == string.Empty || psSettings.Fallback_NativeAdID_IOS == "")
            {
                Debug.LogWarning($"PubScale Fallback Native Ad ID for IOS is Empty. Please provide an Ad Unit ID in the PubScale Window.");
                pubScaleErrors++;
            }
#endif

        }

        if (pubScaleErrors > 0)
        {
            throw new BuildFailedException("PubScale Build PreProcess has detected some issues. Please see Warning messages.");
        }


    }




}
