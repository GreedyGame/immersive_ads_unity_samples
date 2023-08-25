using UnityEngine;
using UnityEditor;

namespace PubScale.SdkOne.NativeAds
{

    [CustomEditor(typeof(NativeAdHolder))]
    public class NativeAdHolderEditor : Editor
    {

        protected NativeAdHolder targetHolder;
        protected GUIStyle styleValid;
        protected GUIStyle styleInvalid;

        // SerializedProperty Prop_AdID;
        protected SerializedProperty Prop_AdTag;
        protected SerializedProperty Prop_Placeholder;
        protected SerializedProperty Prop_AdDisplayHandler;

        protected SerializedProperty Prop_UseExtraFormats;
        protected SerializedProperty Prop_MoreDisplayLandscape;
        protected SerializedProperty Prop_MoreDisplayPotrait;
        protected SerializedProperty Prop_MoreDisplayNoBigMedia;

        protected SerializedProperty Prop_canvas;
        protected SerializedProperty Prop_AutoFetch;
        protected SerializedProperty Prop_TriggerFetch;
        protected SerializedProperty Prop_TriggerTag;
        protected SerializedProperty Prop_UsePriorityCache;
        protected SerializedProperty Prop_RefreshDelay;
        protected SerializedProperty Prop_Retries;

        protected PubEditorUXState prevEditorGUIState = new PubEditorUXState();

        public void OnEnable()
        {
            targetHolder = (NativeAdHolder)target;

            styleValid = new GUIStyle();
            styleValid.normal.textColor = new Color(0f, 0.5f, 0f, 1f);

            styleInvalid = new GUIStyle();
            styleInvalid.normal.textColor = new Color(0.6f, 0f, 0f, 1f);

            Prop_AdTag = serializedObject.FindProperty(nameof(targetHolder.adTag));
            Prop_Placeholder = serializedObject.FindProperty(nameof(targetHolder.placeholder));
            Prop_AdDisplayHandler = serializedObject.FindProperty(nameof(targetHolder.adDisplay));
            Prop_canvas = serializedObject.FindProperty(nameof(targetHolder.canvas));
            Prop_AutoFetch = serializedObject.FindProperty(nameof(targetHolder.AutoFetch));
            Prop_TriggerFetch = serializedObject.FindProperty(nameof(targetHolder.TriggerFetchWithCollider));
            Prop_TriggerTag = serializedObject.FindProperty(nameof(targetHolder.TriggerOnCollisionWithTag));
            Prop_UsePriorityCache = serializedObject.FindProperty(nameof(targetHolder.UsePriorityCache));
            Prop_RefreshDelay = serializedObject.FindProperty(nameof(targetHolder.RefreshDelay));
            Prop_Retries = serializedObject.FindProperty(nameof(targetHolder.Retries));

            Prop_UseExtraFormats = serializedObject.FindProperty(nameof(targetHolder.UseExtraFormats));
            Prop_MoreDisplayLandscape = serializedObject.FindProperty(nameof(targetHolder.landscapeAdFormats));
            Prop_MoreDisplayPotrait = serializedObject.FindProperty(nameof(targetHolder.potraitAdFormats));
            Prop_MoreDisplayNoBigMedia = serializedObject.FindProperty(nameof(targetHolder.nonMediaType));

        }


