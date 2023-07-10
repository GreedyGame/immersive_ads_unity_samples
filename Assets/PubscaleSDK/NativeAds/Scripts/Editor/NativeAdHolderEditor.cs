using UnityEngine;
using UnityEditor;

namespace PubScale.SdkOne.NativeAds
{

    [CustomEditor(typeof(NativeAdHolder))]
    public class NativeAdHolderEditor : Editor
    {

        NativeAdHolder targetHolder;
        protected GUIStyle styleValid;
        protected GUIStyle styleInvalid;

        // SerializedProperty Prop_AdID;
        SerializedProperty Prop_AdTag;
        SerializedProperty Prop_Placeholder;
        SerializedProperty Prop_AdDisplayHandler;

        SerializedProperty Prop_MoreDisplayLandscape;
        SerializedProperty Prop_MoreDisplayPotrait;
        SerializedProperty Prop_MoreDisplayNoBigMedia;

        SerializedProperty Prop_canvas;
        SerializedProperty Prop_AutoFetch;
        SerializedProperty Prop_TriggerFetch;
        SerializedProperty Prop_TriggerTag;
        SerializedProperty Prop_UsePreCachedAds;
        SerializedProperty Prop_RefreshDelay;
        PubEditorUXState prevEditorGUIState = new PubEditorUXState();

        bool foldableExtraFormats = false;
        public void OnEnable()
        {
            targetHolder = (NativeAdHolder)target;

            styleValid = new GUIStyle();
            styleValid.normal.textColor = new Color(0f, 0.5f, 0f, 1f);

            styleInvalid = new GUIStyle();
            styleInvalid.normal.textColor = new Color(0.6f, 0f, 0f, 1f);

            //  Prop_AdID = serializedObject.FindProperty(nameof(targetHolder.adId));
            Prop_AdTag = serializedObject.FindProperty(nameof(targetHolder.adTag));
            Prop_Placeholder = serializedObject.FindProperty(nameof(targetHolder.placeholder));
            Prop_AdDisplayHandler = serializedObject.FindProperty(nameof(targetHolder.adDisplay));
            Prop_canvas = serializedObject.FindProperty(nameof(targetHolder.canvas));
            Prop_AutoFetch = serializedObject.FindProperty(nameof(targetHolder.AutoFetch));
            Prop_TriggerFetch = serializedObject.FindProperty(nameof(targetHolder.TriggerFetchWithCollider));
            Prop_TriggerTag = serializedObject.FindProperty(nameof(targetHolder.TriggerOnCollisionWithTag));
            Prop_UsePreCachedAds = serializedObject.FindProperty(nameof(targetHolder.UsePreCachedAds));
            Prop_RefreshDelay = serializedObject.FindProperty(nameof(targetHolder.RefreshDelay));

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
                //  PubEditorUX.DisplayString(Prop_AdID, new GUIContent("Ad Unit ID:"));
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

                foldableExtraFormats = EditorGUILayout.Toggle("ADD EXTRA DISPLAY FORMATS:", foldableExtraFormats);

                if (foldableExtraFormats)
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

                PubEditorUX.DisplayToggle(Prop_UsePreCachedAds, new GUIContent("Use Pre-Cached Ads From Manager:"));
                PubEditorUX.DisplayFloat(Prop_RefreshDelay, new GUIContent("Refresh time after Impression: "));
                PubEditorUX.DisplayTip("Recommended Refresh Delay: 30s or more");

            }

            EditorGUILayout.Space();

            PubEditorUX.End_CustomEditor(serializedObject, prevEditorGUIState);

            // EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            // EditorGUILayout.HelpBox("Native Ad Holder", MessageType.Info);

        }
    }
}
