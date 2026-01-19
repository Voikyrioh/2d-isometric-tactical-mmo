using UnityEngine;

namespace Database.Models
{
    [CreateAssetMenu(menuName = "Entities/Ressource")]
    public class Resource : ScriptableObject
    {
        [SerializeField] public float respawnTime;
        [SerializeField] public int levelRequirement;
        [SerializeField] public Sprite sprite;
        [SerializeField] public Sprite depletedSprite;
        [SerializeField] public Vector2[] colliderDefault;
        [SerializeField] public Vector2[] colliderDepleted;
        [SerializeField] private Item item;
        public Item Item { get => item; }
    }
}
