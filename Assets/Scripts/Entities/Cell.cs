using DefaultNamespace.Managers;
using JetBrains.Annotations;
using UnityEngine;

namespace DefaultNamespace
{
    public class Cell : MonoBehaviour
    {
        private CellEntity _entity;
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
        
        public CellEntity GetEntity() => _entity;
        
        public void SetEntity(CellEntity entity)
        {
            _entity = entity;
            Walkable = entity.IsWalkable;
        }
    }
}