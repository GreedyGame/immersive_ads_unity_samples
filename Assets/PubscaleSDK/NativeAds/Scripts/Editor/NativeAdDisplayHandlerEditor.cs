using UnityEngine;
using UnityEditor;

namespace PubScale.SdkOne.NativeAds
{

    [CustomEditor(typeof(NativeAdDisplayHandler))]
    public class NativeAdDisplayHandlerEditor : Editor
    {

        protected NativeAdDisplayHandler targetDisplayer;
        protected GUIStyle styleValid;
        protected GUIStyle styleInvalid;

        protected SerializedProperty Prop_TemplateID;

        protected SerializedProperty Prop_AdTagImg;
        protected SerializedProperty Prop_AdIconHolder;
        protected SerializedProperty Prop_AdIconImg;
        protected SerializedProperty Prop_AdChoicesImg;
        protected SerializedProperty Prop_ImageTextures;

        protected SerializedProperty Prop_AdHeadlineTxt;
        protected SerializedProperty Prop_AdCallToActionTxt;
        protected SerializedProperty Prop_AdAdvertiserTxt;
        protected SerializedProperty Prop_BodyTxt;
        protected SerializedProperty Prop_PriceTxt;
        protected SerializedProperty Prop_StoreTxt;


        protected SerializedProperty Prop_StarRatingTxt;
        protected SerializedProperty Prop_StarRatingStroke;
        protected SerializedProperty Prop_StarRatingFill;


        protected SerializedProperty Prop_AdIconDarkBgTint;
        protected SerializedProperty Prop_AdIconLightBgTint;



        protected SerializedProperty Prop_Show_AdIconHolder;
        protected SerializedProperty Prop_Show_AdIconImg;
        protected SerializedProperty Prop_Show_AdChoicesImg;
        protected SerializedProperty Prop_Show_ImageTextures;

        protected SerializedProperty Prop_Show_AdHeadlineTxt;
        protected SerializedProperty Prop_Show_AdCallToActionTxt;
        protected SerializedProperty Prop_Show_AdAdvertiserTxt;
        protected SerializedProperty Prop_Show_BodyTxt;
        protected SerializedProperty Prop_Show_PriceTxt;
        protected SerializedProperty Prop_Show_StoreTxt;
        protected SerializedProperty Prop_Show_StarRating;
        protected SerializedProperty Prop_Show_StarRatingAsText;
        protected SerializedProperty Prop_Show_StarRatingAsVisual;


        protected PubEditorUXState prevEditorGUIState = new PubEditorUXState();


        protected bool foldableOptionalTextFields = true;
        protected bool foldableOptionalVisualFields = true;
        protected bool foldableShowStarRating = true;
        protected bool foldableCustomizeVisuals = true;


