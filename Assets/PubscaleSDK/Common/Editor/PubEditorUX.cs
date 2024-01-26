using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;

namespace PubScale.SdkOne
{

    public static class PubEditorUX
    {

        public const string RootPath = "Assets/PubscaleSDK/"; // Make sure to add a trailing /


        #region SDK_SETTINGS

        public static string PackageSettingsPath
        {
            get { return Path.Combine(RootPath, "Common/Resources/PubScaleSettings.asset"); }
        }

        public const string PackageCommonFolder = "Common";
        public const string PackageSettingsFolderPath = RootPath + "Common/Resources";


        public static PubScaleSettings CreateAndSavePubScaleSettings()
        {
            CheckResourcesFolderInCommon();

            PubScaleSettings psSettings = ScriptableObject.CreateInstance<PubScaleSettings>();
            AssetDatabase.CreateAsset(psSettings, PubEditorUX.PackageSettingsPath);
            AssetDatabase.SaveAssets();

            return psSettings;
        }

        #endregion

        public const string imageName = "pubscale_logo_720";

        public const float DEF_LABEL_WIDTH = 160;
        static Color PubScaleGreen = new Color(0.184f, 0.803f, 0.694f);

        public static GUISkin PubScaleSkin { get { if (pubSkin == null) { InitGlobalStyle(); } return pubSkin; } }
        public static GUIStyle PubTitleStyle { get { if (titleStyle == null) { InitGlobalStyle(); } return titleStyle; } }
        public static GUIStyle PubToolTipStyle { get { if (toolTipStyle == null) { InitGlobalStyle(); } return toolTipStyle; } }


        static GUISkin pubSkin;
        static GUIStyle titleStyle;
        static GUIStyle toolTipStyle;


        public static void CheckResourcesFolderInCommon()
        {
            if (!AssetDatabase.IsValidFolder(PubEditorUX.PackageSettingsFolderPath))
            {
                AssetDatabase.CreateFolder(PubEditorUX.RootPath, PubEditorUX.PackageCommonFolder);
                AssetDatabase.CreateFolder(PubEditorUX.RootPath + PubEditorUX.PackageCommonFolder, "Resources");
            }

        }

        public static void Start_CustomEditor(SerializedObject serializedObj, PubEditorUXState prevGUIState)
        {
            if (pubSkin == null) { InitGlobalStyle(); }

            prevGUIState.labelWidth = EditorGUIUtility.labelWidth;
            prevGUIState.backgroundClr = GUI.backgroundColor;
            prevGUIState.contentClr = GUI.contentColor;
            prevGUIState.guiClr = GUI.color;

            EditorGUIUtility.labelWidth = PubEditorUX.DEF_LABEL_WIDTH;

            // GUI.backgroundColor = Color.white;
            GUI.contentColor = PubScaleGreen;

            serializedObj.Update(); // Sync the serialized properties with the gameobject with GameManager component.
        }

        public static void End_CustomEditor(SerializedObject serializedObj, PubEditorUXState startGuiState)
        {
            serializedObj.ApplyModifiedProperties(); // write the potentially changed levelName property back to the gameobject with the GameManager component.   

            EditorGUIUtility.labelWidth = startGuiState.labelWidth;

            GUI.backgroundColor = startGuiState.backgroundClr;
            GUI.contentColor = startGuiState.contentClr;
            GUI.color = startGuiState.guiClr;
        }


        static void InitGlobalStyle()
        {
            if (pubSkin == null)
                pubSkin = (GUISkin)Resources.Load("PubscaleSkin");

            if (titleStyle == null)
                titleStyle = pubSkin.label;

            if (toolTipStyle == null)
                toolTipStyle = pubSkin.customStyles[0];
        }



        public static void SetGraphicState(Graphic img, bool toState)
        {
            if (img != null)
                img.gameObject.SetActive(toState);

        }

        public static void DisplayHeading(string txt)
        {
            EditorGUILayout.LabelField(txt, PubEditorUX.PubTitleStyle);
        }
        public static void DisplayTip(string txt)
        {
            EditorGUILayout.LabelField(txt, PubEditorUX.PubToolTipStyle);
        }

        public static void SetLabelWidth(float width)
        {
            EditorGUIUtility.labelWidth = width;
        }

        public static void AddToLabelWidth(float incr)
        {
            EditorGUIUtility.labelWidth += incr;
        }

        public static void ReduceLabelWidthBy(float reduce)
        {
            EditorGUIUtility.labelWidth -= reduce;
        }

        public static void DisplayToggle(SerializedProperty prop, GUIContent guiContent, GUIStyle style = null, params GUILayoutOption[] options)
        {
            if (style != null)
                prop.boolValue = EditorGUILayout.Toggle(guiContent, prop.boolValue, style, options);
            else
                prop.boolValue = EditorGUILayout.Toggle(guiContent, prop.boolValue, options);
        }

        public static void DisplayInt(SerializedProperty prop, GUIContent guiContent, GUIStyle style = null, params GUILayoutOption[] options)
        {
            if (style != null)
                prop.intValue = EditorGUILayout.IntField(guiContent, prop.intValue, style, options);
            else
                prop.intValue = EditorGUILayout.IntField(guiContent, prop.intValue, options);
        }

        public static void DisplayFloat(SerializedProperty prop, GUIContent guiContent, GUIStyle style = null, params GUILayoutOption[] options)
        {
            if (style != null)
                prop.floatValue = EditorGUILayout.FloatField(guiContent, prop.floatValue, style, options);
            else
                prop.floatValue = EditorGUILayout.FloatField(guiContent, prop.floatValue, options);
        }

        public static void DisplayString(SerializedProperty prop, GUIContent guiContent, GUIStyle style = null, params GUILayoutOption[] options)
        {
            if (style != null)
                prop.stringValue = EditorGUILayout.TextField(guiContent, prop.stringValue, style, options);
            else
                prop.stringValue = EditorGUILayout.TextField(guiContent, prop.stringValue, options);
        }

        public static void DisplayProperty(SerializedProperty prop, GUIContent guiContent, params GUILayoutOption[] options)
        {
            EditorGUILayout.PropertyField(prop, guiContent, options);
        }


        public static void DisplayColor(SerializedProperty prop, GUIContent guiContent, params GUILayoutOption[] options)
        {
            prop.colorValue = EditorGUILayout.ColorField(guiContent, prop.colorValue, options);
        }

        public static List<T> GetListFromEnum<T>()
        {
            List<T> enumList = new List<T>();
            System.Array enums = System.Enum.GetValues(typeof(T));
            foreach (T e in enums)
            {
                enumList.Add(e);
            }
            return enumList;
        }
    }


    public class PubEditorUXState
    {
        public float labelWidth;
        public Color backgroundClr;
        public Color contentClr;
        public Color guiClr;
    }


}
