using UnityEngine;

namespace SpriteManager
{
    public class CharacterSprite
    {
        private int offset;
        private bool inverted;
        private Sprite sprite;
        private SpriteRenderer renderer;

        public CharacterSprite(Sprite playerSprite, SpriteRenderer spriteRenderer)
        {
            sprite = playerSprite;
            renderer = spriteRenderer;
        }

        public enum Orientation
        {
            So = 0,
            S = 1,
            Se = 2,
            E = 3,
            Ne = 4,
            N = 5,
            No = 6,
            O = 7
        }

        private void Update()
        {
            renderer.flipX = inverted;
            renderer.sprite = sprite;
        }
        
        public void updateOrientation(Orientation orientation)
        {
            var offset = (int) orientation;
            if (orientation >= Orientation.N)
            {
                offset -= 4;
            }
        }
    }
}