        public virtual void OnEnable()
        {
            targetDisplayer = (NativeAdDisplayHandler)target;

            styleValid = new GUIStyle();
            styleValid.normal.textColor = new Color(0f, 0.5f, 0f, 1f);

            styleInvalid = new GUIStyle();
            styleInvalid.normal.textColor = new Color(0.6f, 0f, 0f, 1f);

            Prop_TemplateID = serializedObject.FindProperty(nameof(targetDisplayer.DisplayTemplateID));

            Prop_AdTagImg = serializedObject.FindProperty(nameof(targetDisplayer.adTagImg));
            Prop_AdIconHolder = serializedObject.FindProperty(nameof(targetDisplayer.adIconImgHolder));
            Prop_AdIconImg = serializedObject.FindProperty(nameof(targetDisplayer.adIconImg));
            Prop_AdChoicesImg = serializedObject.FindProperty(nameof(targetDisplayer.adChoicesImg));
            Prop_ImageTextures = serializedObject.FindProperty(nameof(targetDisplayer.imageTextures));
            Prop_AdHeadlineTxt = serializedObject.FindProperty(nameof(targetDisplayer.adHeadlineTxt));
            Prop_AdCallToActionTxt = serializedObject.FindProperty(nameof(targetDisplayer.adCallToActionTxt));
            Prop_AdAdvertiserTxt = serializedObject.FindProperty(nameof(targetDisplayer.adAdvertiserTxt));
            Prop_BodyTxt = serializedObject.FindProperty(nameof(targetDisplayer.bodyTxt));
            Prop_PriceTxt = serializedObject.FindProperty(nameof(targetDisplayer.priceTxt));
            Prop_StoreTxt = serializedObject.FindProperty(nameof(targetDisplayer.storeTxt));

            Prop_StarRatingTxt = serializedObject.FindProperty(nameof(targetDisplayer.starRatingTxt));
            Prop_StarRatingStroke = serializedObject.FindProperty(nameof(targetDisplayer.starRatingStroke));
            Prop_StarRatingFill = serializedObject.FindProperty(nameof(targetDisplayer.starRatingFill));

            Prop_AdIconDarkBgTint = serializedObject.FindProperty(nameof(targetDisplayer.AdIconDarkBgTint));
            Prop_AdIconLightBgTint = serializedObject.FindProperty(nameof(targetDisplayer.AdIconLightBgTint));

            Prop_Show_AdIconImg = serializedObject.FindProperty(nameof(targetDisplayer.Show_adIconImg));
            Prop_Show_ImageTextures = serializedObject.FindProperty(nameof(targetDisplayer.Show_imageTextures));

            Prop_Show_AdCallToActionTxt = serializedObject.FindProperty(nameof(targetDisplayer.Show_adCallToActionTxt));
            Prop_Show_AdAdvertiserTxt = serializedObject.FindProperty(nameof(targetDisplayer.Show_adAdvertiserTxt));
            Prop_Show_BodyTxt = serializedObject.FindProperty(nameof(targetDisplayer.Show_bodyTxt));
            Prop_Show_PriceTxt = serializedObject.FindProperty(nameof(targetDisplayer.Show_priceTxt));
            Prop_Show_StoreTxt = serializedObject.FindProperty(nameof(targetDisplayer.Show_storeTxt));
            Prop_Show_StarRating = serializedObject.FindProperty(nameof(targetDisplayer.Show_StarRating));
            Prop_Show_StarRatingAsText = serializedObject.FindProperty(nameof(targetDisplayer.Show_RatingAsText));
            Prop_Show_StarRatingAsVisual = serializedObject.FindProperty(nameof(targetDisplayer.Show_RatingAsVisual));
        }