        public override void OnInspectorGUI()
        {
            // DrawDefaultInspector();

            PubEditorUX.Start_CustomEditor(serializedObject, prevEditorGUIState);

            EditorGUILayout.Space();

            using (new EditorGUILayout.VerticalScope(GUI.skin.box))
            {
                PubEditorUX.DisplayHeading("AD PLACEMENT INFO");

                PubEditorUX.DisplayString(Prop_AdTag, new GUIContent("Ad Placement Tag:"));

                EditorGUILayout.Space();
                PubEditorUX.DisplayToggle(Prop_UsePriorityCache, new GUIContent("Is On Screen For Short Time:"));
                PubEditorUX.DisplayTip("Enable this for short time placements~ e.g. Level loading screen");
                PubEditorUX.DisplayTip("1. Placement is given priority in the caching system.");
                PubEditorUX.DisplayTip("2. If Cached ad is available, it will be shown instantly.");
                PubEditorUX.DisplayTip("3. New ad request and retry logic will NOT be used.");

            }

            EditorGUILayout.Space();

            using (new EditorGUILayout.VerticalScope(GUI.skin.box))
            {
                PubEditorUX.DisplayHeading("REFERENCES");

                PubEditorUX.DisplayProperty(Prop_canvas, new GUIContent("Parent Canvas:"), GUILayout.MaxWidth(600));
                PubEditorUX.DisplayProperty(Prop_Placeholder, new GUIContent("Placeholder Before Ad Fill"), GUILayout.MaxWidth(600));

                EditorGUILayout.Space();

                PubEditorUX.AddToLabelWidth(20);

                PubEditorUX.DisplayProperty(Prop_AdDisplayHandler, new GUIContent("DEFAULT DISPLAY FORMAT:"), GUILayout.MaxWidth(600));

                PubEditorUX.ReduceLabelWidthBy(20);

                EditorGUILayout.Space();
                EditorGUILayout.Space();

                PubEditorUX.AddToLabelWidth(40);

                Prop_UseExtraFormats.boolValue = EditorGUILayout.Toggle("ADD EXTRA DISPLAY FORMATS:", Prop_UseExtraFormats.boolValue);

                if (Prop_UseExtraFormats.boolValue)
                {
                    EditorGUILayout.Space();

                    using (new EditorGUILayout.VerticalScope(GUI.skin.button))
                    {
                        EditorGUI.indentLevel++;

                        PubEditorUX.DisplayProperty(Prop_MoreDisplayLandscape, new GUIContent("Ads with Landscape Product Image"), GUILayout.MaxWidth(600));
                        PubEditorUX.DisplayProperty(Prop_MoreDisplayPotrait, new GUIContent("Ads with Portrait Product Image"), GUILayout.MaxWidth(600));
                        PubEditorUX.DisplayProperty(Prop_MoreDisplayNoBigMedia, new GUIContent("Ads without Product Image"), GUILayout.MaxWidth(600));

                        EditorGUI.indentLevel--;
                    }
                }

                PubEditorUX.ReduceLabelWidthBy(40);

            }

            EditorGUILayout.Space();

            using (new EditorGUILayout.VerticalScope(GUI.skin.box))
            {
                PubEditorUX.DisplayHeading("AD FETCH TRIGGER MECHANISM");

                PubEditorUX.AddToLabelWidth(60);

                bool preAutoFetchValue = Prop_AutoFetch.boolValue;
                bool preTriggerFetchVal = Prop_TriggerFetch.boolValue;

                PubEditorUX.DisplayToggle(Prop_AutoFetch, new GUIContent("Auto Fetch AD in OnEnable:"));
                PubEditorUX.DisplayToggle(Prop_TriggerFetch, new GUIContent("Trigger Fetch AD with Collider:"));

                bool curAutoFetchValue = Prop_AutoFetch.boolValue;
                bool curTriggerFetchVal = Prop_TriggerFetch.boolValue;

                if (preAutoFetchValue != curAutoFetchValue)
                {
                    if (curAutoFetchValue)
                    {
                        if (curTriggerFetchVal)
                            Prop_TriggerFetch.boolValue = false;
                    }
                }

                if (preTriggerFetchVal != curTriggerFetchVal)
                {
                    if (curTriggerFetchVal)
                    {
                        if (curAutoFetchValue)
                            Prop_AutoFetch.boolValue = false;
                    }
                }

                if (Prop_TriggerFetch.boolValue)
                    PubEditorUX.DisplayString(Prop_TriggerTag, new GUIContent("Trigger On Collision With Tag:"));

                if (Prop_AutoFetch.boolValue == false && Prop_TriggerFetch.boolValue == false)
                    PubEditorUX.DisplayTip("You will call FetchAd() from external script");

            }

            EditorGUILayout.Space();

            using (new EditorGUILayout.VerticalScope(GUI.skin.box))
            {
                PubEditorUX.DisplayHeading("AD BEHAVIOUR");

                PubEditorUX.DisplayFloat(Prop_RefreshDelay, new GUIContent("Refresh time after Impression: "));
                PubEditorUX.DisplayTip("To Disable Refresh use -1");
                PubEditorUX.DisplayInt(Prop_Retries, new GUIContent("Retries on Ad Fail: "));
            }

            EditorGUILayout.Space();

            PubEditorUX.End_CustomEditor(serializedObject, prevEditorGUIState);

            // EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            // EditorGUILayout.HelpBox("Native Ad Holder", MessageType.Info);

        }
    }
}
