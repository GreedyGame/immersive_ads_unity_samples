using UnityEditor;
using UnityEngine;

namespace PubScale.SdkOne.NativeAds
{

    [CustomEditor(typeof(DynamicADFormatHandler))]
    public class DynamicADFormatHandlerEditor : NativeAdDisplayHandlerEditor
    {

        protected DynamicADFormatHandler targetDynamicDisplay;

        protected SerializedProperty Prop_ctaStars;
        protected SerializedProperty Prop_starAdvertiser;
        protected SerializedProperty Prop_starTypeText;
        protected SerializedProperty Prop_starTypeVisuals;
        protected SerializedProperty Prop_singleStarImg;
        protected SerializedProperty Prop_ctaHolderImg;

        protected SerializedProperty Prop_DisableAnimations;
        protected SerializedProperty Prop_NumberOfAnimations;
        protected SerializedProperty Prop_animator;


        protected SerializedProperty Prop_defaultFont;
        protected SerializedProperty Prop_OverrideColors;
        protected SerializedProperty Prop_starsColor;
        protected SerializedProperty Prop_ctaButtonColor;
        protected SerializedProperty Prop_textColor;


#if PUBSCALE_EFFECTS
    protected SerializedProperty Prop_useShineEffect;
#endif


        public override void OnEnable()
        {
            base.OnEnable();

            targetDynamicDisplay = (DynamicADFormatHandler)target;

            Prop_ctaStars = serializedObject.FindProperty(nameof(targetDynamicDisplay.ctaStars));
            Prop_starAdvertiser = serializedObject.FindProperty(nameof(targetDynamicDisplay.starAdvertiser));
            Prop_starTypeText = serializedObject.FindProperty(nameof(targetDynamicDisplay.starTypeText));
            Prop_starTypeVisuals = serializedObject.FindProperty(nameof(targetDynamicDisplay.starTypeVisuals));

            Prop_singleStarImg = serializedObject.FindProperty(nameof(targetDynamicDisplay.singleStarImg));
            Prop_ctaHolderImg = serializedObject.FindProperty(nameof(targetDynamicDisplay.ctaHolderImg));

            Prop_DisableAnimations = serializedObject.FindProperty(nameof(targetDynamicDisplay.DisableAnimation));
            Prop_NumberOfAnimations = serializedObject.FindProperty(nameof(targetDynamicDisplay.NumberOfAnimations));
            Prop_animator = serializedObject.FindProperty(nameof(targetDynamicDisplay.animator));


            Prop_defaultFont = serializedObject.FindProperty(nameof(targetDynamicDisplay.defaultFont));

            Prop_OverrideColors = serializedObject.FindProperty(nameof(targetDynamicDisplay.OverrideColors));
            Prop_starsColor = serializedObject.FindProperty(nameof(targetDynamicDisplay.starsColor));

            Prop_ctaButtonColor = serializedObject.FindProperty(nameof(targetDynamicDisplay.ctaButtonColor));
            Prop_textColor = serializedObject.FindProperty(nameof(targetDynamicDisplay.textColor));


#if PUBSCALE_EFFECTS
            Prop_useShineEffect = serializedObject.FindProperty(nameof(targetDynamicDisplay.useShineEffect));
#endif

        }

        bool foldableBase;

