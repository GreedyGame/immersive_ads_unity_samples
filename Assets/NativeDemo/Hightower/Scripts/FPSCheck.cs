using System.Collections.Generic;
using TMPro;
using UnityEngine;
namespace PubScale.SdkOne.NativeAds.Hightower
{
    public class FPSCheck : MonoBehaviour
    {
        private float deltaTime;
        private int fps;

        void Update()
        {
            deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        }

        void OnGUI()
        {
            fps = Mathf.RoundToInt(1.0f / deltaTime);
            string text = $"FPS: {fps}";

            GUIStyle style = new GUIStyle();
            style.alignment = TextAnchor.UpperLeft;
            style.fontSize = 100;
            style.normal.textColor = Color.white;

            GUI.Label(new Rect(50, 100, 100, 20), text, style);
        }
    }
}