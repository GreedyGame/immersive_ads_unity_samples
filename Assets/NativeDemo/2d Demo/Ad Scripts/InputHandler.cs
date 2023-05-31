using UnityEngine;

namespace PubScale.SdkOne.Sample
{

    /// <summary>
    /// Handles the on screen button events
    /// </summary>
    public class InputHandler : MonoBehaviour
    {
        public float HorizontalAxis = 0;
        public bool jumpPress = false;

        private void Awake()
        {
            Input.multiTouchEnabled = true;
        }
        public void SetHorizontalAxis(float value)
        {
            HorizontalAxis = value;
        }
        public void SetButtonState(bool ans)
        {
            jumpPress = ans;
        }
    }
}
