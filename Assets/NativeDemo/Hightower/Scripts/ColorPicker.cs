using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PubScale.SdkOne.NativeAds.Hightower
{
    public class ColorPicker : MonoBehaviour
    {
        public SpriteRenderer myRenderer;
        public ParticleSystem particle;
        public int Xoffset;
        public int Yoffset;
        private Color spriteColor;

        void Start()
        {
            Vector2 pos = myRenderer.sprite.rect.position;
            pos = new Vector2(pos.x + Xoffset, pos.y + Yoffset);
            spriteColor = myRenderer.sprite.texture.GetPixel((int)pos.x, (int)pos.y);
            var FSParticleMain = particle.main;
            FSParticleMain.startColor = new Color(spriteColor.r, spriteColor.g, spriteColor.b, 1);

        }
    }
}