        public override void OnInspectorGUI()
        {
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            bool prevShowStarRating = Prop_Show_StarRating.boolValue;
            bool prevShowStartRatingText = Prop_Show_StarRatingAsText.boolValue;
            bool prevShowStartRatingVisual = Prop_Show_StarRatingAsVisual.boolValue;

            base.OnInspectorGUI();

            EditorGUILayout.Separator();

            PubEditorUX.Start_CustomEditor(serializedObject, prevEditorGUIState);


            bool curShowStarRating = Prop_Show_StarRating.boolValue;
            bool curShowStartRatingText = Prop_Show_StarRatingAsText.boolValue;
            bool curShowStartRatingVisual = Prop_Show_StarRatingAsVisual.boolValue;

            if (curShowStarRating != prevShowStarRating || curShowStartRatingText != prevShowStartRatingText && curShowStartRatingVisual != prevShowStartRatingVisual)
            {

                if (targetDynamicDisplay.starTypeText != null)
                    targetDynamicDisplay.starTypeText.gameObject.SetActive(curShowStarRating && curShowStartRatingText);

                if (targetDynamicDisplay.singleStarImg != null)
                    PubEditorUX.SetGraphicState(targetDynamicDisplay.singleStarImg, curShowStarRating && curShowStartRatingText);


                if (targetDynamicDisplay.starTypeVisuals != null)
                    targetDynamicDisplay.starTypeVisuals.gameObject.SetActive(curShowStarRating && curShowStartRatingVisual);

                // if (curShowStartRatingText && curShowStartRatingVisual)
                // {
                //     if (targetDynamicDisplay.starTypeText != null)
                //         targetDynamicDisplay.starTypeText.gameObject.SetActive(false);

                //     if (targetDynamicDisplay.starTypeVisuals != null)
                //         targetDynamicDisplay.starTypeVisuals.gameObject.SetActive(true);
                // }

                EditorUtility.SetDirty(targetDynamicDisplay);
            }


            EditorGUILayout.Space();
            EditorGUILayout.Space();

            int labelWidthChange = 70;

            PubEditorUX.AddToLabelWidth(labelWidthChange);

            using (new EditorGUILayout.VerticalScope(GUI.skin.button))
            {
                PubEditorUX.DisplayHeading("DYNAMIC DISPLAY EXTENSIONS");

                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUILayout.Space();

                using (new EditorGUILayout.VerticalScope(GUI.skin.box))
                {
                    PubEditorUX.DisplayTip("HLG - Horizontal Layout Group containing CTA and STAR RATING");

                    EditorGUILayout.Space(5);
                    EditorGUILayout.Space(5);

                    PubEditorUX.DisplayProperty(Prop_ctaStars, new GUIContent("HLG PARENT: CTA & STAR RATING"));

                    EditorGUILayout.Space(5);

                    PubEditorUX.DisplayProperty(Prop_ctaHolderImg, new GUIContent("CTA Holder Image"));

                    EditorGUILayout.Space(5);

                    using (new EditorGUILayout.VerticalScope(GUI.skin.box))
                    {
                        PubEditorUX.DisplayTip("STAR RATING DISPLAY");
                        EditorGUILayout.Space(5);
                        PubEditorUX.DisplayProperty(Prop_starAdvertiser, new GUIContent("VLG Parent: Star Ratings"));


                        EditorGUILayout.Space(5);
                        PubEditorUX.DisplayTip("Display Rating as Text with single star image. E.g. 4*");
                        PubEditorUX.DisplayProperty(Prop_starTypeText, new GUIContent("Parent: Star Rating Text Display"));
                        PubEditorUX.DisplayProperty(Prop_singleStarImg, new GUIContent("Star Image Next to Text"));

                        EditorGUILayout.Space(10);
                        PubEditorUX.DisplayTip("Display Rating as a Star Bar. E.g. ****");
                        PubEditorUX.DisplayProperty(Prop_starTypeVisuals, new GUIContent("Parent: Star Rating Bar Display"));
                    }

                    EditorGUILayout.Space(15);

                    using (new EditorGUILayout.VerticalScope(GUI.skin.box))
                    {
                        PubEditorUX.DisplayTip("FONT AND COLORS");
                        PubEditorUX.DisplayProperty(Prop_defaultFont, new GUIContent("Default Font:"));
                        PubEditorUX.DisplayProperty(Prop_OverrideColors, new GUIContent("Override Colors:"));
                        PubEditorUX.DisplayProperty(Prop_starsColor, new GUIContent("Star Colors:"));
                        PubEditorUX.DisplayProperty(Prop_ctaButtonColor, new GUIContent("Button Colors:"));
                        PubEditorUX.DisplayProperty(Prop_textColor, new GUIContent("Text Color:"));


                        EditorGUILayout.Space(10);
                        PubEditorUX.DisplayTip("ANIMATIONS");
                        PubEditorUX.DisplayProperty(Prop_DisableAnimations, new GUIContent("Disable Animations:"));

                        if (Prop_DisableAnimations.boolValue == false)
                        {
                            PubEditorUX.DisplayProperty(Prop_animator, new GUIContent("Animator:"));
                            PubEditorUX.DisplayProperty(Prop_NumberOfAnimations, new GUIContent("Number of Animations in Animator:"));
                        }

                        EditorGUILayout.Space(10);
                    }



                }






#if PUBSCALE_EFFECTS
                    PubEditorUX.DisplayProperty(Prop_useShineEffect, new GUIContent("Use Shine Effect:"));
#endif
            }

            PubEditorUX.ReduceLabelWidthBy(labelWidthChange);

            EditorGUILayout.Space();
            EditorGUILayout.Space();


            PubEditorUX.End_CustomEditor(serializedObject, prevEditorGUIState);


        }


    }
}