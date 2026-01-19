using Database.Models;
using DefaultNamespace.Managers;
using Entities;
using Unity.VisualScripting;
using UnityEngine;

namespace DefaultNamespace
{
    public class ResourceGameEntity : GameEntity, Interactable
    {
        public bool depleted;
        private SpriteRenderer spriteRenderer;
        private float respawnTimeRemaining;
        private PolygonCollider2D collider;
        [SerializeField]private Resource resource;
       
        private void Awake()
        {
            IsInteractable = true;
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.material.SetFloat("_Outline_thickness", 0f);
            this.AddComponent<PolygonCollider2D>();
            collider = GetComponent<PolygonCollider2D>();
            collider.layerOverridePriority = 1;
            collider.points = resource.colliderDefault;
        }

        private void Deplete()
        {
            spriteRenderer.sprite = resource.depletedSprite;
            depleted = true;
            respawnTimeRemaining = resource.respawnTime;
            collider.points = resource.colliderDepleted;
        }
    
        private void Regenerate()
        {
            spriteRenderer.sprite = resource.sprite;
            depleted = false;
            collider.points = resource.colliderDefault;
        }

        public void Hover()
        {
            spriteRenderer.material.SetFloat("_Outline_thickness", 0.1f);
        }

        public void RemoveHover()
        {
            spriteRenderer.material.SetFloat("_Outline_thickness", 0f);
        }
    
        public void Interact()
        {
            if (depleted) return;
            GameManager.Instance.inventoryManager.Add(resource.Item, 2);
            Deplete();
        }

        public void Click()
        {
            GameManager.Instance.playerInstance.InteractWithEntity(new Vector2Int(posX, posY), this);
        }

        private void Update()
        {
            if (depleted)
            {
                respawnTimeRemaining -= Time.deltaTime;
                if (respawnTimeRemaining <= 0)
                {
                    Regenerate();
                }
            }
        }
    }
}