using DefaultNamespace.Managers;
using Entities;
using JetBrains.Annotations;
using UnityEngine;

namespace DefaultNamespace
{
    public class Cell : MonoBehaviour
    {
        private GameEntity gameEntity;
        public Vector3Int tilePosition;
        private bool isSelected;

        public bool Walkable
        {
            get => GetComponent<SpriteRenderer>().enabled;
            set
            {
                GetComponent<SpriteRenderer>().enabled = value;
            }
        }
        
        public GameEntity GetEntity() => gameEntity;
        
        public void SetEntity(GameEntity gameEntity)
        {
            this.gameEntity = gameEntity;
            Walkable = false;
        }
    }
}