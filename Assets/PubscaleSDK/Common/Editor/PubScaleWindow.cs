using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace PubScale.SdkOne
{
    public class PubScaleWindow : EditorWindow
    {
        public enum Category
        {
            SETUP,
            SETTINGS,
            SUPPORT
        }

        public Category category = Category.SETUP;

        private List<Category> _categories;
        private List<string> _categoryLabels;
        private Category _categorySelected;



        const string WindowTitle = "PubScale : " + PubEditorUX.packageName;
        GUIStyle textStyle;
        GUIStyle linkStyle;
        Texture image;
        bool initialized = false;


        static PubScaleSettings psSettings = null;

        float defaultEditorLabelWidth = 60;

        PubEditorUXState startState = new PubEditorUXState();

        [MenuItem("Window/PubScale SDK/Main", priority = -10)]
        public static void OpenWindow()
        {
            if (psSettings == null)
                LoadPubScaleSettings();

            PubScaleWindow wnd = GetWindow<PubScaleWindow>(false);
            wnd.titleContent = new GUIContent(WindowTitle);
            wnd.minSize = new Vector2(400, 400);
            wnd.maxSize = wnd.minSize;
            wnd.Show();

            psSettings.IsFirstTimeUsingTheAsset = false;

            EditorUtility.SetDirty(psSettings);
            AssetDatabase.SaveAssets();

            //  Debug.Log("Is Pro Skin - " + EditorGUIUtility.isProSkin);
        }

        private void OnEnable()
        {
            if (_categories == null)
            {
                InitCategories();
            }
            // if (_categorizedItems == null)
            // {
            //     InitContent();
            // }
            InitStyles();
        }

        private GUIStyle _tabStyle;
        private void InitStyles()
        {
            _tabStyle = new GUIStyle();
            _tabStyle.alignment = TextAnchor.MiddleCenter;
            _tabStyle.fontSize = 16;
            Texture2D tabNormal = (Texture2D)Resources.Load("Tab_Normal");
            Texture2D tabSelected = (Texture2D)Resources.Load("Tab_Pub");
            Font tabFont = (Font)Resources.Load("Oswald-Regular");
            _tabStyle.font = tabFont;
            _tabStyle.fixedHeight = 40;
            _tabStyle.normal.background = tabNormal;
            _tabStyle.normal.textColor = Color.grey;
            _tabStyle.onNormal.background = tabSelected;
            _tabStyle.onNormal.textColor = Color.black;
            _tabStyle.onFocused.background = tabSelected;
            _tabStyle.onFocused.textColor = Color.black;
            _tabStyle.border = new RectOffset(18, 18, 20, 4);
        }

        private void InitCategories()
        {
            _categories = PubEditorUX.GetListFromEnum<Category>();
            _categoryLabels = new List<string>();
            foreach (Category category in _categories)
            {
                _categoryLabels.Add(category.ToString());
            }
        }

        private void DrawTabs()
        {

            int index = (int)_categorySelected;
            EditorGUILayout.Space();

            index = GUILayout.Toolbar(index, _categoryLabels.ToArray(), _tabStyle);
            _categorySelected = _categories[index];
        }


        static void LoadPubScaleSettings()
        {
            psSettings = AssetDatabase.LoadAssetAtPath<PubScaleSettings>(PubEditorUX.PackageSettingsPath);

            if (psSettings == null)
            {
                psSettings = ScriptableObject.CreateInstance<PubScaleSettings>();
                AssetDatabase.CreateAsset(psSettings, PubEditorUX.PackageSettingsPath);
            }

            PubScaleSettings.Instance = (PubScaleSettings)AssetDatabase.LoadAssetAtPath(PubEditorUX.PackageSettingsPath, typeof(PubScaleSettings));

            //  Selection.activeObject = PubScaleSettings.Instance;
        }

        public void OnGUI()
        {
            if (psSettings == null)
                LoadPubScaleSettings();


            defaultEditorLabelWidth = EditorGUIUtility.labelWidth;





            if (!initialized)
            {
                string[] s = AssetDatabase.FindAssets(PubEditorUX.imageName + " t:Texture");

                if (s.Length > 0)
                {
                    image = AssetDatabase.LoadAssetAtPath<Texture>(AssetDatabase.GUIDToAssetPath(s[0]));
                }

                textStyle = new GUIStyle(EditorStyles.label);
                textStyle.wordWrap = true;
                textStyle.margin = new RectOffset(20, 20, 20, 20);
                textStyle.alignment = TextAnchor.UpperLeft;
                linkStyle = new GUIStyle(textStyle);
                linkStyle.hover.textColor = linkStyle.normal.textColor * 0.5f;
                initialized = true;
            }

            float imageHeight = image.height / 4;
            float imageWidth = image.width / 4;

            float margin = 20;

            if (image != null)
            {
                GUI.DrawTexture(new Rect(EditorGUIUtility.currentViewWidth / 2 - imageWidth / 2, 5, imageWidth, imageHeight), image, ScaleMode.ScaleToFit);
            }

            EditorGUILayout.Space();

            //   EditorGUILayout.LabelField(@"SDK ONE: REVENUE OPTIMIZATION", textStyle);

            using (new EditorGUILayout.VerticalScope(GUI.skin.box))
            {


                EditorGUIUtility.labelWidth = PubEditorUX.DEF_LABEL_WIDTH;


                GUILayout.BeginArea(new Rect(20, imageHeight + 10, position.width - margin * 2, position.height - imageHeight + 2 * margin));

                DrawTabs();

                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUILayout.Space();

                int selection = (int)_categorySelected;

                if (selection == 0)
                {
                    EditorGUILayout.Space();
                    EditorGUILayout.Space();

                    EditorGUIUtility.labelWidth = PubEditorUX.DEF_LABEL_WIDTH - 40;

                    using (new EditorGUILayout.VerticalScope(GUI.skin.box))
                    {
                        psSettings.AppId = EditorGUILayout.TextField("PubScale APP ID:", psSettings.AppId);
                    }

                    EditorGUIUtility.labelWidth = PubEditorUX.DEF_LABEL_WIDTH + 40;


                    EditorGUILayout.Space();

                }
                else if (selection == 1)
                {
                    EditorGUILayout.Space();
                    EditorGUILayout.Space();

                    using (new EditorGUILayout.VerticalScope(GUI.skin.box))
                    {


                        PubEditorUX.DisplayHeading("NATIVE ADS");

                        EditorGUILayout.BeginHorizontal();

                        EditorGUIUtility.labelWidth = PubEditorUX.DEF_LABEL_WIDTH - 60;

                        psSettings.UseTestMode = EditorGUILayout.Toggle("Use Test Mode:", psSettings.UseTestMode);

                        if (psSettings.UseTestMode == false)
                        {
                            GUILayout.FlexibleSpace();

                            EditorGUIUtility.labelWidth = PubEditorUX.DEF_LABEL_WIDTH + 30;
                            psSettings.EnablePluginLogsInRelease = EditorGUILayout.Toggle("Enable Plugin Logs in Release:", psSettings.EnablePluginLogsInRelease);

                        }

                        EditorGUIUtility.labelWidth = PubEditorUX.DEF_LABEL_WIDTH + 20;


                        EditorGUILayout.EndHorizontal();

                        EditorGUILayout.Space();

                        psSettings.IsPreCachedNativeAdUnits = EditorGUILayout.Toggle("Enable Native Ads Caching: ", psSettings.IsPreCachedNativeAdUnits);

                        EditorGUILayout.Space();

                        psSettings.UseAdaptiveColor = EditorGUILayout.Toggle("Use Adaptive Color: ", psSettings.UseAdaptiveColor);
                        PubEditorUX.DisplayTip("Ensures contrast of Advertiser Logo color from background");

                        EditorGUILayout.Space();

#if UNITY_IOS
                        psSettings.Fallback_NativeAdID_IOS = EditorGUILayout.TextField("Fallback Native AD ID: ", psSettings.Fallback_NativeAdID_IOS);
#else
                        psSettings.Fallback_NativeAdID_Android = EditorGUILayout.TextField("Fallback Native AD ID: ", psSettings.Fallback_NativeAdID_Android);
#endif


                        PubEditorUX.DisplayTip("Used in case there is delay in getting ad config from server");

                        EditorGUILayout.Space();

                    }
                }
                else if (selection == 2)
                {
                    using (new EditorGUILayout.VerticalScope(GUI.skin.box))
                    {
                        if (GUILayout.Button("Integration Docs"))
                        {
                            Application.OpenURL("https://greedygame.github.io/sdkone-unity_native_ads_plugin/");
                        }

                        EditorGUILayout.Space();
                        EditorGUILayout.Space();

                        if (GUILayout.Button("Scripting API"))
                        {
                            Application.OpenURL("https://github.com/GreedyGame/sdkone-unity_native_ads_plugin/wiki");
                        }

                        EditorGUILayout.Space();
                        EditorGUILayout.Space();

                        if (GUILayout.Button("Website"))
                        {
                            Application.OpenURL("https://pubscale.com/ad-revenue-optimization-platform");
                        }
                    }

                }
                else
                {
                    EditorGUILayout.LabelField(@"COMING SOON...", textStyle);
                }

                GUILayout.EndArea();

            }


            EditorGUIUtility.labelWidth = defaultEditorLabelWidth;


        }

    }
}
