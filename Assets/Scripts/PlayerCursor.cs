using UnityEngine;

namespace DefaultNamespace
{
    public class PlayerCursor : MonoBehaviour
    {
        private SpriteRenderer renderer;
        private Sprite texture;
        private void Awake()
        {
            renderer = GetComponent<SpriteRenderer>();
            texture = renderer.sprite;
        }
        
        public void Hide() => renderer.sprite = null;

        public void Show(Vector3 position)
        {
            gameObject.transform.position = position;
            renderer.sprite = texture;
        }
    }
}