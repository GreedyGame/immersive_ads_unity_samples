#if UNITY_ANDROID
using System.IO;
using System.Xml.Linq;
using UnityEditor;
using UnityEditor.Build;
#if UNITY_2018_1_OR_NEWER
using UnityEditor.Build.Reporting;
#endif
using UnityEngine;

namespace PubScale.OfferWall
{

#if UNITY_2018_1_OR_NEWER
    public class ManifestProcessor : IPreprocessBuildWithReport
#else
public class ManifestProcessor : IPreprocessBuild
#endif
    {

        private XNamespace ns = "http://schemas.android.com/apk/res/android";

        public int callbackOrder { get { return 0; } }

#if UNITY_2018_1_OR_NEWER
        public void OnPreprocessBuild(BuildReport report)
#else
    public void OnPreprocessBuild(BuildTarget target, string path)
#endif
        {
            string manifestPath = Path.Combine(
                    Application.dataPath, "Plugins/Android/OfferWall/AndroidManifest.xml");


            XDocument manifest = null;
            try
            {
                manifest = XDocument.Load(manifestPath);
            }
            catch
            {
                StopBuildWithMessage("AndroidManifest.xml is not valid. Try re-importing the plugin.");
            }

            XElement elemManifest = manifest.Element("manifest");
            if (elemManifest == null)
            {
                StopBuildWithMessage("AndroidManifest.xml is not valid. Try re-importing the plugin.");
            }

            XElement elemApplication = elemManifest.Element("application");
            if (elemApplication == null)
            {
                StopBuildWithMessage("AndroidManifest.xml is not valid. Try re-importing the plugin.");
            }

            XElement elemActivity = elemApplication.Element("activity");
            if (elemActivity == null)
            {
                StopBuildWithMessage("AndroidManifest.xml is not valid. Try re-importing the plugin.");
            }
            else
            {
                if (PlayerSettings.allowedAutorotateToPortraitUpsideDown  || PlayerSettings.allowedAutorotateToPortrait)
                {
                    elemActivity.SetAttributeValue(ns + "screenOrientation", "portrait");
                }
                else
                {
                    elemActivity.SetAttributeValue(ns + "screenOrientation", "landscape");
                }
            }
            elemManifest.Save(manifestPath);
        }
        private void StopBuildWithMessage(string message)
        {
            string prefix = "[PubScale] ";
#if UNITY_2017_1_OR_NEWER
            throw new BuildPlayerWindow.BuildMethodException(prefix + message);
#else
        throw new OperationCanceledException(prefix + message);
#endif
        }
    }
}
#endif