        public override void OnInspectorGUI()
        {
            // DrawDefaultInspector();

            //--------------------------------

            PubEditorUX.Start_CustomEditor(serializedObject, prevEditorGUIState);

            EditorGUILayout.Space();

            if (Prop_AdTagImg.objectReferenceValue == null)
            {
                targetDisplayer.SetAdTagImgReference();
            }

            PubEditorUX.DisplayProperty(Prop_TemplateID, new GUIContent("Template ID:"));

            using (new EditorGUILayout.VerticalScope(GUI.skin.box))
            {
                PubEditorUX.DisplayHeading("REQUIRED AD FIELDS");

                PubEditorUX.DisplayProperty(Prop_AdHeadlineTxt, new GUIContent("Ad Headline Text:"));

                PubEditorUX.DisplayProperty(Prop_AdTagImg, new GUIContent("Ad Attribution Tag:"));

                PubEditorUX.DisplayProperty(Prop_AdChoicesImg, new GUIContent("Ad Choices Icon:"));

                // if (Prop_Show_AdCallToActionTxt.boolValue)
                PubEditorUX.DisplayProperty(Prop_AdCallToActionTxt, new GUIContent("AD CTA Text:"));

                // PubEditorUX.SetGraphicState(targetDisplayer.adCallToActionTxt, true);// Prop_Show_AdCallToActionTxt.boolValue);
            }

            EditorGUILayout.Space();

            using (new EditorGUILayout.VerticalScope(GUI.skin.box))
            {
                PubEditorUX.DisplayHeading("OPTIONAL AD MEDIA FIELDS");

                foldableOptionalVisualFields = EditorGUILayout.BeginFoldoutHeaderGroup(foldableOptionalVisualFields, "CHOOSE FIELDS");

                if (foldableOptionalVisualFields)
                {
                    using (new EditorGUILayout.VerticalScope(GUI.skin.button))
                    {
                        EditorGUILayout.Space();

                        EditorGUILayout.BeginHorizontal();

                        PubEditorUX.ReduceLabelWidthBy(20);

                        PubEditorUX.DisplayToggle(Prop_Show_AdIconImg, new GUIContent("Show Advertiser Logo"));
                        PubEditorUX.DisplayToggle(Prop_Show_ImageTextures, new GUIContent("Show Product Image"));

                        PubEditorUX.AddToLabelWidth(20);

                        EditorGUILayout.EndHorizontal();

                        EditorGUILayout.Space();

                    }
                }

                EditorGUILayout.EndFoldoutHeaderGroup();


                EditorGUILayout.Space();

                if (Prop_Show_AdIconImg.boolValue)
                {
                    PubEditorUX.DisplayProperty(Prop_AdIconImg, new GUIContent("AD Icon (Brand Logo):"));
                    PubEditorUX.DisplayProperty(Prop_AdIconHolder, new GUIContent("AD Icon BG (Logo Bg):"));
                }

                PubEditorUX.SetGraphicState(targetDisplayer.adIconImg, Prop_Show_AdIconImg.boolValue);
                PubEditorUX.SetGraphicState(targetDisplayer.adIconImgHolder, Prop_Show_AdIconImg.boolValue);

                EditorGUILayout.Space();

                if (Prop_Show_ImageTextures.boolValue)
                    PubEditorUX.DisplayProperty(Prop_ImageTextures, new GUIContent("Big Image (Product):"));

                PubEditorUX.SetGraphicState(targetDisplayer.imageTextures, Prop_Show_ImageTextures.boolValue);

            }

            EditorGUILayout.Space();

            using (new EditorGUILayout.VerticalScope(GUI.skin.box))
            {
                PubEditorUX.DisplayHeading("OPTIONAL AD TEXT FIELDS");

                GUILayout.FlexibleSpace();

                foldableOptionalTextFields = EditorGUILayout.BeginFoldoutHeaderGroup(foldableOptionalTextFields, "CHOOSE FIELDS");

                if (foldableOptionalTextFields)
                {
                    using (new EditorGUILayout.VerticalScope(GUI.skin.button))
                    {

                        EditorGUILayout.Space();

                        // PubEditorUX.ReduceLabelWidthBy(80);
                        // PubEditorUX.DisplayToggle(Prop_Show_AdCallToActionTxt, new GUIContent("Show CTA:"));
                        // PubEditorUX.AddToLabelWidth(80);

                        GUILayout.FlexibleSpace();

                        EditorGUILayout.BeginHorizontal();

                        PubEditorUX.ReduceLabelWidthBy(20);

                        PubEditorUX.DisplayToggle(Prop_Show_AdAdvertiserTxt, new GUIContent("Show Advertiser:"));
                        PubEditorUX.DisplayToggle(Prop_Show_BodyTxt, new GUIContent("Show Body Text:"));

                        PubEditorUX.AddToLabelWidth(20);

                        EditorGUILayout.EndHorizontal();


                        EditorGUILayout.BeginHorizontal();

                        PubEditorUX.ReduceLabelWidthBy(20);

                        PubEditorUX.DisplayToggle(Prop_Show_StoreTxt, new GUIContent("Show Store Text:"));
                        PubEditorUX.DisplayToggle(Prop_Show_PriceTxt, new GUIContent("Show Price Text:"));

                        PubEditorUX.AddToLabelWidth(20);

                        EditorGUILayout.EndHorizontal();

                        EditorGUILayout.Space();

                    }

                }

                EditorGUILayout.EndFoldoutHeaderGroup();

                EditorGUILayout.Space();

                if (Prop_Show_AdAdvertiserTxt.boolValue)
                    PubEditorUX.DisplayProperty(Prop_AdAdvertiserTxt, new GUIContent("AD Advertiser Text:"));

                PubEditorUX.SetGraphicState(targetDisplayer.adAdvertiserTxt, Prop_Show_AdAdvertiserTxt.boolValue);


                if (Prop_Show_BodyTxt.boolValue)
                    PubEditorUX.DisplayProperty(Prop_BodyTxt, new GUIContent("AD Body Text:"));

                PubEditorUX.SetGraphicState(targetDisplayer.bodyTxt, Prop_Show_BodyTxt.boolValue);


                if (Prop_Show_PriceTxt.boolValue)
                    PubEditorUX.DisplayProperty(Prop_PriceTxt, new GUIContent("AD Price Text:"));

                PubEditorUX.SetGraphicState(targetDisplayer.priceTxt, Prop_Show_PriceTxt.boolValue);

                if (Prop_Show_StoreTxt.boolValue)
                    PubEditorUX.DisplayProperty(Prop_StoreTxt, new GUIContent("AD Store Text:"));

                PubEditorUX.SetGraphicState(targetDisplayer.storeTxt, Prop_Show_StoreTxt.boolValue);


            }

            EditorGUILayout.Space();

            foldableShowStarRating = EditorGUILayout.BeginFoldoutHeaderGroup(foldableShowStarRating, "OPTIONAL: SHOW RATINGS");

            if (foldableShowStarRating)
            {
                using (new EditorGUILayout.VerticalScope(GUI.skin.box))
                {
                    PubEditorUX.DisplayHeading("OPTIONAL RATINGS DISPLAY");

                    PubEditorUX.DisplayToggle(Prop_Show_StarRating, new GUIContent("Show Star Rating:"));

                    if (Prop_Show_StarRating.boolValue)
                    {
                        using (new EditorGUILayout.VerticalScope(GUI.skin.button))
                        {
                            EditorGUILayout.BeginHorizontal(GUI.skin.button);

                            bool preGameObjTxt = Prop_Show_StarRatingAsText.boolValue;
                            bool preGameObjVisual = Prop_Show_StarRatingAsVisual.boolValue;

                            PubEditorUX.DisplayToggle(Prop_Show_StarRatingAsText, new GUIContent("Show Rating as Number:"));
                            PubEditorUX.DisplayToggle(Prop_Show_StarRatingAsVisual, new GUIContent("Show Rating as Stars:"));

                            EditorGUILayout.EndHorizontal();

                        }

                        EditorGUILayout.Space();

                        if (Prop_Show_StarRatingAsText.boolValue)
                        {
                            PubEditorUX.DisplayProperty(Prop_StarRatingTxt, new GUIContent("Star Rating Text:"));
                            EditorGUILayout.Space();
                        }

                        if (targetDisplayer.starRatingTxt != null)
                            targetDisplayer.starRatingTxt.gameObject.SetActive(Prop_Show_StarRatingAsText.boolValue);

                        if (Prop_Show_StarRatingAsVisual.boolValue)
                        {
                            PubEditorUX.DisplayProperty(Prop_StarRatingStroke, new GUIContent("Star Rating Stroke:"));
                            PubEditorUX.DisplayProperty(Prop_StarRatingFill, new GUIContent("Star Rating Fill:"));
                        }

                        if (targetDisplayer.starRatingFill != null)
                            targetDisplayer.starRatingFill.gameObject.SetActive(Prop_Show_StarRatingAsVisual.boolValue);

                        if (targetDisplayer.starRatingStroke != null)
                            targetDisplayer.starRatingStroke.gameObject.SetActive(Prop_Show_StarRatingAsVisual.boolValue);
                    }
                    else
                    {

                        if (targetDisplayer.starRatingTxt != null)
                            targetDisplayer.starRatingTxt.gameObject.SetActive(false);

                        if (targetDisplayer.starRatingFill != null)
                            targetDisplayer.starRatingFill.gameObject.SetActive(false);

                        if (targetDisplayer.starRatingStroke != null)
                            targetDisplayer.starRatingStroke.gameObject.SetActive(false);
                    }

                }
            }

            EditorGUILayout.EndFoldoutHeaderGroup();

            EditorGUILayout.Space();

            foldableCustomizeVisuals = EditorGUILayout.BeginFoldoutHeaderGroup(foldableCustomizeVisuals, "OPTIONAL: CUSTOMIZE VISUALS");

            if (foldableCustomizeVisuals)
            {
                using (new EditorGUILayout.VerticalScope(GUI.skin.box))
                {
                    PubEditorUX.DisplayHeading("ADAPTIVE LOGO BG COLORS");

                    PubEditorUX.DisplayColor(Prop_AdIconDarkBgTint, new GUIContent("AdaptCLR: Logo Bg Dark:"));
                    PubEditorUX.DisplayColor(Prop_AdIconLightBgTint, new GUIContent("AdaptCLR: Logo Bg Light:"));
                }
            }

            EditorGUILayout.EndFoldoutHeaderGroup();

            EditorGUILayout.Space();

            PubEditorUX.End_CustomEditor(serializedObject, prevEditorGUIState);

        }
    }
}